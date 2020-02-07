<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import session from '../stores/session';
    import friends from '../stores/friends';
    import Api from '../server/api';
    import User from '../models/user';

    export let api: Api = null;

    let visibleFriends: User[] = [];
    let isLoading: boolean = true;
    let inError: boolean = false;

    const unsubscribeFriends = friends.subscribe((current) => visibleFriends = current);

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        const { user, accessToken } = session.get();
        api.get(`users/${user.id}/friends`, null, accessToken)
            .then(({ data }) => {
                if (!data) {
                    isLoading = false;
                    return;
                }
                data.forEach(({ id, username }) => {
                    friends.add(new User(id, username));
                });
                isLoading = false;
            })
            .catch((_) => {
                isLoading = false;
                inError = true;
            });
    });

    onDestroy(() => {
        unsubscribeFriends();
    });
</script>

<div class="bg-gray-200 p-3 rounded w-1/4">
    <div class="text-lg">Friends</div>
    {#if isLoading}
        <div>Loading...</div>
    {:else if visibleFriends.length > 0}
        {#each visibleFriends as friend}
            <div class="flex flex-row p-3 cursor-pointer rounded hover:bg-gray-300">
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
