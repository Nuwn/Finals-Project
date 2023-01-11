<script>
    import { isLoggedIn, ModalComponent, ModalActive, jwtcookie } from "./stores.js";
    import { GetProfile } from "./api";
    import Icon from "$lib/images/Icon.png";

    let username;

    $: if ($isLoggedIn) {
        GetProfile()
        .then(data => {
            username = data.username.charAt(0).toUpperCase() + data.username.slice(1);
        })
        .catch(error => {
            if(error.code === 401)
                jwtcookie.remove();
        });
    }

    function ShowModal(e){
        ModalComponent.set(e.target.name);
        ModalActive.set(true); 
    }
</script>

<header>
	<div class="corner">
        <img src="{Icon}" alt="">
	</div>

	<nav>
        {#if $isLoggedIn && username}
        <button on:click={e => ShowModal(e)} name="score">{username}</button>
        {:else}
        <button on:click={e => ShowModal(e)} name="login">Login</button>
        <button on:click={e => ShowModal(e)} name="register">Register</button>
        {/if}
	</nav>
</header>

<style lang=scss>
	header{
        display: flex;
        justify-content: space-between;
        height: 60px;
        background-color: rgba($color: #000000, $alpha: 0.2);

        .corner{
            padding: 15px 0 5px 15px;
            img{
                height:100%;
                filter: invert(1);
            }
        }

        nav{
            display: flex;
            align-items: center;
            padding-right: 10px;

            button {
                background-color: inherit;
                border: 0;
                color: white;
                font-size: 1.2em;
                padding:10px;
                cursor: pointer;
            }
        }
    }
</style>
