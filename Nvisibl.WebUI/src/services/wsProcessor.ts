import { Subscription } from 'rxjs';
import WebSocketSession from '../server/webSocketSession';
import MessagesStore from '../stores/messagesStore';
import ReceiveChatMessage from '../server/messages/server/receiveChatMessage';
import Message from '../models/message';

export default class WSProcessor {
    private readonly _subscription: Subscription;

    constructor(config: {
        webSocketSession: WebSocketSession,
        messages: MessagesStore,
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
            }
        });
    }

    public stop(): void {
        this._subscription.unsubscribe();
    }
}
