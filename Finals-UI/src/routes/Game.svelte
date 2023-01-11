<script>
	import { onMount } from "svelte";
    import { CheckQuiz, GetQuiz } from "./api.js";
	import { jwtcookie, solvedCookie } from "./stores.js";

    const height = '60px';
    const width = '60px';
    
    let quiz;
    let values = [];
    let solved = false;

    onMount(async () => {
        GetQuiz()
        .then(data => {
            values = solvedCookie.Get();

            quiz = {
                operators: data.operators.split(','),
                results: data.results.split(','), 
            }
            const obj = JSON.parse(data.numbers);

            for(let k in obj) {
                values[k] = obj[k];
            }

            solved = solvedCookie.IsSolved();
            console.log(values)
        })
        .catch(error => {
            console.error(error);
        });
    });


    async function HandleSubmit(event) {
        const formData = new FormData(event.target);

        let numbers = [];

        for (let index = 0; index < 9; index++) {
            numbers.push(formData.get(`field${index}`) || 0);
        }

        CheckQuiz(numbers.join(""))
            .then(data => {
                if(data)
                    solvedCookie.Set(values);
            })
            .catch(err => {
                console.log(err);
                if(err.code === 401)
                    jwtcookie.remove();
            });
    }

</script>

<section>
    {#if quiz}
    <form on:submit|preventDefault="{HandleSubmit}">
        <div class="grid">
            <input style="width:{width}; height:{height};" type="text" inputmode="numeric" pattern="[0-9]*" name={'field0'} bind:value={values[0]} />
            <p style="width:{width}; height:{height};">{quiz.operators[0]}</p>
            <input style="width:{width}; height:{height};" type="text" inputmode="numeric" pattern="[0-9]*" name={'field1'} bind:value={values[1]} />
            <p>{quiz.operators[1]}</p>
            <div style="width:{width}; height:{height};">
                <input type="text" inputmode="numeric" pattern="[0-9]*" name={'field2'} bind:value={values[2]} />
                <p class="result side">{quiz.results[0]}</p>
            </div>
            <p style="width:'{width}'; height:'{height}';">{quiz.operators[2]}</p>
            <p style="width:{width}; height:{height};"></p>
            <p style="width:{width}; height:{height};">{quiz.operators[3]}</p>
            <p style="width:{width}; height:{height};"></p>
            <p style="width:{width}; height:{height};">{quiz.operators[4]}</p>
            <input style="width:{width}; height:{height};" type="text" inputmode="numeric" pattern="[0-9]*" name={'field3'} bind:value={values[3]} />
            <p style="width:{width}; height:{height};">{quiz.operators[5]}</p>
            <input style="width:{width}; height:{height};" type="text" inputmode="numeric" pattern="[0-9]*" name={'field4'} bind:value={values[4]} />
            <p style="width:{width}; height:{height};">{quiz.operators[6]}</p>
            <div style="width:{width}; height:{height};">
                <input  type="text" inputmode="numeric" pattern="[0-9]*" name={'field5'} bind:value={values[5]} />
                <p class="result side">{quiz.results[1]}</p>
            </div>
            <p style="width:{width}; height:{height};">{quiz.operators[7]}</p>
            <p style="width:{width}; height:{height};"></p>
            <p style="width:{width}; height:{height};">{quiz.operators[8]}</p>
            <p style="width:{width}; height:{height};"></p>
            <p style="width:{width}; height:{height};">{quiz.operators[9]}</p>
            <div style="width:{width}; height:{height};">
                <input  type="text" inputmode="numeric" pattern="[0-9]*" name={'field6'} bind:value={values[6]}/>
                <p class="result bottom">{quiz.results[3]}</p>
            </div>
            <p style="width:{width}; height:{height};">{quiz.operators[10]}</p>
            <div style="width:{width}; height:{height};">
                <input  type="text" inputmode="numeric" pattern="[0-9]*" name={'field7'} bind:value={values[7]}/>
                <p class="result bottom">{quiz.results[4]}</p>
            </div>
            <p style="width:{width}; height:{height};">{quiz.operators[11]}</p>
            <div style="width:{width}; height:{height};">
                <input  type="text" inputmode="numeric" pattern="[0-9]*" name={'field8'} bind:value={values[8]} />
                <p class="result side">{quiz.results[2]}</p>
                <p class="result bottom">{quiz.results[5]}</p>
            </div>
        </div>
        <button type="submit" disabled= {solved}>Solve</button>
    </form>
    {/if}

    <div class="rules">
        <h3>How to play</h3>
        <p>Calculate each row to be equal the numbers to the right,</p>
        <p>Calculate each column to be equal the numbers on the bottom</p>
        <p>Only whole numbers 0-9 is allowed.</p>
    </div>
</section>

<style lang="scss">
	section {
        position: relative;
		display: flex;
		flex-direction: column;
		justify-content: flex-end;
		align-items: center;
		flex: 0.6;

        form{
            display: flex;
            flex-direction: column;
            
            .grid {
                display: grid;
                grid-template-columns: repeat(5, 1fr);
                grid-template-rows: repeat(3, 1fr);

                p {
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 1.2em;
                    margin: 0;
                    width: 60px;
                    height: 60px;
                }

                input {
                    width: 100%;
                    height: 100%;
                    padding: 10px;
                    font-size: 16px;
                    font-weight: 600;
                    border-radius: 5px;
                    border: 2px solid white;
                    text-align: center;
                    font-size: 2rem;
                    box-sizing: border-box;
                    color: rgb(39, 39, 39);
                }

                div{
                    position: relative;

                    .result{
                        position: absolute;
                    }
                    .side{
                        top:0;
                        left: 100%
                    }
                    .bottom{
                        top:100%;
                        left:0;
                    }
                }
            }

            button{
                margin-top: 70px;
                border-radius: 5px;
                border: 2px solid white;
            }
        }

        .rules{
            border: 1px solid rgb(71, 22, 22);
            margin-top: 50px;
            padding: 20px;
        }
    }
    
    button {
        width: 100%;
        height: 100%;
        box-sizing: border-box;
        padding: 10px;
        font-size: 20px;
        border: 0;

        &:disabled{
            background-color: rgb(46, 46, 46);
            color: rgb(148, 148, 148);
            border: 0;
        }
    }
</style>
