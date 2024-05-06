import {Button, Form} from "react-bootstrap";
import {useAppDispatch, useAppSelector} from "../redux/Hooks.tsx";
import {login} from "../redux/AuthSlice.tsx";
import {useEffect, useState} from "react";
import {useNavigate} from "react-router-dom";
import {SaveUserApi} from "../api/AppApi.tsx";

export function LoginPage() {
    const dispatch = useAppDispatch();
    const is_logged_in = useAppSelector((state) => state.auth.isLoggedIn);
    const navigate = useNavigate();
    const [usrnm, setUsrnm] = useState<string>("");

    const [loginError, setLoginError] = useState<boolean>(false);

    useEffect(() => {
        if (is_logged_in){
            navigate("/personal_page")
        }
    }, [is_logged_in]);

    return (
        <Form id="loginform">
            <Form.Group className="mb-3">
                <Form.Control id="emailText" type="email" placeholder="Enter email" onChange={(e) => setUsrnm(e.target.value)}/>
                {loginError && (
                    <h1>Некорректный логин</h1>
                )}
            </Form.Group>

            <Button variant="primary" type="submit" onClick={() => {
                SaveUserApi(usrnm)
                    .then((res) => {
                        if (res.data === "incorrect_login") {
                            setLoginError(true)
                        }
                        else {
                            setLoginError(false)
                            dispatch(login(usrnm));
                        }
                    });
            }}>
                Authorize
            </Button>
        </Form>
    )
}