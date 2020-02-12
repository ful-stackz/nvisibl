export default class Message {
    constructor(
        id: number,
        authorId: number,
        chatroomId: number,
        body: string,
        timeSentUTC: Date,
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
    public readonly timeSentUTC: Date;
}
