<script>
	import { onMount } from "svelte";
	import { GetHighscore, GetScore } from "../api";
	import { jwtcookie } from "../stores.js";

	let score;
	let highscore;

	onMount(async () => {
		GetScore()
			.then((data) => {
				score = data;
			})
			.catch((err) => {
				console.log(err);
			});

		GetHighscore()
			.then((data) => {
				highscore = data;
			})
			.catch((err) => {
				console.log(err);
			});
	});
</script>

<div class="user">
	{#if score}
		<div class="score">
			<div>
				<p>{score.solved}</p>
				<p>Solved</p>
			</div>
			<div>
				<p>{score.attempts}</p>
				<p>Attempts</p>
			</div>
		</div>
	{/if}

	{#if highscore}
    <div class="highscore">
            <h1>Top 10</h1>
			{#each highscore as hs}
				<div class="list-item">
					<p>{hs.userName}</p>
					<p>Solved: {hs.solved}</p>
					<p>Attempts: {hs.attempts}</p>
				</div>
			{/each}
		</div>
	{/if}

	<button on:click={() => jwtcookie.remove()}>Logout</button>
</div>

<style lang="scss">
	.user {
		width: 300px;
		display: flex;
		flex-direction: column;
		align-items: center;

		.score {
			display: flex;
            width: 100%;
            justify-content: space-evenly;

			div {
				display: flex;
				flex-direction: column;
				align-items: center;

				p {
					margin: 0;
				}
				p:first-child {
					font-size: 3rem;
				}
			}
		}
		.highscore {
			display: flex;
			flex-direction: column;
            padding-top: 30px;

			h1 {
				margin: 0 0 10px 0;
			}

			.list-item {
				display: grid;
                grid-template-columns: 1fr 1fr auto;
				min-height: 30px;

				p {
					text-transform: capitalize;
					margin: 0;
					padding: 5px 10px;
				}
			}
		}
		button {
			width: 60%;
			margin-top: 20px;
			background-color: inherit;
			color: inherit;
			border: 0;
			cursor: pointer;
			padding: 10px;
			font-size: 1.2rem;
		}
	}
</style>
