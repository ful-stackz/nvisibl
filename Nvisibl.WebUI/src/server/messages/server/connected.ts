import ServerMessage from './serverMessage';

export default class Connected implements ServerMessage {
    constructor(session: string) {
        this.session = session;
    }

    public readonly session: string;
}
