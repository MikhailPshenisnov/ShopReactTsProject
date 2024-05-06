import axios from "axios";

export const appApiIns = axios.create({
    baseURL: 'http://localhost:5128/Test/',
    headers: {'Content-Type': 'application/json'},
})

export function GetProductsApi(){
    return appApiIns.get("GetProducts");
}

export function GetUserApi(){
    return appApiIns.get("GetUser");
}

export function SaveUserApi(username: string){
    return appApiIns.get(`SaveUser/${username}`);
}

export function SetSomeCookiesApi(username: string){
    return appApiIns.get(`CookiesTest/${username}`);
}