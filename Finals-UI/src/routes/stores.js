import { writable } from 'svelte/store';
import Cookies from 'js-cookie';

export const ModalComponent = writable("");
export const ModalActive = writable(false);
export const isLoggedIn = writable(Cookies.get('token') != undefined);

function createCookieStore() {
	const { subscribe, set } = writable(Cookies.get('token'));

    return {
		subscribe,
		add: (token) => {
            set(token);
            Cookies.set('token', token);
            isLoggedIn.set(true);
        },
		remove:() =>  {
            set("");
            Cookies.remove('token', { path: '' })
            isLoggedIn.set(false);
        }
	};
}

export const jwtcookie = createCookieStore();
export const solvedCookie = {
    // accessor property(getter)
    data: {},
    Get() {
        this.data = Cookies.get('solved') 
            ? JSON.parse(Cookies.get('solved')) 
            : {
                values: Array(9).fill(undefined),
                solved: false
            };

        if(CreateToday() === this.date){
            this.values = Array(9).fill(undefined);
            this.solved = false;
        }

        console.log(this);
        return this.data.values;
    },
    Set(values){
        this.data.values = values;
        this.data.date = CreateToday();
        this.data.solved = true;
        Cookies.set('solved', JSON.stringify(this.data));
        console.log(this.data);
    },
    IsSolved(){
        return this.data.solved;
    }

};

function CreateToday(){
    const date = new Date();
    const year = date.getFullYear();
    const month = date.getMonth() + 1;
    const day = date.getDate();

    return `${day}-${month}-${year}`;
}

export function getToken() {
    return Cookies.get('token');
}