<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import Api from '../server/api';
    import User from '../models/user';
    import Chatroom from '../models/chatroom';
    import SessionManager from '../services/sessionManager';

    export let api: Api = null;
    export let sessionManager: SessionManager = null;

    let visibleFriends: User[] = [];
    let isLoading: boolean = true;
    let inError: boolean = false;

    const friendsSub = sessionManager.get().friends.onChange
        .subscribe((current) => visibleFriends = current);

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        if (!sessionManager) throw new Error('SessionManager prop is not provided.');
        const session = sessionManager.get();
        const { user, authToken: { token } } = session.auth;
        api.get(`users/${user.id}/friends`, null, token)
            .then(({ data }) => {
                if (!data) {
                    isLoading = false;
                    return;
                }
                data.forEach(({ id, username }) => {
                    session.friends.add(new User(id, username));
                });
                isLoading = false;
            })
            .catch((_) => {
                isLoading = false;
                inError = true;
            });
    });

    onDestroy(() => {
        friendsSub.unsubscribe();
    });

    function handleFriendClick(friend: User): void {
        const { chatrooms, chatService } = sessionManager.get();
        const chatroom = chatrooms.getAll().find((chatroom) => {
            return chatroom.isShared
                ? false
                : chatroom.users.find((user) => user.id === friend.id);
        });
        if (chatroom) {
            chatService.setActiveChatroom(chatroom);
        } else {
            createPrivateChatroom(friend)
                .then((cr: Chatroom) => {
                    chatrooms.add(cr);
                    chatService.setActiveChatroom(cr);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }

    async function createPrivateChatroom(friend: User): Promise<Chatroom> {
        const { user, authToken: { token } } = sessionManager.get().auth;
        const { data } = await api.post(
            'chatrooms',
            {
                ownerId: user.id,
                isShared: false,
            },
            token,
        );
        const chatroom = new Chatroom(data.id, friend.username, data.isShared, [user, friend]);
        await api.post(
            `chatrooms/${chatroom.id}/users`,
            {
                userId: friend.id,
            },
            token,
        );
        return chatroom;
    }
</script>

<div class="bg-gray-200 p-3 rounded">
    <div class="text-lg">Friends</div>
    {#if isLoading}
        <div>Loading...</div>
    {:else if visibleFriends.length > 0}
        {#each visibleFriends as friend}
            <div
                class="flex flex-row p-3 cursor-pointer rounded hover:bg-gray-300"
                on:click={handleFriendClick(friend)}>
                <div class="flex-none bg-gray-400 rounded-full w-6 h-6"></div>
                <div class="flex-shrink truncate ml-2">{friend.username}</div>
            </div>
        {/each}
    {:else if inError}
        <div class="text-red-800">Could not load friends</div>
    {:else}
        <div>No friends</div>
    {/if}
</div>
