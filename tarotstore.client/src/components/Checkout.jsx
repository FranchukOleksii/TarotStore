import React, { useState } from "react";
import GiftRecipientSelect from "./GiftRecipientSelect";

function Checkout({ cart, clearCart }) {
    const [submitted, setSubmitted] = useState(false);
    const [orderSubmitted, setOrderSubmitted] = useState(false);
    const [form, setForm] = useState({
        address: "",
        giftRecipientId: ""
    });

    const handleSubmit = async () => {
        const token = localStorage.getItem("token");

        const orderBody = {
            productId: cart[0]?.id,
            amount: cart.length,
            priceAtPurchase: cart.reduce((s, p) => s + p.price, 0),
            giftRecipientId: form.giftRecipientId || null
        };

        try {
            const response = await fetch("https://localhost:7056/api/order", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify(orderBody)
            });

            if (response.ok) {
                console.log("✅ Order sent to server");
                setSubmitted(true);
                setOrderSubmitted(true);
                clearCart();
            } else {
                console.error("❌ Failed to send order");
            }
        } catch (error) {
            console.error("❌ Server error:", error);
        }
    };

    if (submitted) {
        return <div className="container mt-4 alert alert-success">✅ Thank you for your order!</div>;
    }

    return (
        <div className="container mt-4">
            <h2>🛒 Checkout</h2>
            <ul className="list-group mb-3">
                {cart.map((item, index) => (
                    <li key={index} className="list-group-item">
                        {item.name} — ${item.price}
                    </li>
                ))}
            </ul>
            <div className="mb-3">
                <label className="form-label">Address</label>
                <input
                    className="form-control"
                    value={form.address}
                    onChange={(e) => setForm({ ...form, address: e.target.value })}
                />
            </div>

            <GiftRecipientSelect
                selectedId={form.giftRecipientId}
                onChange={(id) => setForm({ ...form, giftRecipientId: id })}
            />

            <button className="btn btn-success mt-3" onClick={handleSubmit}>
                Confirm Order
            </button>
        </div>
    );
}

export default Checkout;
