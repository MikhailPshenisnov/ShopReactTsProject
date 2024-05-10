import Header from "./components/Header.tsx";
import Footer from "./components/Footer.tsx";
import {Category, Pokedex} from "./components/Pokedex.tsx";
import {useEffect, useState} from "react";
import './index.css';
import Items from "./components/Items.tsx";
import {Categories} from "./components/Categories.tsx";
import {ItemPage} from "./components/ItemPage.tsx";
import {BrowserRouter, Navigate, Route, Routes} from "react-router-dom";
import {LoginPage} from "./components/LoginPage.tsx";
import {PersonalPage} from "./components/PersonalPage.tsx";
import {UsernameType} from "./components/UsernameType.tsx";
import {useAppDispatch} from "./redux/Hooks.tsx";
import {login} from "./redux/AuthSlice.tsx";
import {GetProductsApi, GetUserApi, SetEmptyCookiesApi} from "./api/AppApi.tsx";


export function App() {
    const [data, setData] = useState<Pokedex[]>([]);
    const [orders, setOrders] = useState<Pokedex[]>([]);
    const [curCategory, setCurCategory] = useState<Category>(Category.All);
    const [curData, setCurData] = useState<Pokedex[]>([]);

    const [isOneItemMode, setIsOneItemMode] = useState<boolean>(false);
    const [curOneItem, setCurOneItem] = useState<Pokedex>();

    const dispatch = useAppDispatch();
    const [curUser, setCurUser] = useState<UsernameType>( {username: ""});
    
    useEffect(() => {
        SetEmptyCookiesApi();

        GetProductsApi().then((res) => {
            setData(res.data);
        });

        GetUserApi().then((res) => {
            setCurUser(res.data);
        });
    }, [])

    useEffect(() => {
        if (curCategory === Category.All) {
            setCurData(data);
        } else {
            setCurData(data.filter(el => el.category === curCategory));
        }
    }, [curCategory, data]);

    useEffect(() => {
        if (curUser.username !== "") {
            dispatch(login(curUser.username))
        }
    }, [curUser]);

    function addToOrder(newItem: Pokedex) {
        let flag: boolean = false;
        orders.map((oldItem: Pokedex) => {
            if (oldItem.id === newItem.id) {
                flag = true;
            }
        })

        if (!flag) {
            setOrders([...orders, newItem]);
        }
    }

    function deleteFromOrder(deleteItemId: number) {
        setOrders(orders.filter(el => el.id !== deleteItemId));
    }

    function chooseCategory(category: Category) {
        setCurCategory(category);
    }

    function ShowItemPage(product: Pokedex) {
        setIsOneItemMode(!isOneItemMode);
        setCurOneItem(product);
    }


    return (
        <div className='wrapper'>
            <BrowserRouter>
                <Header cartProducts={orders} onDelete={deleteFromOrder}/>
                <Routes>
                    <Route path="/" element={<Navigate to="/home"/>} />

                    <Route path="/home" element={
                        <>
                            <Categories chooseCategory={chooseCategory}/>
                            <Items curData={curData} onAdd={addToOrder} onShowItemPage={ShowItemPage}/>
                            {isOneItemMode && (
                                <ItemPage product={curOneItem!} onAdd={addToOrder} onShowItemPage={ShowItemPage}/>)}
                        </>
                    } />
                    <Route path="/about" element={
                        <div className="about">
                            <h2>О нас</h2>
                            <h3>
                                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tincidunt faucibus
                                tincidunt.
                                Nulla auctor rutrum sapien, vel cursus tortor elementum blandit. Suspendisse pulvinar
                                eget
                                lorem sit amet tincidunt. Nunc vitae eleifend urna. In vitae posuere risus, eget
                                suscipit
                                lacus.
                                Praesent diam tellus, iaculis vel pulvinar quis, consequat eu nulla. Vestibulum non
                                accumsan
                                metus. Fusce tellus ipsum, volutpat eget ligula nec, fermentum posuere velit. Phasellus
                                vitae enim
                                mi.
                            </h3>
                        </div>
                    }/>
                    <Route path="/contacts" element={
                        <div className="contacts">
                            <h2>Контакты</h2>
                            <h3>Email: buy-me-a-cup-of-coffee@please.com</h3>
                            <h3>Телефон: 8(800)555-35-35</h3>
                        </div>
                    }/>
                    <Route path="/login" element={
                        <LoginPage/>
                    }/>
                    <Route path="/personal_page" element={
                        <PersonalPage/>
                    }/>

                    <Route path="*" element={<Navigate to="/home"/>}/>
                </Routes>

                <Footer/>
            </BrowserRouter>
        </div>
    )
}

export default App;
