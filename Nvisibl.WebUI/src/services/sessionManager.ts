import Lockr from 'lockr';
import { BehaviorSubject } from 'rxjs';
import Session from './session';
import AuthDetails from '../models/authDetails';
import User from '../models/user';
import Api from '../server/api';
import WebSocketSession from '../server/webSocketSession';
import WSProcessor from './wsProcessor';

interface PersistanceAuth {
    tokenDetails: {
        token: string,
        createdAt: string,
        validBefore: string,
    },
    user: {
        id: number,
        username: string,
    },
}

const encode = (value: any): string => btoa(escape(typeof value === 'string' ? value : JSON.stringify(value)));

const decode = (value: string): any => (value ? JSON.parse(unescape(atob(value))) : null);

export default class SessionManager {
    private readonly LockPrefix = 'nvisibl_';
    private readonly AuthDetailsStorageKey: string = 'session';
    private readonly TimeBeforeTokenRenewalInMs: number = 60 * 1000;
    private readonly CheckTokenRenewalTimeInMs: number = 20 * 1000;
    private readonly _api: Api;
    private readonly _webSocketAddress: string;
    private _session: Session;
    private _wsProcessor: WSProcessor;
    private _tokenRenewalTask: NodeJS.Timeout;

    constructor(api: Api, webSocketAddress: string) {
        if (!api) throw new Error('Invalid api.');
        if (!webSocketAddress) throw new Error('Invalid web socket address.');
        this._api = api;
        this._webSocketAddress = webSocketAddress;
        Lockr.prefix = this.LockPrefix;
        const auth: AuthDetails = this.loadPersistedAuth();
        if (auth) this.startSessionInternal(auth);
        else Lockr.set(this.AuthDetailsStorageKey, null);
        this.onChange = new BehaviorSubject<Session>(this._session);
    }

    public onChange: BehaviorSubject<Session>;

    public get(): Session {
        return this._session;
    }

    public startSession(auth: AuthDetails): void {
        if (!auth) return;
        this.startSessionInternal(auth);
        this.onChange.next(this._session);
    }

    public clear(): void {
        Lockr.set(this.AuthDetailsStorageKey, null);
        this.stopPeriodicTokenRenewal();
        this._wsProcessor.stop();
        this._wsProcessor = null;
        this._session.webSocketSession.close();
        this._session = null;
        this.onChange.next(null);
    }

    private loadPersistedAuth(): AuthDetails {
        const stored: PersistanceAuth = decode(Lockr.get(this.AuthDetailsStorageKey, null));
        if (!(
            stored
            && stored.tokenDetails
            && stored.tokenDetails.createdAt
            && stored.tokenDetails.token
            && stored.tokenDetails.validBefore
            && stored.user
            && stored.user.id
            && stored.user.username)) return null;

        const auth = new AuthDetails(
            new User(stored.user.id, stored.user.username),
            {
                token: stored.tokenDetails.token,
                createdAt: new Date(stored.tokenDetails.createdAt),
                validBefore: new Date(stored.tokenDetails.validBefore),
            },
        );
        return auth.authToken.validBefore > new Date() ? auth : null;
    }

    private persistAuth(auth: AuthDetails): void {
        const persistDetails: PersistanceAuth = {
            tokenDetails: {
                createdAt: auth.authToken.createdAt.toString(),
                token: auth.authToken.token,
                validBefore: auth.authToken.validBefore.toString(),
            },
            user: {
                id: auth.user.id,
                username: auth.user.username,
            },
        };
        Lockr.set(this.AuthDetailsStorageKey, encode(persistDetails));
    }

    private startSessionInternal(auth: AuthDetails): void {
        this._session = new Session(auth, new WebSocketSession(this._webSocketAddress, auth));
        this._wsProcessor = new WSProcessor({
            api: this._api,
            session: this._session,
        });
        this.persistAuth(auth);
        this.startPeriodicTokenRenewal();
    }

    private startPeriodicTokenRenewal(): void {
        this._tokenRenewalTask = setInterval(async () => {
            const expirationMs = this._session.auth.authToken.validBefore.getTime();
            const nowMs = Date.now();
            if (nowMs + this.TimeBeforeTokenRenewalInMs <= expirationMs) return;

            const { authToken: { token }, user } = this._session.auth;
            try {
                const { data } = await this._api.post('auth/renew-token', { username: user.username }, token);
                const auth = new AuthDetails(user, {
                    createdAt: new Date(data.createdAt),
                    token: data.accessToken,
                    validBefore: new Date(data.validBefore),
                });
                this._session.auth = auth;
                this._session.webSocketSession.changeAccessToken(data.accessToken);
                this.persistAuth(auth);
            } catch (error) {
                // eslint-disable-next-line no-console
                console.error(error);
                this.clear();
            }
        }, this.CheckTokenRenewalTimeInMs);
    }

    private stopPeriodicTokenRenewal(): void {
        clearInterval(this._tokenRenewalTask);
        this._tokenRenewalTask = null;
    }
}
