import { BehaviorSubject } from 'rxjs';
import Chatroom from '../models/chatroom';
import Session from './session';

export default class ChatService {
    private readonly _session: Session;

    constructor(session: Session) {
        if (!session) throw new Error('Invalid session.');
        this._session = session;
        this.onActiveChatroomChange = new BehaviorSubject<Chatroom>(null);
    }

    public readonly onActiveChatroomChange: BehaviorSubject<Chatroom>;

    public getActiveChatroom(): Chatroom {
        return this.onActiveChatroomChange.getValue();
    }

    public setActiveChatroom(next: Chatroom): void {
        this.onActiveChatroomChange.next(next);
    }

    public sendMessage(body: string) : Promise<any> {
        return new Promise((resolve, reject) => {
            if (!body) {
                reject(new Error('Invalid body.'));
                return;
            }
            const { api, auth } = this._session;
            const chatroom = this.getActiveChatroom();
            const timeSent = new Date();
            api.post(
                'messages',
                {
                    authorId: auth.user.id,
                    chatroomId: chatroom.id,
                    body,
                    timeSentUtc: timeSent.toISOString(),
                },
                auth.authToken.token,
            )
                .then(resolve)
                .catch(reject);
        });
    }
}
