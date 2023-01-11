<script>
    import {ModalActive, jwtcookie } from '../stores.js';
    import { Login } from "../api.js";

	let email;
	let password;
    let errorMessage = "";

	async function HandleSubmit() {
        if(!email || !password) return;

        Login(email, password)
            .then(data => {
                jwtcookie.add(data.token);
                ModalActive.set(false);
            })
            .catch(error => {
                console.log(error)
                errorMessage = "Wrong information.";
            });    
	}
</script>

<div>
	<h1>Welcome back!</h1>
	<form on:submit|preventDefault={HandleSubmit}>
		<label>
			<p>Email:</p> 
			<input type="email" bind:value={email} />
		</label>
		<label>
			<p>Password:</p> 
			<input type="password" bind:value={password} />
		</label>
        {#if errorMessage != ""}
        <p>{errorMessage}</p>
        {/if}
        <div>
            <button type="submit" disabled={false}>Login</button>
        </div>
	</form>
</div>

<style lang="scss">
    $color: rgb(238, 238, 238);

    div{
        width: 400px;

        @media (max-width: 400px) {
            width: 100vw;
        }

        h1{
            margin: 0;
        }

        form{
            display: flex;
            flex-direction: column;
            align-items: center;

            label{
                display: flex;
                flex-direction: column;
                padding: 10px;
                width: 100%;

                p{
                    margin: 0;
                    padding-left: 5px;
                }
                input{
                    padding: 10px;
                    background-color: #333;
                    border: 0;
                    border-radius: 3px;
                    color: $color;
                    font-size: 1.1rem;

                    &:focus-visible{
                        outline-offset: 0px;
                        outline: 2px solid white;
                    }
                }
            }
            div{
                padding: 5px;
                margin-top: 25px;
                box-sizing: border-box;

                button[type="submit"]{
                    width: 100%;
                    padding: 10px;
                    font-size: 1.2rem;
                    font-weight: 600;
                    color: $color;
                    background-color: #333;
                    border-radius: 3px;
                    border:0;
                    text-transform: uppercase;
                }
            }
        }
    } 
</style>
