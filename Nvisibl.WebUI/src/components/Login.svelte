<script lang="ts">
    import { onMount } from 'svelte'
    import Api from '../server/api';
    import AuthDetails from '../models/authDetails';
    import User from '../models/user';
    import SessionManager from '../services/sessionManager';

    export let api: Api = null;
    export let sessionManager: SessionManager = null;

    let username: string;
    let password: string;
    let usernameWarning: string;
    let passwordWarning: string;
    let inError: boolean;

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        if (!sessionManager) throw new Error('SessionManager prop is not provided');
    });

    function login(): void {
        usernameWarning = username ? '' : 'Please type in your username.';
        passwordWarning = password ? '' : 'Please type in your password.';
        if (!username || !password) return;

        api.post('auth/login', { username, password })
            .then(({ data }) => {
                inError = false;
                sessionManager.startSession(new AuthDetails(
                    data.accessToken,
                    new User(data.userId, username),
                ));
                username = '';
                password = '';
            })
            .catch((_) => inError = true);
    }

    function loginOnEnter(e: KeyboardEvent): void {
        if (e.keyCode === 13) login();
    }
</script>

<div class="w-full max-w-xs">
    <form class="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
        <h1 class="text-lg mb-4">Login</h1>
        <div class="mb-4">
            <label class="block text-gray-700 text-sm font-bold mb-2" for="login-username">
                Username
            </label>
            <input
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                id="login-username"
                type="text"
                placeholder="Username"
                bind:value={username}
                on:keyup={loginOnEnter}>
            <p class="{usernameWarning ? 'block' : 'hidden'} text-red-500 mt-2 text-xs italic">{usernameWarning}</p>
        </div>
        <div class="mb-4">
            <label class="block text-gray-700 text-sm font-bold mb-2" for="login-password">
                Password
            </label>
            <input
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                id="password"
                type="password"
                placeholder="******************"
                bind:value={password}
                on:keyup={loginOnEnter}>
            <p class="{passwordWarning ? 'block' : 'hidden'} text-red-500 mt-2 text-xs italic">{passwordWarning}</p>
        </div>
        <div class="{inError ? 'block' : 'hidden'} mb-6">
            <p class="text-red-500 mt-2 text-xs">Incorrect login attempt! Please try again.</p>
        </div>
        <div class="flex items-center justify-between">
            <button
                class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                type="button"
                on:click|preventDefault={login}>
                Login
            </button>
        </div>
    </form>
</div>
