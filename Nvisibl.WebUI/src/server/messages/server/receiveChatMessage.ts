import ServerMessage from './serverMessage';

export default class ReceiveChatMessage implements ServerMessage {
    constructor(
        id: number,
        authorId: number,
        chatroomId: number,
        body: string,
        timeSentUTC: string,
    ) {
        this.id = id;
        this.authorId = authorId;
        this.chatroomId = chatroomId;
        this.body = body;
        this.timeSentUTC = timeSentUTC;
    }

    public readonly id: number;
    public readonly authorId: number;
    public readonly chatroomId: number;
    public readonly body: string;
    public readonly timeSentUTC: string;
}
