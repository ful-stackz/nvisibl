import User from './user';

export default class Chatroom {
    constructor(id: number, name: string, isShared: boolean, users: User[]) {
        this.id = id;
        this.name = name;
        this.isShared = isShared;
        this.users = users;
    }

    public readonly id: number;

    public readonly name: string;

    public readonly isShared: boolean;

    public readonly users: User[];
}
