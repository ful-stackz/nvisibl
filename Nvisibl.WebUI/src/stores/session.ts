import Lockr from 'lockr';
import { writable } from 'svelte/store';
import User from '../models/user';

export interface AuthSession {
    user: User,
    accessToken: string
}

Lockr.prefix = 'nvisibl_';
const storedSession: AuthSession = Lockr.get('session', {});

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
        session.set(activeSession);
        Lockr.set('session', activeSession);
    },
    subscribe: session.subscribe,
};
