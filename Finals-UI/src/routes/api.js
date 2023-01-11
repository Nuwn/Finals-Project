import { getToken } from './stores.js';

const url = "http://localhost:5200/api";

function customFetch(url, options) {
    return new Promise((resolve, reject) => {
        fetch(url, options)
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    reject({ code: response.status, message: response.statusText });
                }
            })
            .then(data => {
                resolve(data);
            })
            .catch(error => {
                reject(error);
            });
    });
}

export async function GetQuiz() {
    return customFetch(`${url}/quiz`);
}

export async function CheckQuiz(numbers) {
    const token = getToken();

    const options = {
        headers: {
            Authorization: `Bearer ${token}`
        }
    };
    return customFetch(`${url}/quiz/check/${numbers}`, token !== undefined ? options : null);
}

export async function GetProfile() {
    const token = getToken();

    if (typeof token === 'undefined') {
        return Promise.reject('Error: Unauthorised');
    }

    const options = {
        headers: {
            Authorization: `Bearer ${token}`
        }
    };
    return customFetch(`${url}/users/profile`, options);
}

export async function Login(email, password) {
    const options = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ Email: email, Password: password }),
    };

    return customFetch(`${url}/users/login`, options);

}

export async function Register(username, email, password) {
    const options = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ Username: username, Email: email, Password: password }),
    };

    return customFetch(`${url}/users/register`, options);
}

export async function GetScore(){
    const token = getToken();

    if (typeof token === 'undefined') {
        return Promise.reject('Error: Unauthorised');
    }

    const options = {
        headers: {
            Authorization: `Bearer ${token}`
        }
    };
    return customFetch(`${url}/score/personal`, options);
}

export async function GetHighscore(){
    return customFetch(`${url}/score/highscore`);
}