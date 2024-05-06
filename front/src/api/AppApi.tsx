import axios from "axios";

export const appApiIns = axios.create({
    baseURL: 'http://localhost:5128/Test/',
    headers: {
        'Content-Type': 'application/json'
    },
    withCredentials: true
})

export function GetProductsApi(){
    return appApiIns.get("GetProducts");
}

export function GetUserApi(){
    return appApiIns.get("GetUser");
}

export function SaveUserApi(username: string | null){
    if (username === null){
        return appApiIns.get("SaveUser/");
    }
    return appApiIns.get(`SaveUser/${username}`);
}

export function SetEmptyCookiesApi(){
    return appApiIns.get(`SetEmptyCookies`);
}