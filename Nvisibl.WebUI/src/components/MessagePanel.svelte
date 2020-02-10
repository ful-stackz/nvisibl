<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import session from '../stores/session';
    import messages from '../stores/messages';
    import chatrooms from '../stores/chatrooms';
    import chatService from '../services/chatService';
    import User from '../models/user';
    import Message from '../models/message';
    import Chatroom from '../models/chatroom';

    let visibleMessages: Message[] = [];
    let chatroom: Chatroom = null;
    let user: User = null;

    const unsubscribeMessages = messages.subscribe((message: Message): void => {
        console.log('Received message in message panel', message);
        if (!chatroom) return;
        console.log(chatroom);
        console.log('Received message', message);
        if (message.chatroomId === chatroom.id) {
            visibleMessages = [...visibleMessages, message];
        }
    });

    const unsubscribeChatroom = chatService.activeChatroom.subscribe((next: Chatroom) => {
        if (!next) return;
        chatroom = next;
        visibleMessages = messages.getChatroomMessages(next);
    });

    function ownsMessage(message: Message): boolean {
        return message.authorId === user.id;
    }

    onMount((): void => {
        user = session.get().user;
        chatroom = chatrooms.get()[0];
        if (!chatroom) {
            const unsubscribe = chatrooms.subscribe((chatrooms) => {
                if (chatrooms.length > 0) {
                    chatroom = chatrooms[0];
                    unsubscribe();
                }
            });
        }
    });

    onDestroy(() => {
        unsubscribeMessages();
        unsubscribeChatroom.unsubscribe();
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
                            <p class="opacity-25">{message.body}</p>
                        </div>
                    </div>
                {:else}
                    <div class="flex self-start w-1/2">
                        <div class="bg-blue-500 text-white p-2 m-2 rounded inline-block">
                            <p class="opacity-25">{message.body}</p>
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
