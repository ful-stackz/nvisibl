import User from './user';

export interface AuthToken {
    token: string,
    createdAt: Date,
    validBefore: Date,
}

export default class AuthDetails {
    constructor(user: User, token: AuthToken) {
        if (!user) throw new Error('Invalid user.');
        if (!token) throw new Error('Invalid token.');
        this.user = user;
        this.authToken = token;
    }

    public readonly user: User;
    public readonly authToken: AuthToken;
}
