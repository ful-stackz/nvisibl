import App from './App.svelte';
import Api from './server/api';
import SessionManager from './services/sessionManager';

const api: Api = new Api('https://nvisibl.azurewebsites.net/api/');
const webSocketAddress: string = 'wss://nvisibl.azurewebsites.net/ws';
const sessionManager = new SessionManager(api, webSocketAddress);

const app = new App({
    target: document.body,
    props: {
        api,
        sessionManager,
    },
});

export default app;
