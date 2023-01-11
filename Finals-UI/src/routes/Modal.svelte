<script>
	import Login from "./forms/login.svelte";
    import Register from "./forms/register.svelte";
    import User from "./forms/user.svelte";
    import { ModalComponent, ModalActive }  from "./stores.js";

</script>

<div class="modal">
    <div class="modal-content">
        <button class="closebtn" on:click={() => ModalActive.set(false)}>
            <i class="fa-solid fa-xmark"></i>
        </button>
        {#if $ModalComponent === 'login'}
        <Login/>
        {:else if $ModalComponent === 'register'}
        <Register/>
        {:else if $ModalComponent === 'score'}
        <User/>
        {/if}
    </div>
</div>


<style lang="scss">
    $color: rgb(238, 238, 238);

    .modal{
        position: fixed;
        top:0;
        left: 0;
        z-index: 1;
        width: 100vw;
        height: 100vh;
        color: $color;
        background-color: rgba($color: #000000, $alpha: 0.95);
        display: grid;
        justify-content: center;
        align-content: center;
        
        .modal-content {
            background-color: #121212;
            border-radius: 10px;
            border: 1px solid #1a1a1b;
            padding: 10px;
            position: relative;
            display: flex;
            justify-content: center;
            flex-direction: column;
            
            .closebtn{
                position: absolute;
                top: 10px;
                right: 10px;
                height: 26px;
                width: 26px;
                vertical-align: middle;
                text-align: center;
                border: 0;
                cursor: pointer;
                background-color: inherit;
                color: $color;
            }
        }
    }
</style>