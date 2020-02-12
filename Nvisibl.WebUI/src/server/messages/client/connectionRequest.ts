import ClientMessage from './clientMessage';

export default class ConnectionRequest implements ClientMessage {
    constructor(userId: number, accessToken: string) {
        this.userId = userId;
        this.accessToken = accessToken;
    }

    public readonly userId: number;
    public readonly accessToken: string;
}
