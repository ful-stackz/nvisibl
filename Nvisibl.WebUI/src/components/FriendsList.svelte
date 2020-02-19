<script lang="ts">
    import { onMount, onDestroy } from 'svelte';
    import Api from '../server/api';
    import User from '../models/user';
    import Chatroom from '../models/chatroom';
    import SessionManager from '../services/sessionManager';

    export let api: Api = null;
    export let sessionManager: SessionManager = null;

    interface FriendRequest {
        id: number,
        sender: {
            id: number,
            username: string,
        },
        receiver: {
            id: number,
            username: string,
        }
    }

    let user: User = sessionManager.get().auth.user;
    let visibleFriends: User[] = [];
    let isLoading: boolean = true;
    let inError: boolean = false;
    let friendRequests: FriendRequest[] = [];
    let friendRequestsIsLoading: boolean = true;
    let friendRequestsInError: boolean = false;
    let addFriendUsername: string = '';
    let addFriendMessage: string = '';
    let addFriendMessageColor: string = '';
    let addFriendMessageTimeout: NodeJS.Timeout = null;

    const friendsSub = sessionManager.get().friends.onChange
        .subscribe((current) => visibleFriends = current);

    const friendRequestsSub = sessionManager.get().friends.onFriendRequest
        .subscribe((request) => {
            if (friendRequests.find((req) => req.id === request.id)) {
                friendRequests = friendRequests.filter((req) => req.id !== request.id);
            } else if (!request.accepted && request.sender.id !== user.id) {
                friendRequests = [...friendRequests, request];
            }
        });

    onMount(() => {
        if (!api) throw new Error('Api prop is not provided.');
        if (!sessionManager) throw new Error('SessionManager prop is not provided.');
        const session = sessionManager.get();
        const { user, authToken: { token } } = session.auth;
        api.get(`users/${user.id}/friends`, null, token)
            .then(({ data }) => {
                if (!data) {
                    isLoading = false;
                    return;
                }
                data.forEach(({ id, username }) => {
                    session.friends.add(new User(id, username));
                });
                isLoading = false;
            })
            .catch((_) => {
                isLoading = false;
                inError = true;
            });
        api.get(`users/${user.id}/friend-requests`, null, token)
            .then(({ data }: { data: FriendRequest[] }) => {
                friendRequests = data.filter((req) => req.receiver.id === user.id);
                friendRequestsIsLoading = false;
            })
            .catch((error) => {
                console.error(error);
                friendRequestsIsLoading = false;
                friendRequestsInError = true;
            });
    });

    onDestroy(() => {
        friendsSub.unsubscribe();
        friendRequestsSub.unsubscribe();
    });

    function handleFriendClick(friend: User): void {
        const { chatrooms, chatService } = sessionManager.get();
        const chatroom = chatrooms.getAll().find((chatroom) => {
            return chatroom.isShared
                ? false
                : chatroom.users.find((user) => user.id === friend.id);
        });
        if (chatroom) {
            chatService.setActiveChatroom(chatroom);
        } else {
            createPrivateChatroom(friend)
                .then((cr: Chatroom) => {
                    chatrooms.add(cr);
                    chatService.setActiveChatroom(cr);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }

    async function createPrivateChatroom(friend: User): Promise<Chatroom> {
        const { user, authToken: { token } } = sessionManager.get().auth;
        const { data } = await api.post(
            'chatrooms',
            {
                ownerId: user.id,
                isShared: false,
            },
            token,
        );
        const chatroom = new Chatroom(data.id, friend.username, data.isShared, [user, friend]);
        await api.post(
            `chatrooms/${chatroom.id}/users`,
            {
                userId: friend.id,
            },
            token,
        );
        return chatroom;
    }

    function answerFriendRequest(request: FriendRequest, accept: boolean): void {
        const { user, authToken: { token } } = sessionManager.get().auth;
        api.post(`users/${user.id}/friend-requests/${request.id}`, { accept }, token)
            .catch(console.error);
    }

    function showFriendMessage(message: string, type: string): void {
        switch (type) {
            case 'success':
                addFriendMessageColor = 'text-green-400';
                break;

            case 'warning':
                addFriendMessageColor = 'text-orange-500';
                break;

            case 'error':
                addFriendMessageColor = 'text-red-400';
                break;
        }
        addFriendMessage = message;
        clearInterval(addFriendMessageTimeout);
        addFriendMessageTimeout = setTimeout(() => addFriendMessage = '', 4000);
    }

    function sendFriendRequest(): void {
        if (!addFriendUsername) return;
        const friendUsername = addFriendUsername;
        addFriendUsername = '';
        const { user, authToken: { token } } = sessionManager.get().auth;
        if (friendUsername === user.username) {
            showFriendMessage('Cannot add yourself as a friend.', 'warning');
            return;
        } else if (visibleFriends.find((f) => f.username === friendUsername)) {
            showFriendMessage(`${friendUsername} is already a friend.`, 'warning');
            return;
        }

        api.get('users/find', { username: friendUsername }, token)
            .then(({ data }: { data: { id: number, username: string } }) => {
                api.post(`users/${data.id}/friend-requests`, { senderId: user.id }, token)
                    .then(() => showFriendMessage('Friend request sent!', 'success'))
                    .catch((error) => {
                        console.error(error);
                        const { response } = error;
                        if (response.status === 409) showFriendMessage('Friend request has already been sent.', 'warning');
                        else showFriendMessage('Could not send friend request.', 'error');
                    });
            })
            .catch((error) => {
                console.error(error);
                const { response } = error;
                if (response.status === 404) showFriendMessage(`${friendUsername} does not exist.`, 'error');
            });
    }

    function sendFriendRequestOnEnter(e: KeyboardEvent): void {
        if (e.keyCode === 13) sendFriendRequest();
    }
</script>

<div class="bg-gray-200 p-3 rounded">
    <div class="text-lg">Friends</div>
    {#if isLoading}
        <div>Loading...</div>
    {:else if visibleFriends.length > 0}
        {#each visibleFriends as friend}
            <div
                class="flex flex-row p-3 cursor-pointer rounded hover:bg-gray-300"
                on:click={handleFriendClick(friend)}>
                <div class="flex-none bg-gray-400 rounded-full w-6 h-6"></div>
                <div class="flex-shrink truncate ml-2">{friend.username}</div>
            </div>
        {/each}
    {:else if inError}
        <div class="text-red-800">Could not load friends</div>
    {:else}
        <div class="text-xs">No friends</div>
    {/if}

    <div class="text-lg mt-2">Friend requests</div>
    {#if friendRequestsIsLoading}
        <div>Loading...</div>
    {:else if friendRequests.length > 0}
        {#each friendRequests as request}
            <div class="flex flex-row p-3 rounded">
                <div class="flex-grow truncate ml-2">{request.sender.username}</div>
                <div
                    class="flex-none rounded-full w-6 h-6 cursor-pointer hover:bg-gray-400 p-1"
                    on:click={answerFriendRequest(request, true)}
                >
                    <svg viewBox="0 0 20 20">
                        <path fill="current" d="M7.629,14.566c0.125,0.125,0.291,0.188,0.456,0.188c0.164,0,0.329-0.062,0.456-0.188l8.219-8.221c0.252-0.252,0.252-0.659,0-0.911c-0.252-0.252-0.659-0.252-0.911,0l-7.764,7.763L4.152,9.267c-0.252-0.251-0.66-0.251-0.911,0c-0.252,0.252-0.252,0.66,0,0.911L7.629,14.566z"></path>
                    </svg>
                </div>
                <div
                    class="flex-none rounded-full w-6 h-6 cursor-pointer hover:bg-gray-400 p-1"
                    on:click={answerFriendRequest(request, false)}
                >
                    <svg viewBox="0 0 20 20">
                        <path fill="current" d="M15.898,4.045c-0.271-0.272-0.713-0.272-0.986,0l-4.71,4.711L5.493,4.045c-0.272-0.272-0.714-0.272-0.986,0s-0.272,0.714,0,0.986l4.709,4.711l-4.71,4.711c-0.272,0.271-0.272,0.713,0,0.986c0.136,0.136,0.314,0.203,0.492,0.203c0.179,0,0.357-0.067,0.493-0.203l4.711-4.711l4.71,4.711c0.137,0.136,0.314,0.203,0.494,0.203c0.178,0,0.355-0.067,0.492-0.203c0.273-0.273,0.273-0.715,0-0.986l-4.711-4.711l4.711-4.711C16.172,4.759,16.172,4.317,15.898,4.045z"></path>
                    </svg>
                </div>
            </div>
        {/each}
    {:else if friendRequestsInError}
        <div class="text-red-800">Could not load friends</div>
    {:else}
        <div class="text-xs">No requests</div>
    {/if}

    <div class="text-lg mt-2">Send friend request</div>
    <div class="flex flex-row mt-2">
        <div class="flex-grow mr-2">
            <input
                class="w-full rounded p-1"
                placeholder="Friend username..."
                type="text"
                bind:value={addFriendUsername}
                on:keyup={sendFriendRequestOnEnter} />
        </div>
        <div class="flex-none self-center rounded-full w-8 h-8 {!!addFriendUsername ? 'cursor-pointer' : 'cursor-not-allowed'} hover:bg-gray-400 p-1" on:click={sendFriendRequest}>
            <svg viewBox="0 0 20 20">
                <path fill="current" d="M16.999,4.975L16.999,4.975C16.999,4.975,16.999,4.975,16.999,4.975c-0.419-0.4-0.979-0.654-1.604-0.654H4.606c-0.584,0-1.104,0.236-1.514,0.593C3.076,4.928,3.05,4.925,3.037,4.943C3.034,4.945,3.035,4.95,3.032,4.953C2.574,5.379,2.276,5.975,2.276,6.649v6.702c0,1.285,1.045,2.329,2.33,2.329h10.79c1.285,0,2.328-1.044,2.328-2.329V6.649C17.724,5.989,17.441,5.399,16.999,4.975z M15.396,5.356c0.098,0,0.183,0.035,0.273,0.055l-5.668,4.735L4.382,5.401c0.075-0.014,0.145-0.045,0.224-0.045H15.396z M16.688,13.351c0,0.712-0.581,1.294-1.293,1.294H4.606c-0.714,0-1.294-0.582-1.294-1.294V6.649c0-0.235,0.081-0.445,0.192-0.636l6.162,5.205c0.096,0.081,0.215,0.122,0.334,0.122c0.118,0,0.235-0.041,0.333-0.12l6.189-5.171c0.099,0.181,0.168,0.38,0.168,0.6V13.351z"></path>
            </svg>
        </div>
    </div>
    <div class="text-sm mt-1 {addFriendMessageColor}">{addFriendMessage}</div>
</div>
