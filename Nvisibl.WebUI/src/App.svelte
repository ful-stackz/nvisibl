<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import session from './stores/session';
    import friends from './stores/friends';
    import chatrooms from './stores/chatrooms';
    import Login from './components/Login.svelte';
    import Register from './components/Register.svelte';
    import UserBar from './components/UserBar.svelte';
    import Chatrooms from './components/Chatrooms.svelte';
    import FriendsList from './components/FriendsList.svelte';
    import Api from './server/api';
    import User from './models/user';

    export let api: Api = null;

    let isLoggedIn: boolean;

    const unsubscribeSession = session.subscribe((state) => {
        if (state.user && state.accessToken) {
            isLoggedIn = true;
        } else {
            friends.clear();
            chatrooms.clear();
            isLoggedIn = false;
        }
    });

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
    });

    onDestroy(() => {
        unsubscribeSession();
    });
</script>

<div class="container mx-auto">
    {#if !isLoggedIn}
        <div class="flex justify-center">
            <div class="m-2 self-center">
                <Login {api} />
            </div>
            <div class="m-2 self-center italic">- Or -</div>
            <div class="m-2 self-center">
                <Register {api} />
            </div>
        </div>
    {:else}
        <div class="mb-2">
            <UserBar />
        </div>
        <div class="mb-2">
            <FriendsList {api} />
        </div>
        <Chatrooms {api} />
    {/if}
</div>
