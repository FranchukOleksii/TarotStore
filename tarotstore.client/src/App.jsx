//import React, { useState } from 'react';
//import ProductList from './components/ProductList.jsx';
//import Cart from './components/Cart.jsx';
//import Register from './components/Register.jsx';
//import Login from './components/Login.jsx';
//import SecretZone from './components/SecretZone.jsx';
//import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
////import AdminPanel from "./components/AdminPanel";

//function App() {
//    const [cart, setCart] = useState([]);
//    const [orderSubmitted, setOrderSubmitted] = useState(false);

//    const addToCart = (product) => {
//        if (orderSubmitted) setOrderSubmitted(false);
//        setCart([...cart, product]);
//    };

//    const removeFromCart = (index) => {
//        const updatedCart = [...cart];
//        updatedCart.splice(index, 1);
//        setCart(updatedCart);
//    };

//    const clearCart = () => {
//        setCart([]);
//    };

//    return (
//        <div className="App">
//            <h1 className="text-center mt-4">🔮 Tarot Store</h1>

//            <Register />
//            <Login />
//            <SecretZone />
//            {/*<Route path="/admin" element={<AdminPanel />} />*/}

//            <ProductList addToCart={addToCart} />
//            <Cart
//                cart={cart}
//                removeFromCart={removeFromCart}
//                clearCart={clearCart}
//                orderSubmitted={orderSubmitted}
//                setOrderSubmitted={setOrderSubmitted}
//            />
//        </div>
//    );
//}

//export default App;
//===================================================================================================
//import React from "react";
//import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
//import AdminPanel from "./components/AdminPanel";
//// можеш додати й інші компоненти пізніше (наприклад, Home, Login, Profile)
//import Profile from "./components/Profile";
//function App() {
//    return (
//        <Router>
//            <div className="container">
//                <h1>Tarot Store 🃏</h1>

//                <Routes>
//                    {/* 🔮 Додаткові маршрути додаси тут */}
//                    <Route path="/admin" element={<AdminPanel />} />
//                </Routes>

//                <Route path="/profile" element={<Profile />} />
//            </div>
//        </Router>
//    );
//}

//export default App;
//===========================================================================

import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import Navbar from "./components/Navbar";
import AppRoutes from "./routes/AppRoutes";

function App() {
    return (
        <Router>
            <Navbar />
            <AppRoutes />
        </Router>
    );
}

export default App;