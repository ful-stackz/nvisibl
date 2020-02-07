<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import session from '../stores/session';
    import chatrooms from '../stores/chatrooms';
    import Api from '../server/api';
    import Chatroom from '../models/chatroom';
    import User from '../models/user';

    export let api: Api = null;

    let visibleChatrooms: Chatroom[] = [];
    let isLoading: boolean = true;
    let inError: boolean = false;

    const unsubscribeChatrooms = chatrooms.subscribe((current) => visibleChatrooms = current);

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        const { user, accessToken } = session.get();
        api.get(`users/${user.id}/chatrooms`, null, accessToken)
            .then(({ data }) => {
                if (!data.chatrooms) {
                    isLoading = false;
                    return;
                }
                data.chatrooms.forEach((chatroom) => {
                    chatrooms.add(new Chatroom(
                        chatroom.id,
                        chatroom.name,
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
        unsubscribeChatrooms();
    });
</script>

<div class="bg-gray-200 p-3 rounded w-1/4">
    <div class="text-lg">Chatrooms</div>
    {#if isLoading}
        <div>Loading...</div>
    {:else if visibleChatrooms.length > 0}
        {#each visibleChatrooms as chatroom}
            <div class="flex flex-row p-3 cursor-pointer rounded hover:bg-gray-300">
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
