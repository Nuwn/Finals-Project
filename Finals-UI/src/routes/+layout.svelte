<script>
	import Header from "./Header.svelte";
	import Footer from "./Footer.svelte";
    import Game from "./Game.svelte";
	import "./styles.css";
	import Modal from "./Modal.svelte";
    import { ModalActive } from './stores.js';

</script>

<div class="app">
	<Header />

	<main>
        <Game/>
        {#if $ModalActive}
        <Modal/> 
        {/if}
        <slot/>
    </main>

	<Footer />
</div>

<style>
	.app {
		display: flex;
		flex-direction: column;
		min-height: 100vh;
	}

	main {
		flex: 1;
		display: flex;
		flex-direction: column;
		padding: 1rem;
		width: 100%;
		max-width: 64rem;
		margin: 0 auto;
		box-sizing: border-box;
	}
</style>
