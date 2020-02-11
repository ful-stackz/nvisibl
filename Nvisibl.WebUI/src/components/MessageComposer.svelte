<script lang="ts">
    import { onMount } from 'svelte';
    import SessionManager from '../services/sessionManager';
    import Message from '../models/message';

    export let sessionManager: SessionManager = null;

    let value: string = '';
    let placeholder: string = 'Type your message...';
    $: canSend = !!value;

    function sendMessage(): void {
        if (!value) return;
        const { chatService } = sessionManager.get();
        chatService.sendMessage(value);
        value = '';
    }

    function sendMessageOnEnter(e: KeyboardEvent): void {
        if (e.ctrlKey && e.keyCode === 13) sendMessage();
    }
</script>

<div class="h-full bg-white rounded border-gray-200 border-2 overflow-y-auto p-2">
    <div class="flex flex-row max-h-full">
        <div class="flex-grow mr-2">
            <textarea
                class="resize-none w-full h-full"
                {placeholder}
                bind:value
                on:keyup={sendMessageOnEnter}></textarea>
        </div>
        <div class="flex-none self-center">
            <button
                class="{canSend ? 'bg-gray-200' : 'bg-transparent text-gray-200 cursor-not-allowed'} border border-gray-200 rounded p-2"
                on:click={sendMessage}>
                Send
            </button>
        </div>
    </div>
</div>