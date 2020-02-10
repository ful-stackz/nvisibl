import { writable, Writable } from 'svelte/store';
import Message from '../models/message';
import Chatroom from '../models/chatroom';

const messages: Message[] = [];
const latestMessage: Writable<Message> = writable(new Message(-1, -1, -1, '', null));

export default {
    all: (): Message[] => messages,
    getChatroomMessages: (chatroom: Chatroom): Message[] => messages
        .filter((message) => message.chatroomId === chatroom.id),
    add: (message: Message): void => {
        console.log('Adding message to store', message);
        messages.push(message);
        latestMessage.set(message);
        console.log('Pushed message from store');
    },
    clear: (): void => {
        messages.length = 0;
    },
    subscribe: latestMessage.subscribe,
};
