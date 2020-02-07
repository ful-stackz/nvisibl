import { writable, Writable, get } from 'svelte/store';
import User from '../models/user';

const friends: Writable<User[]> = writable([]);

export default {
    get: (): User[] => get(friends),
    add: (friend: User): void => {
        friends.update((current) => [...current, friend]);
    },
    remove: (friend: User): void => {
        friends.update((current) => current.filter((user) => user.id !== friend.id));
    },
    clear: (): void => friends.set([]),
    subscribe: friends.subscribe,
};
