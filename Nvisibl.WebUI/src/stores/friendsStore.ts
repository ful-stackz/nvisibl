import { BehaviorSubject } from 'rxjs';
import User from '../models/user';

export default class FriendsStore {
    private _friends: User[];

    constructor() {
        this._friends = [];
        this.onChange = new BehaviorSubject<User[]>([]);
    }

    public onChange: BehaviorSubject<User[]>;

    public add(friend: User): void {
        if (!friend) return;
        this._friends = [...this._friends, friend];
        this.onChange.next(this._friends);
    }

    public getAll(): User[] {
        return this._friends;
    }

    public remove(friend: User): void {
        this._friends = this._friends.filter((f) => f.id !== friend.id);
        this.onChange.next(this._friends);
    }
}
