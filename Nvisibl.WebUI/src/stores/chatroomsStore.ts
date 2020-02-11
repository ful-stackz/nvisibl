import { BehaviorSubject } from 'rxjs';
import Chatroom from '../models/chatroom';

export default class ChatroomsStore {
    private _chatrooms: Chatroom[];

    constructor() {
        this._chatrooms = [];
        this.onChange = new BehaviorSubject<Chatroom[]>([]);
    }

    public onChange: BehaviorSubject<Chatroom[]>;

    public add(chatroom: Chatroom): void {
        this._chatrooms = [...this._chatrooms, chatroom];
        this.onChange.next(this._chatrooms);
    }

    public getAll(): Chatroom[] {
        return this._chatrooms;
    }

    public update(chatroom: Chatroom): void {
        this._chatrooms = this._chatrooms.map((c) => (c.id === chatroom.id ? chatroom : c));
        this.onChange.next(this._chatrooms);
    }
}
