import User from './user';

export default class Chatroom {
    constructor(id: number, name: string, users: User[]) {
        this.id = id;
        this.name = name;
        this.users = users;
    }

    public readonly id: number;

    public readonly name: string;

    public readonly users: User[];
}
