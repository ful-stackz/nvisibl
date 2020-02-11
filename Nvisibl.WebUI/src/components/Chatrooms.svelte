<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import Api from '../server/api';
    import Chatroom from '../models/chatroom';
    import User from '../models/user';
    import SessionManager from '../services/sessionManager';

    export let api: Api = null;
    export let sessionManager: SessionManager = null;

    let visibleChatrooms: Chatroom[] = [];
    let isLoading: boolean = true;
    let inError: boolean = false;

    const chatroomsSub = sessionManager.get().chatrooms.onChange
        .subscribe((current) => visibleChatrooms = current);

    function handleChatroomClick(chatroom: Chatroom): void {
        const chatService = sessionManager.get().chatService;
        const activeChatroom = chatService.getActiveChatroom();
        if (activeChatroom && activeChatroom.id === chatroom.id) return;
        chatService.setActiveChatroom(chatroom);
    }

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        if (!sessionManager) throw new Error('SessionManager prop is not provided.');

        const session = sessionManager.get();
        const { user, accessToken } = session.auth;
        api.get(`users/${user.id}/chatrooms`, null, accessToken)
            .then(({ data }) => {
                if (!data.chatrooms) {
                    isLoading = false;
                    return;
                }
                data.chatrooms.forEach((chatroom) => {
                    if (!chatroom.isShared && !chatroom.name) {
                        chatroom.name = chatroom.users
                            .find(({ id }) => id !== user.id)
                            .username;
                    }
                    session.chatrooms.add(new Chatroom(
                        chatroom.id,
                        chatroom.name,
                        chatroom.isShared,
                        chatroom.users.map(({ id, username }) => new User(id, username)),
                    ));
                });
                isLoading = false;
            })
            .catch((_) => {
                isLoading = false;
                inError = true;
            });
    });

    onDestroy(() => {
        chatroomsSub.unsubscribe();
    });
</script>

<div class="bg-gray-200 p-3 rounded">
    <div class="text-lg">Chatrooms</div>
    {#if isLoading}
        <div>Loading...</div>
    {:else if visibleChatrooms.length > 0}
        {#each visibleChatrooms as chatroom}
            <div
                class="flex flex-row p-3 cursor-pointer rounded hover:bg-gray-300"
                on:click={handleChatroomClick(chatroom)}>
                <div class="flex-none bg-gray-400 rounded-full w-6 h-6"></div>
                <div class="flex-shrink truncate ml-2">{chatroom.name}</div>
            </div>
        {/each}
    {:else if inError}
        <div class="text-red-800">Could not load chatrooms</div>
    {:else}
        <div>No chatrooms</div>
    {/if}
</div>
