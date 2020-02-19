import ServerMessage from './serverMessage';

export default class ChatroomInvitation implements ServerMessage {
    constructor(data: {
        chatroomId: number,
        chatroomName: string,
        users: number[],
    }) {
        this.chatroomId = data.chatroomId;
        this.chatroomName = data.chatroomName;
        this.users = data.users;
    }

    public readonly chatroomId: number;
    public readonly chatroomName: string;
    public readonly users: number[];
}
