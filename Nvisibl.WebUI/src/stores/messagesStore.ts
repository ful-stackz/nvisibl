import { BehaviorSubject, Subject } from 'rxjs';
import Message from '../models/message';
import Chatroom from '../models/chatroom';

export default class MessagesStore {
    private _messages: Message[];

    constructor() {
        this._messages = [];
        this.onChange = new BehaviorSubject<Message[]>([]);
        this.latestMessage = new Subject<Message>();
    }

    public onChange: BehaviorSubject<Message[]>;
    public latestMessage: Subject<Message>;

    public add(message: Message): void {
        if (!message) return;
        this._messages = [...this._messages, message];
        this.onChange.next(this._messages);
        this.latestMessage.next(message);
    }

    public getChatroomMessages(chatroom: Chatroom): Message[] {
        if (!chatroom) return null;
        return this._messages.filter((message) => message.chatroomId === chatroom.id);
    }
}
