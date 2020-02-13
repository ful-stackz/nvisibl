import { Subscription } from 'rxjs';
import WebSocketSession from '../server/webSocketSession';
import FriendsStore from '../stores/friendsStore';
import MessagesStore from '../stores/messagesStore';
import ReceiveChatMessage from '../server/messages/server/receiveChatMessage';
import FriendRequestMessage from '../server/messages/server/friendRequest';
import AuthDetails from '../models/authDetails';
import Message from '../models/message';
import FriendRequest from '../models/friendRequest';
import User from '../models/user';

export default class WSProcessor {
    private readonly _subscription: Subscription;

    constructor(config: {
        auth: AuthDetails,
        webSocketSession: WebSocketSession,
        messages: MessagesStore,
        friends: FriendsStore,
    }) {
        this._subscription = config.webSocketSession.receivedMessages.subscribe((message) => {
            if (message instanceof ReceiveChatMessage) {
                config.messages.add(new Message(
                    message.id,
                    message.authorId,
                    message.chatroomId,
                    message.body,
                    new Date(message.timeSentUTC),
                ));
            } else if (message instanceof FriendRequestMessage) {
                const { sender, receiver } = message;
                config.friends.onFriendRequest.next(new FriendRequest({
                    id: message.id,
                    accepted: message.accepted,
                    sender: new User(sender.id, sender.username),
                    receiver: new User(receiver.id, receiver.username),
                }));

                if (!message.accepted) return;
                const friend = sender.id === config.auth.user.id ? receiver : sender;
                config.friends.add(new User(friend.id, friend.username));
            }
        });
    }

    public stop(): void {
        this._subscription.unsubscribe();
    }
}
