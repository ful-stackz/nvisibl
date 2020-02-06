<script lang="ts">
    import session from '../stores/session';

    function logout(): void {
        session.clear();
    }
</script>

<div class="bg-gray-200 p-3 rounded rounded-t-none">
    <span>Welcome, {($session).user.username}!</span>
    <a
        href="/#"
        class="float-right"
        on:click|preventDefault={logout}>
        Logout
    </a>
</div>
