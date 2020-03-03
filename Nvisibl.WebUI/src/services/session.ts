import AuthDetails from '../models/authDetails';
import ChatroomsStore from '../stores/chatroomsStore';
import FriendsStore from '../stores/friendsStore';
import MessagesStore from '../stores/messagesStore';
import Api from '../server/api';
import WebSocketSession from '../server/webSocketSession';
import ChatService from './chatService';

export default class Session {
    constructor(api: Api, auth: AuthDetails, webSocketSession: WebSocketSession) {
        if (!api) throw new Error('Invalid API.');
        if (!auth) throw new Error('Invalid auth details.');
        if (!webSocketSession) throw new Error('Invalid WebSocket session.');
        this.api = api;
        this.auth = auth;
        this.webSocketSession = webSocketSession;
        this.chatrooms = new ChatroomsStore();
        this.friends = new FriendsStore();
        this.messages = new MessagesStore();
        this.chatService = new ChatService(this);
    }

    public auth: AuthDetails;
    public readonly api: Api;
    public readonly webSocketSession: WebSocketSession;
    public readonly chatrooms: ChatroomsStore;
    public readonly friends: FriendsStore;
    public readonly messages: MessagesStore;
    public readonly chatService: ChatService;
}
