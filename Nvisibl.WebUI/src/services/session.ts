import AuthDetails from '../models/authDetails';
import ChatroomsStore from '../stores/chatroomsStore';
import FriendsStore from '../stores/friendsStore';
import MessagesStore from '../stores/messagesStore';
import WebSocketSession from '../server/webSocketSession';

export default class Session {
    constructor(auth: AuthDetails, webSocketSession: WebSocketSession) {
        if (!auth) throw new Error('Invalid auth details.');
        if (!webSocketSession) throw new Error('Invalid WebSocket session.');
        this.auth = auth;
        this.webSocketSession = webSocketSession;
        this.chatrooms = new ChatroomsStore();
        this.friends = new FriendsStore();
        this.messages = new MessagesStore();
    }

    public readonly auth: AuthDetails;
    public readonly webSocketSession: WebSocketSession;
    public readonly chatrooms: ChatroomsStore;
    public readonly friends: FriendsStore;
    public readonly messages: MessagesStore;
}
