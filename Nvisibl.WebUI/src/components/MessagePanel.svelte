<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import User from '../models/user';
    import Message from '../models/message';
    import Chatroom from '../models/chatroom';
    import SessionManager from '../services/sessionManager';

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
        visibleMessages = sessionManager.get().messages.getChatroomMessages(next);
    });

    function ownsMessage(message: Message): boolean {
        return message.authorId === user.id;
    }

    onMount((): void => {
        if (!sessionManager) throw new Error('SessionManager prop is not provided.');

        const session = sessionManager.get();
        user = session.auth.user;
        chatroom = session.chatrooms.getAll()[0];
        if (!chatroom) {
            const subscription = session.chatrooms.onChange.subscribe((chatrooms) => {
                if (chatrooms.length > 0) {
                    chatroom = chatrooms[0];
                    session.chatService.setActiveChatroom(chatroom);
                    subscription.unsubscribe();
                }
            });
        }
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
