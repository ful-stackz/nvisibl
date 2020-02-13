import User from './user';

export default class FriendRequest {
    constructor(data: {
        id: number,
        accepted: boolean,
        sender: User,
        receiver: User,
    }) {
        this.id = data.id;
        this.accepted = data.accepted;
        this.sender = data.sender;
        this.receiver = data.receiver;
    }

    public readonly id: number;
    public readonly accepted: boolean;
    public readonly sender: User;
    public readonly receiver: User;
}
