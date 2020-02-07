import ClientMessage from './clientMessage';

export default class SendChatMessage implements ClientMessage {
    constructor(
        authorId: number,
        chatroomId: number,
        body: string,
        timeSentUTC: string,
    ) {
        this.authorId = authorId;
        this.chatroomId = chatroomId;
        this.body = body;
        this.timeSentUTC = timeSentUTC;
    }

    public readonly authorId: number;

    public readonly chatroomId: number;

    public readonly body: string;

    public readonly timeSentUTC: string;
}
