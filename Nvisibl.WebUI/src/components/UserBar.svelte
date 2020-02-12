<script lang="ts">
    import { onMount } from 'svelte';
    import User from '../models/user';
    import SessionManager from '../services/sessionManager';

    export let sessionManager: SessionManager = null;

    let user: User = sessionManager.get().auth.user;

    function logout(): void {
        sessionManager.clear();
    }

    onMount(() => {
        if (!sessionManager) throw new Error('SessionManager prop is not provided.');
    });
</script>

<div class="bg-gray-200 p-3 rounded rounded-t-none">
    <span>Welcome, {user.username}!</span>
    <a
        href="/#"
        class="float-right"
        on:click|preventDefault={logout}>
        Logout
    </a>
</div>
