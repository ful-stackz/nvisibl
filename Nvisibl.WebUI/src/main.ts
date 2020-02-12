import App from './App.svelte';
import Api from './server/api';
import SessionManager from './services/sessionManager';

const api: Api = new Api('https://localhost:5001/api/');
const webSocketAddress: string = 'wss://localhost:5001/ws/chat';
const sessionManager = new SessionManager(webSocketAddress);

const app = new App({
    target: document.body,
    props: {
        api,
        sessionManager,
    },
});

export default app;
