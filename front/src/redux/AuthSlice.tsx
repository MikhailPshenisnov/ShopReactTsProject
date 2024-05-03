import {createSlice, type PayloadAction} from '@reduxjs/toolkit'

export interface AuthState {
    isLoggedIn: boolean;
    username: string;
    aToken: Token;
    rToken: Token;
}

interface Token {
    isPresent: boolean;
    token: string;
}

const initialState: AuthState = {
    isLoggedIn: false,
    username: "",
    aToken: {
        isPresent: false,
        token: ""
    },
    rToken: {
        isPresent: false,
        token: ""
    }
}

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        login: (state, action: PayloadAction<string>) => {
            state.isLoggedIn = true;
            state.username = action.payload;
        },
        logout: (state) => {
            state.isLoggedIn = false;
            state.username = "";
        },
    },
})

export const {login, logout} = authSlice.actions

// export default counterSlice.reducer