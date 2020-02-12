import Lockr from 'lockr';
import { BehaviorSubject } from 'rxjs';
import Session from './session';
import AuthDetails from '../models/authDetails';
import WebSocketSession from '../server/webSocketSession';
import WSProcessor from './wsProcessor';

interface PersistanceAuth {
    accessToken: string,
    user: {
        id: number,
        username: string,
    },
}

const encode = (value: any): string => btoa(typeof value === 'string' ? value : JSON.stringify(value));

const decode = (value: string): any => (value ? JSON.parse(atob(value)) : null);

export default class SessionManager {
    private readonly LockPrefix = 'nvisibl_';
    private readonly AuthDetailsStorageKey: string = 'session';
    private readonly _webSocketAddress: string;
    private _session: Session;
    private _wsProcessor: WSProcessor;

    constructor(webSocketAddress: string) {
        if (!webSocketAddress) throw new Error('Invalid web socket address.');
        this._webSocketAddress = webSocketAddress;
        Lockr.prefix = this.LockPrefix;
        const auth: PersistanceAuth = this.loadPersistedAuth();
        if (auth) {
            const authDetails = new AuthDetails(auth.accessToken, auth.user);
            this._session = new Session(
                authDetails,
                new WebSocketSession(webSocketAddress, authDetails),
            );
            this._wsProcessor = new WSProcessor({
                webSocketSession: this._session.webSocketSession,
                messages: this._session.messages,
            });
        }
        this.onChange = new BehaviorSubject<Session>(this._session);
    }

    public onChange: BehaviorSubject<Session>;

    public get(): Session {
        return this._session;
    }

    public startSession(auth: AuthDetails): void {
        if (!auth) return;
        this._session = new Session(auth, new WebSocketSession(this._webSocketAddress, auth));
        this._wsProcessor = new WSProcessor({
            webSocketSession: this._session.webSocketSession,
            messages: this._session.messages,
        });
        this.persistAuth({
            accessToken: auth.accessToken,
            user: auth.user,
        });
        this.onChange.next(this._session);
    }

    public clear(): void {
        Lockr.set(this.AuthDetailsStorageKey, null);
        this._wsProcessor.stop();
        this._wsProcessor = null;
        this._session.webSocketSession.close();
        this._session = null;
        this.onChange.next(null);
    }

    private loadPersistedAuth(): PersistanceAuth {
        const auth: PersistanceAuth = decode(Lockr.get(this.AuthDetailsStorageKey, null));
        return (auth && auth.accessToken && auth.user && auth.user.id && auth.user.username)
            ? auth
            : null;
    }

    private persistAuth(details: PersistanceAuth): void {
        Lockr.set(this.AuthDetailsStorageKey, encode(details));
    }
}
