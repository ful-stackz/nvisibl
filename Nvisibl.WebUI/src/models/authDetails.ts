import { BehaviorSubject } from 'rxjs';
import User from './user';

export default class AuthDetails {
    constructor(accessToken: string, user: User) {
        if (!accessToken) throw new Error('Invalid access token.');
        if (!user) throw new Error('Invalid user.');
        this.accessToken = accessToken;
        this.user = user;
        this.onChange = new BehaviorSubject<AuthDetails>(this);
    }

    public readonly user: User;
    public readonly onChange: BehaviorSubject<AuthDetails>;
    public accessToken: string;

    public changeAccessToken(accessToken: string): void {
        this.accessToken = accessToken;
        this.onChange.next(this);
    }
}
