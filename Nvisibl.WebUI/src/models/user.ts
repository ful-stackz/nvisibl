export default class User {
    constructor(id: number, username: string) {
        this.id = id;
        this.username = username;
    }

    public readonly id: number;

    public readonly username: string;
}
