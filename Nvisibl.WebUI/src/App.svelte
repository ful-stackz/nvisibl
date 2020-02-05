<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import session from './stores/session';
    import Login from './components/Login.svelte';
    import Api from './server/api';
    import User from './models/user';

    export let api: Api = null;

    let isLoggedIn: boolean;

    const unsubscribeSession = session.subscribe((state) => {
        console.log(state);
        if (state.user && state.accessToken) {
            isLoggedIn = true;
        } else {
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
        <Login {api} />
    {/if}
</div>