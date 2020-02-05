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
    set: (user?: User, accessToken?: string): void => {
        const activeUser = user instanceof User ? user : activeSession.user;
        const activeToken = accessToken || activeSession.accessToken;
        activeSession = { user: activeUser, accessToken: activeToken };
        session.set(activeSession);
    },
    subscribe: session.subscribe,
};
