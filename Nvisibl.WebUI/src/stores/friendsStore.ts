import { BehaviorSubject, Subject } from 'rxjs';
import User from '../models/user';
import FriendRequest from '../models/friendRequest';

export default class FriendsStore {
    private _friends: User[];

    constructor() {
        this._friends = [];
        this.onChange = new BehaviorSubject<User[]>([]);
        this.onFriendRequest = new Subject<FriendRequest>();
    }

    public readonly onChange: BehaviorSubject<User[]>;
    public readonly onFriendRequest: Subject<FriendRequest>;

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
