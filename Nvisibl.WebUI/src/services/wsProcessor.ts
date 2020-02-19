import { Subscription } from 'rxjs';
import Api from '../server/api';
import ReceiveChatMessage from '../server/messages/server/receiveChatMessage';
import FriendRequestMessage from '../server/messages/server/friendRequest';
import ChatroomInvitationMessage from '../server/messages/server/chatroomInvitation';
import Chatroom from '../models/chatroom';
import Message from '../models/message';
import FriendRequest from '../models/friendRequest';
import User from '../models/user';
import Session from './session';

interface ApiGetChatroom {
    id: number,
    name: string,
    isShared: boolean,
    users: { id: number, username: string }[],
}

export default class WSProcessor {
    private readonly _subscription: Subscription;

    constructor(config: {
        api: Api,
        session: Session,
    }) {
        const { api, session } = config;
        const {
            chatrooms,
            messages,
            friends,
            webSocketSession,
        } = session;

        this._subscription = webSocketSession.receivedMessages.subscribe((message) => {
            if (message instanceof ReceiveChatMessage) {
                messages.add(new Message(
                    message.id,
                    message.authorId,
                    message.chatroomId,
                    message.body,
                    new Date(message.timeSentUTC),
                ));
            } else if (message instanceof FriendRequestMessage) {
                const { sender, receiver } = message;
                friends.onFriendRequest.next(new FriendRequest({
                    id: message.id,
                    accepted: message.accepted,
                    sender: new User(sender.id, sender.username),
                    receiver: new User(receiver.id, receiver.username),
                }));

                if (!message.accepted) return;
                const friend = sender.id === config.session.auth.user.id ? receiver : sender;
                friends.add(new User(friend.id, friend.username));
            } else if (message instanceof ChatroomInvitationMessage) {
                if (message.users.length === 1
                    || chatrooms.getAll().find((chatroom) => chatroom.id === message.chatroomId)) {
                    return;
                }

                const { authToken: { token }, user } = session.auth;
                api.get(`chatrooms/${message.chatroomId}`, { includeUsers: true }, token)
                    .then(({ data }: { data: ApiGetChatroom }) => {
                        const currentFriends = friends.getAll();
                        const chatroomName = data.isShared
                            ? data.name
                            : currentFriends
                                .find((friend) => friend.id !== user.id
                                    && data.users.find((u) => u.id === friend.id))
                                .username;
                        chatrooms.add(new Chatroom(
                            data.id,
                            chatroomName,
                            data.isShared,
                            data.users
                                .filter((u) => u.id === user.id
                                        || currentFriends.find((friend) => friend.id === u.id))
                                .map((u) => new User(u.id, u.username)),
                        ));
                    })
                    .catch(/* TODO: Handle */);
            }
        });
    }

    public stop(): void {
        this._subscription.unsubscribe();
    }
}
