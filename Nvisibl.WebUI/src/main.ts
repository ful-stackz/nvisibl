import App from './App.svelte';
import Api from './server/api';

const api: Api = new Api('https://localhost:5001/api/');
const webSocketAddress: string = 'wss://localhost:5001/ws/chat';

const app = new App({
    target: document.body,
    props: {
        api,
        webSocketAddress,
    },
});

export default app;
