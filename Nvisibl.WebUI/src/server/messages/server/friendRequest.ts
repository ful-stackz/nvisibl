import ServerMessage from './serverMessage';

export interface FriendRequestUser {
    id: number,
    username: string,
}

export default class FriendRequest implements ServerMessage {
    constructor(data: {
        id: number,
        accepted: boolean,
        sender: FriendRequestUser,
        receiver: FriendRequestUser,
    }) {
        this.id = data.id;
        this.accepted = data.accepted;
        this.sender = data.sender;
        this.receiver = data.receiver;
    }

    public readonly id: number;
    public readonly accepted: boolean;
    public readonly sender: FriendRequestUser;
    public readonly receiver: FriendRequestUser;
}
