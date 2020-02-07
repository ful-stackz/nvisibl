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
    import WebSocketSession from './server/webSocketSession';
    import User from './models/user';

    export let api: Api = null;
    export let webSocketAddress: string;

    let isLoggedIn: boolean;
    let webSocketSession: WebSocketSession;

    const unsubscribeSession = session.subscribe((state) => {
        if (state.user && state.accessToken) {
            isLoggedIn = true;
            if (!webSocketSession) {
                webSocketSession = new WebSocketSession(
                    webSocketAddress,
                    state.user,
                    state.accessToken,
                );
            } else {
                webSocketSession.changeAccessToken(state.accessToken);
            }
        } else {
            friends.clear();
            chatrooms.clear();
            if (webSocketSession) {
                webSocketSession.close();
                webSocketSession = null;
            }
            isLoggedIn = false;
        }
    });

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        if (!webSocketAddress) throw new Error('WebSocketAddress prop is not provided.');
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
