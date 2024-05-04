import {Button} from "react-bootstrap";
import {useAppDispatch, useAppSelector} from "../redux/Hooks.tsx";
import {logout} from "../redux/AuthSlice.tsx";
import {useEffect} from "react";
import {useNavigate} from "react-router-dom";
import {SaveUserApi} from "../api/AppApi.tsx";

export function PersonalPage() {
    const dispatch = useAppDispatch();
    const is_logged_in = useAppSelector((state) => state.auth.isLoggedIn);
    const username = useAppSelector((state) => state.auth.username);
    const navigate = useNavigate();

    useEffect(() => {
        if (!is_logged_in) {
            navigate("/home")
        }
    }, [is_logged_in]);

    return (
        <div className="personal-info">
            <h2>Hello, {username}!</h2>
            <Button variant="primary" onClick={() => {
                dispatch(logout());
                SaveUserApi("@");
            }}>
                Logout
            </Button>
        </div>
    )
}