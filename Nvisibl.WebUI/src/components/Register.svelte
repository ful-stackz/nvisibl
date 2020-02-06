<script lang="ts">
    import { onMount } from 'svelte'
    import Api from '../server/api';

    export let api: Api = null;

    let username: string = '';
    let password: string = '';
    let confirmPassword: string = '';
    let usernameWarning: string = '';
    let passwordWarning: string = '';
    let confirmPasswordWarning: string = '';
    let errorMessages: string[] = [];

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
    });

    function register(): void {
        usernameWarning = username ? '' : 'Please choose a username.';
        passwordWarning = password ? '' : 'Please choose a strong password.';
        confirmPasswordWarning
            = password === confirmPassword
            ? ''
            : 'Passwords don\'t match.';
        if (!username || !password || !confirmPassword) return;

        api.post('auth/register', { username, password })
            .then((_) => {
                username = '';
                password = '';
                confirmPassword = '';
                errorMessages = [];
            })
            .catch(({ response }) => {
                const data = response.data;
                errorMessages = Array.isArray(data) ? data : [];
            });
    }

    function registerOnEnter(e: KeyboardEvent): void {
        if (e.keyCode === 13) register();
    }
</script>

<div class="w-full max-w-xs">
    <form class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <h1 class="text-lg mb-4">Register</h1>
        <div class="mb-4">
            <label class="block text-gray-700 text-sm font-bold mb-2" for="register-username">
                Username
            </label>
            <input
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                id="register-username"
                type="text"
                placeholder="Username"
                bind:value={username}
                on:keyup={registerOnEnter}>
            <p class="{usernameWarning ? 'block' : 'hidden'} text-red-500 mt-2 text-xs italic">{usernameWarning}</p>
        </div>
        <div class="mb-4">
            <label class="block text-gray-700 text-sm font-bold mb-2" for="register-password">
                Password
            </label>
            <input
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                id="password"
                type="password"
                placeholder="******************"
                bind:value={password}
                on:keyup={registerOnEnter}>
            <p class="{passwordWarning ? 'block' : 'hidden'} text-red-500 mt-2 text-xs italic">{passwordWarning}</p>
        </div>
        <div class="mb-4">
            <label class="block text-gray-700 text-sm font-bold mb-2" for="confirm-password">
                Confirm password
            </label>
            <input
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                id="confirm-password"
                type="password"
                placeholder="******************"
                bind:value={confirmPassword}
                on:keyup={registerOnEnter}>
            <p class="{confirmPasswordWarning ? 'block' : 'hidden'} text-red-500 mt-2 text-xs italic">{confirmPasswordWarning}</p>
        </div>
        {#each errorMessages as errorMessage}
            <div class="mb-6">
                <p class="text-red-500 mt-2 text-xs">{errorMessage}</p>
            </div>
        {/each}
        <div class="flex items-center justify-between">
            <button
                class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                type="button"
                on:click|preventDefault={register}>
                Register
            </button>
        </div>
    </form>
</div>
