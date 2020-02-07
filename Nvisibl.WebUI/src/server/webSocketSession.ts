import { Subject, BehaviorSubject } from 'rxjs';
import messageParser from './messages/messageParser';
import User from '../models/user';
import ClientMessage from './messages/client/clientMessage';
import ServerMessage from './messages/server/serverMessage';
import ConnectedMessage from './messages/server/connected';
import ConnectionRequest from './messages/client/connectionRequest';

export enum WebSocketConnectionState {
    Disconnected,
    Connected,
    Closed,
    Error,
}

export default class WebSocketSession {
    private readonly ConnectionRetryInterval: number = 5000;

    private readonly _address: string;

    private readonly _user: User;

    private _accessToken: string;

    private _webSocket: WebSocket;

    private _isConnected: boolean;

    private _connectionTask: NodeJS.Timeout | null;

    private _closeRequested: boolean;

    constructor(address: string, user: User, accessToken: string) {
        if (!address) throw new Error('Invalid address.');
        if (!user) throw new Error('Invalid user.');
        if (!accessToken) throw new Error('Invalid access token.');
        this._address = address;
        this._user = user;
        this._accessToken = accessToken;
        this._connectionTask = setInterval(
            () => this.tryConnect(),
            this.ConnectionRetryInterval,
        );
        this.receivedMessages = new Subject<ServerMessage>();
        this.connectionState = new BehaviorSubject<WebSocketConnectionState>(
            WebSocketConnectionState.Disconnected,
        );
        this.tryConnect();
    }

    public readonly receivedMessages: Subject<ServerMessage>;

    public readonly connectionState: BehaviorSubject<WebSocketConnectionState>;

    public changeAccessToken(accessToken: string): void {
        this._accessToken = accessToken;
    }

    public sendMessage(message: ClientMessage): void {
        if (!message || !this._isConnected) return;
        const json = messageParser.serializeClientMessage(message);
        if (json) this._webSocket.send(json);
    }

    public close(): void {
        this._closeRequested = true;
        if (this._webSocket) this._webSocket.close();
    }

    private tryConnect(): void {
        if (this._isConnected || this._closeRequested) return;

        const ws = new WebSocket(this._address);
        this._webSocket = ws;
        ws.onclose = () => {
            this._isConnected = false;
            this._webSocket = null;
            if (!this._closeRequested) {
                this._connectionTask = setInterval(
                    () => this.tryConnect(),
                    this.ConnectionRetryInterval,
                );
                this.connectionState.next(WebSocketConnectionState.Disconnected);
            } else {
                this.connectionState.next(WebSocketConnectionState.Closed);
            }
        };
        ws.onerror = () => {
            if (!this._closeRequested) {
                this.connectionState.next(WebSocketConnectionState.Error);
            }
        };
        ws.onmessage = (frame) => {
            const message: ServerMessage = messageParser
                .deserializeServerMessage(frame.data);
            if (message instanceof ConnectedMessage) {
                this.connectionState.next(WebSocketConnectionState.Connected);
                return;
            }
            if (message) this.receivedMessages.next(message);
        };
        ws.onopen = () => {
            if (this._closeRequested) {
                ws.close();
                return;
            }
            this._isConnected = true;
            clearInterval(this._connectionTask);
            const message = messageParser.serializeClientMessage(new ConnectionRequest(
                this._user.id,
                this._accessToken,
            ));
            if (message) this._webSocket.send(message);
        };
    }
}
