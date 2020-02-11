<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import Login from './components/Login.svelte';
    import Register from './components/Register.svelte';
    import UserBar from './components/UserBar.svelte';
    import Chatrooms from './components/Chatrooms.svelte';
    import FriendsList from './components/FriendsList.svelte';
    import MessagePanel from './components/MessagePanel.svelte';
    import Api from './server/api';
    import SessionManager from './services/sessionManager';

    export let api: Api = null;
    export let sessionManager: SessionManager;

    let isLoggedIn: boolean;

    const sessionSub = sessionManager.onChange.subscribe((session) => isLoggedIn = !!session);

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        if (!sessionManager) throw new Error('SessionManager prop is not provided.');
    });

    onDestroy(() => {
        sessionSub.unsubscribe();
    });
</script>

<div class="container mx-auto h-screen">
    {#if !isLoggedIn}
        <div class="flex justify-center">
            <div class="m-2 self-center">
                <Login {api} {sessionManager} />
            </div>
            <div class="m-2 self-center italic">- Or -</div>
            <div class="m-2 self-center">
                <Register {api} />
            </div>
        </div>
    {:else}
        <div class="mb-2">
            <UserBar {sessionManager} />
        </div>
        <div class="flex flex-row" style="height: calc(100% - 56px);">
            <div class="w-1/4">
                <div class="mb-2">
                    <FriendsList {api} {sessionManager} />
                </div>
                <Chatrooms {api} {sessionManager} />
            </div>
            <div class="ml-2 w-3/4">
                <div class="" style="height: 75%;">
                    <MessagePanel {sessionManager} />
                </div>
            </div>
        </div>
    {/if}
</div>
