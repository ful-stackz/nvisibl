import { BehaviorSubject } from 'rxjs';
import Chatroom from '../models/chatroom';

const activeChatroom: BehaviorSubject<Chatroom> = new BehaviorSubject(null);

export default {
    setActiveChatroom: (chatroom: Chatroom): void => {
        activeChatroom.next(chatroom);
    },
    activeChatroom,
};
