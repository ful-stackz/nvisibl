import { BehaviorSubject } from 'rxjs';
import Chatroom from '../models/chatroom';
import SendChatMessage from '../server/messages/client/sendChatMessage';
import Session from './session';
import Message from '../models/message';

export default class ChatService {
    private readonly _session: Session;

    constructor(session: Session) {
        if (!session) throw new Error('Invalid session.');
        this._session = session;
        this.onActiveChatroomChange = new BehaviorSubject<Chatroom>(null);
    }

    public readonly onActiveChatroomChange: BehaviorSubject<Chatroom>;

    public getActiveChatroom(): Chatroom {
        return this.onActiveChatroomChange.getValue();
    }

    public setActiveChatroom(next: Chatroom): void {
        this.onActiveChatroomChange.next(next);
    }

    public sendMessage(body: string) : void {
        if (!body) return;
        const { auth, messages } = this._session;
        const chatroom = this.getActiveChatroom();
        const timeSent = new Date();
        this._session.webSocketSession.sendMessage(new SendChatMessage(
            auth.user.id,
            chatroom.id,
            body,
            timeSent.toUTCString(),
        ));
        messages.add(new Message(
            -1,
            auth.user.id,
            chatroom.id,
            body,
            timeSent,
        ));
    }
}
