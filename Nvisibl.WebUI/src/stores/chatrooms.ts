import { writable, Writable, get } from 'svelte/store';
import Chatroom from '../models/chatroom';

const chatrooms: Writable<Chatroom[]> = writable([]);

export default {
    get: (): Chatroom[] => get(chatrooms),
    add: (chatroom: Chatroom): void => {
        chatrooms.update((current) => [...current, chatroom]);
    },
    remove: (chatroom: Chatroom): void => {
        chatrooms.update((current) => current.filter(({ id }) => id !== chatroom.id));
    },
    update: (chatroom: Chatroom): void => {
        chatrooms.update((current) => current.map((c) => (c.id === chatroom.id ? chatroom : c)));
    },
    clear: (): void => chatrooms.set([]),
    subscribe: chatrooms.subscribe,
};
