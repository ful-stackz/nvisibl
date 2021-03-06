import ClientMessage from './client/clientMessage';
import ConnectionRequest from './client/connectionRequest';
import ServerMessage from './server/serverMessage';
import serverMessages from './server';

const ClientMessageTypes = {
    ConnectionRequest: 'CONNECTION_REQUEST',
};
Object.freeze(ClientMessageTypes);

const ServerMessageTypes = {
    Connected: 'CONNECTED',
    ChatMessage: 'CHAT_MESSAGE_RECEIVE',
    ChatroomInvitation: 'CHATROOM_INVITATION',
    FriendRequest: 'FRIEND_REQUEST',
};
Object.freeze(ServerMessageTypes);

const deserializeServerMessage = (message: string): ServerMessage => {
    if (!message) return new serverMessages.Empty();

    const json: { type: string, payload: any } = JSON.parse(message);
    if (!json.type || !json.payload) return new serverMessages.Empty();

    switch (json.type) {
        case ServerMessageTypes.Connected:
            return new serverMessages.Connected(json.payload.sessionId);

        case ServerMessageTypes.ChatMessage:
            return new serverMessages.ReceiveChatMessage(
                json.payload.messageId,
                json.payload.authorId,
                json.payload.chatroomId,
                json.payload.body,
                json.payload.timeSentUtc,
            );

        case ServerMessageTypes.FriendRequest: {
            const {
                id,
                accepted,
                senderId,
                senderUsername,
                receiverId,
                receiverUsername,
            } = json.payload;
            return new serverMessages.FriendRequest({
                id,
                accepted,
                sender: { id: senderId, username: senderUsername },
                receiver: { id: receiverId, username: receiverUsername },
            });
        }

        case ServerMessageTypes.ChatroomInvitation:
            return new serverMessages.ChatroomInvitation({
                chatroomId: json.payload.chatroomId,
                chatroomName: json.payload.chatroomName,
                users: json.payload.users,
            });

        default:
            return new serverMessages.Empty();
    }
};

const serializeClientMessage = (message: ClientMessage): string => {
    if (!message) return '';

    let type = '';
    let payload = { ...message };

    if (message instanceof ConnectionRequest) {
        type = ClientMessageTypes.ConnectionRequest;
    } else {
        type = '';
        payload = null;
    }

    return (type && payload)
        ? JSON.stringify({ type, payload })
        : '';
};

export default {
    deserializeServerMessage,
    serializeClientMessage,
};
