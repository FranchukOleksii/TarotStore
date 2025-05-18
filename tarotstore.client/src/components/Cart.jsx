import React, { useState, useEffect } from 'react';

function Cart({ cart, removeFromCart, clearCart, orderSubmitted, setOrderSubmitted }) {
    const [showCheckout, setShowCheckout] = useState(false);
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [submitted, setSubmitted] = useState(false);

    const total = cart.reduce((sum, item) => sum + item.price, 0);

    useEffect(() => {
        if (!orderSubmitted) {
            setSubmitted(false);
            setShowCheckout(false);
        }
    }, [orderSubmitted]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const order = {
            customer: { name, email },
            items: cart,
            total: total.toFixed(2)
        };

        //console.log("Order submitted:", order);
        const token = localStorage.getItem("token");

        const orderBody = {
            userId: 0, // якщо UserId на сервері визначатиметься з токена, цей можна не відправляти
            productId: cart[0]?.id, // тимчасово беремо перший товар
            amount: cart.length, // або окремо порахувати
            //priceAtPurchase: cart.reduce((s, p) => s + p.price, 0)
        };

        //const response = fetch("https://localhost:7056/api/order", {
        //    method: "POST",
        //    headers: {
        //        "Content-Type": "application/json",
        //        Authorization: `Bearer ${token}`
        //    },
        //    body: JSON.stringify(orderBody)
        //});

        const response = await fetch("https://localhost:7056/api/order", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(orderBody)
        });

        console.log("Status:", response.status);

        if (response.status === 201) {
            console.log("✅ Order sent to server");
            setSubmitted(true);
            setOrderSubmitted(true);
            clearCart();
        } else {
            console.error(`❌ Failed to send order: Status ${response.status}`);
        }

        setSubmitted(true);
        setOrderSubmitted(true);
        clearCart();
    };

    return (
        <div className="container mt-4">
            <h3>🛒 Cart</h3>

            {submitted && (
                <div className="alert alert-success mt-4 text-center">
                    ✅ Thank you for your order, <strong>{name}</strong>! <br />
                    We’ll contact you at <strong>{email}</strong>.
                </div>
            )}

            {!submitted && cart.length === 0 && <p>Your cart is empty</p>}

            {!submitted && cart.length > 0 && (
                <>
                    <ul className="list-group mb-3">
                        {cart.map((item, index) => (
                            <li key={index} className="list-group-item d-flex justify-content-between align-items-center">
                                <span>{item.name} — ${item.price}</span>
                                <button className="btn btn-sm btn-danger" onClick={() => removeFromCart(index)}>
                                    Remove
                                </button>
                            </li>
                        ))}
                    </ul>

                    <div className="alert alert-info text-end">
                        <strong>Total: ${total.toFixed(2)}</strong>
                    </div>

                    {!showCheckout && (
                        <button className="btn btn-success" onClick={() => setShowCheckout(true)}>
                            Place Order
                        </button>
                    )}

                    {showCheckout && (
                        <form className="mt-4" onSubmit={handleSubmit}>
                            <div className="mb-3">
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="Your Name"
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="mb-3">
                                <input
                                    type="email"
                                    className="form-control"
                                    placeholder="Your Email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>
                            <button className="btn btn-primary" type="submit">
                                Submit Order
                            </button>
                        </form>
                    )}
                </>
            )}
        </div>
    );
}

export default Cart;
