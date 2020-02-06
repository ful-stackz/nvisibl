import Lockr from 'lockr';
import { writable } from 'svelte/store';
import User from '../models/user';

export interface AuthSession {
    user: User,
    accessToken: string
}

const sessionStorageKey: string = 'session';

Lockr.prefix = 'nvisibl_';
const storedSession: AuthSession = Lockr.get(sessionStorageKey, { });

let activeSession: AuthSession = (
    storedSession
    && storedSession.user
    && storedSession.user.id
    && storedSession.user.username
    && storedSession.accessToken)
    ? storedSession
    : { user: undefined, accessToken: undefined };

const session = writable(activeSession);

export default {
    get: (): AuthSession => activeSession,
    set: (accessToken: string, user?: User): void => {
        const activeUser = user || activeSession.user;
        activeSession = { user: activeUser, accessToken };
        Lockr.set(sessionStorageKey, activeSession);
        session.set(activeSession);
    },
    subscribe: session.subscribe,
};
