<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import Api from '../server/api';
    import User from '../models/user';
    import Message from '../models/message';
    import Chatroom from '../models/chatroom';
    import SessionManager from '../services/sessionManager';

    export let api: Api = null;
    export let sessionManager: SessionManager = null;

    let visibleMessages: Message[] = [];
    let chatroom: Chatroom = null;
    let user: User = null;

    const messagesSub = sessionManager.get().messages.latestMessage.subscribe((message): void => {
        if (!chatroom) return;
        if (message.chatroomId === chatroom.id) {
            visibleMessages = [...visibleMessages, message];
        }
    });

    const chatroomsSub = sessionManager.get().chatService.onActiveChatroomChange.subscribe((next) => {
        if (!next || (chatroom && chatroom.id === next.id)) return;
        chatroom = next;
        loadChatroomMessages();
    });

    function loadChatroomMessages(): void {
        if (chatroom.initialLoadComplete) {
            visibleMessages = sessionManager.get().messages.getChatroomMessages(chatroom);
            return;
        }

        visibleMessages = [];
        const { auth: { accessToken }, chatrooms, messages } = sessionManager.get();
        api.get(`chatrooms/${chatroom.id}/messages`, null, accessToken)
            .then(({ data }) => {
                data.forEach((msg) => {
                    messages.add(new Message(
                        msg.messageId,
                        msg.authorId,
                        msg.chatroomId,
                        msg.body,
                        new Date(msg.timeSentUTC),
                    ));
                });
                chatroom.initialLoadComplete = true;
                chatrooms.update(chatroom);
            })
            .catch(console.error);
    }

    function ownsMessage(message: Message): boolean {
        return message.authorId === user.id;
    }

    onMount((): void => {
        if (!api) throw new Error('Api prop is not provided.');
        if (!sessionManager) throw new Error('SessionManager prop is not provided.');

        const session = sessionManager.get();
        user = session.auth.user;
        
        const subscription = session.chatrooms.onChange.subscribe((chatrooms) => {
            if (chatrooms.length > 0) {
                session.chatService.setActiveChatroom(chatrooms[0]);
                subscription.unsubscribe();
            }
        });
    });

    onDestroy(() => {
        messagesSub.unsubscribe();
        chatroomsSub.unsubscribe();
    });
</script>

<div class="h-full bg-white border-gray-200 border-2 rounded overflow-y-auto">
    {#if chatroom}
        <div class="w-full text-center">
            <span class="inline-block bg-gray-200 p-2 mt-2 rounded">{chatroom.name}</span>
        </div>
        <div class="flex flex-col">
            {#each visibleMessages as message}
                {#if ownsMessage(message)}
                    <div class="flex self-end justify-end w-1/2">
                        <div class="bg-blue-400 text-white p-2 m-2 rounded inline-block">
                            <p class="-opacity-25 -hover:opacity-100">{message.body}</p>
                        </div>
                    </div>
                {:else}
                    <div class="flex self-start w-1/2">
                        <div class="bg-blue-500 text-white p-2 m-2 rounded inline-block">
                            <p class="-opacity-25 -hover:opacity-100">{message.body}</p>
                        </div>
                    </div>
                {/if}
            {/each}
        </div>
    {:else}
        <div class="w-full text-center">
            <span class="inline-block bg-gray-200 p-2 mt-2 rounded">Loading...</span>
        </div>
    {/if}
</div>
