import {FaCartShopping} from "react-icons/fa6";
import {useState} from "react";
import {Pokedex} from "./Pokedex.tsx";
import Order from "./Order.tsx";
import {NavLink} from "react-router-dom";
import {useAppSelector} from "../redux/Hooks.tsx";

const showOrders = (props: CartProps) => {
    let total = 0;
    props.cartProducts.map(order => (total += order.price));
    return (
        <div>
            {props.cartProducts.map(order => (
                <Order key={order.id} product={order} onDelete={props.onDelete}/>
            ))}
            <p className='total'>Итого: {new Intl.NumberFormat().format(total)}$</p>
        </div>
    )
}

const showEmptyCart = () => {
    return (
        <div className="empty-cart">
            <h2>Корзина пуста</h2>
        </div>
    )
}

type CartProps = {
    cartProducts: Pokedex[],
    onDelete: (deleteItemId: number) => void,
};

export default function Header(props: CartProps) {
    const [isCartOpen, setIsCartOpen] = useState(false);

    const is_logged_in = useAppSelector((state) => state.auth.isLoggedIn);
    const username = useAppSelector((state) => state.auth.username);

    return (
        <header>
            <div>
                <span className="logo" >
                    <NavLink to={"/home"} style={{textDecoration: "none", color: "black"}}>Whatever u need</NavLink>
                </span>
                <ul className="nav">
                    <li>
                        <NavLink to={"/about"} style={{textDecoration: "none", color: "black"}}>О нас</NavLink>
                    </li>
                    <li>
                        <NavLink to={"/contacts"} style={{textDecoration: "none", color: "black"}}>Контакты</NavLink>
                    </li>
                    <li>
                        <NavLink to={"/login"} style={{textDecoration: "none", color: "black"}}>
                            {!is_logged_in && (
                                <>Личный кабинет</>
                            )}
                            {is_logged_in && (
                                <>{username}</>
                            )}

                        </NavLink>
                    </li>
                </ul>
                <FaCartShopping className={`cart ${isCartOpen && "active"}`}
                                onClick={() => setIsCartOpen(!isCartOpen)}/>
                {isCartOpen && (
                    <div className="cart-panel">
                        {props.cartProducts.length > 0 ? showOrders(props) : showEmptyCart()}
                    </div>
                )}
            </div>
            <div className="presentation"/>
        </header>
    )
}