import React, { useEffect, useState } from "react";

function Profile() {
    const [profile, setProfile] = useState(null);
    const [edit, setEdit] = useState(false);
    const [form, setForm] = useState({});
    const [orders, setOrders] = useState([]);
    const [showPasswordForm, setShowPasswordForm] = useState(false);
    const [passwordForm, setPasswordForm] = useState({
        currentPassword: '',
        newPassword: ''
    });
    const [passwordError, setPasswordError] = useState("");
    const [passwordSuccess, setPasswordSuccess] = useState("");


    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) return;

        fetch("https://localhost:7056/api/user/profile", {
            headers: { Authorization: `Bearer ${token}` }
        })
            .then(res => res.json())
            .then(data => {
                setProfile(data);
                setForm(data);
            });

        fetch("https://localhost:7056/api/order/my-orders", {
            headers: { Authorization: `Bearer ${token}` }
        })
            .then(res => res.json())
            .then(setOrders);
    }, []);

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const token = localStorage.getItem("token");

        try {
            const response = await fetch(`https://localhost:7056/api/user`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify(form)
            });

            if (response.ok) {
                alert("✅ Profile updated!");
                setProfile(form);
                setEdit(false);
            } else {
                alert("❌ Failed to update");
            }
        } catch {
            alert("❌ Server error");
        }
    };

    const handleDelete = async () => {
        if (!window.confirm("❗ Are you sure you want to delete your profile? This cannot be undone.")) return;

        const token = localStorage.getItem("token");

        try {
            const response = await fetch("https://localhost:7056/api/user/deleteuserself", {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (response.ok) {
                localStorage.removeItem("token");
                alert("✅ Your profile was deleted.");
                window.location.href = "/";
            } else {
                alert("❌ Failed to delete profile");
            }
        } catch (error) {
            alert("❌ Server error");
            console.error(error);
        }
    };

    const handlePasswordChange = (e) => {
        setPasswordForm({ ...passwordForm, [e.target.name]: e.target.value });
    };

    const submitPasswordChange = async (e) => {
        e.preventDefault();
        setPasswordError("");
        setPasswordSuccess("");

        const token = localStorage.getItem("token");

        try {
            const response = await fetch("https://localhost:7056/api/user/change-password", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify(passwordForm)
            });

            if (response.ok) {
                setPasswordSuccess("✅ Password successfully saved.");
                setPasswordForm({ currentPassword: "", newPassword: "" });
                setShowPasswordForm(false);
            } else {
                const err = await response.text();
                setPasswordError(err || "❌ Password incorect error.");
            }
        } catch {
            setPasswordError("❌ Server error.");
        }
    };

    if (!profile) return <div className="container mt-4">Loading...</div>;

    return (
        <div className="container mt-4">
            <h2>👤 My Profile</h2>

            {!edit ? (
                <>
                    <ul className="list-group mb-3">
                        <li className="list-group-item">Email: {profile.email}</li>
                        <li className="list-group-item">Name: {profile.name}</li>
                        <li className="list-group-item">Surname: {profile.surname}</li>
                        <li className="list-group-item">Last Name: {profile.lastName}</li>
                        <li className="list-group-item">Birthday: {profile.birthDay}</li>
                        <li className="list-group-item">Phone: {profile.phoneNumber}</li>
                        <li className="list-group-item">Address: {profile.address}</li>
                    </ul>
                    <button className="btn btn-primary" onClick={() => setEdit(true)}>Edit</button>
                    <button className="btn btn-danger mt-3" onClick={handleDelete}> 🗑️ Delete My Profile </button>
                    {!profile.isEmailConfirmed && ( 
                        <button className="btn btn-warning" onClick={async () => {
                            const token = localStorage.getItem("token");
                            try {
                                const res = await fetch("https://localhost:7056/api/user/resend-confirmation", {
                                    method: "POST",
                                    headers: { Authorization: `Bearer ${token}` }
                                });
                                const msg = await res.text();
                                alert(msg);
                            } catch {
                                alert("❌ Failed to send confirmation email.");
                            }
                        }}>
                            🔁 Resend Email Confirmation
                        </button>
                    )}
                    <h4 className="mt-5">🧾 Order History</h4>
                    {orders.length === 0 ? (
                        <p>No orders yet.</p>
                    ) : (
                        <ul className="list-group">
                            {orders.map(order => (
                                <li key={order.id} className="list-group-item">
                                    <strong>{order.name}</strong> — {order.amount} pcs — 💰 {order.priceAtPurchase}$
                                </li>
                            ))}
                        </ul>
                    )}
                </>
            ) : (
                <form onSubmit={handleSubmit}>
                    <input name="name" className="form-control mb-2" placeholder="Name" value={form.name || ''} onChange={handleChange} />
                    <input name="surname" className="form-control mb-2" placeholder="Surname" value={form.surname || ''} onChange={handleChange} />
                    <input name="lastName" className="form-control mb-2" placeholder="Last name" value={form.lastName || ''} onChange={handleChange} />
                    <input name="birthDay" className="form-control mb-2" type="date" value={form.birthDay || ''} onChange={handleChange} />
                    <input name="phoneNumber" className="form-control mb-2" placeholder="Phone" value={form.phoneNumber || ''} onChange={handleChange} />
                    <input name="address" className="form-control mb-2" placeholder="Address" value={form.address || ''} onChange={handleChange} />
                    <button className="btn btn-success me-2" type="submit">Save</button>
                    <button className="btn btn-secondary" onClick={() => setEdit(false)}>Cancel</button>
                </form>
            )
            }
            {showPasswordForm ? (
                <form onSubmit={submitPasswordChange} className="mt-4">
                    {passwordSuccess && <div className="alert alert-success">{passwordSuccess}</div>}
                    {passwordError && <div className="alert alert-danger">{passwordError}</div>}

                    <input
                        type="password"
                        name="currentPassword"
                        placeholder="Current Password"
                        className="form-control mb-2"
                        value={passwordForm.currentPassword}
                        onChange={handlePasswordChange}
                        required
                    />
                    <input
                        type="password"
                        name="newPassword"
                        placeholder="New Password"
                        className="form-control mb-2"
                        value={passwordForm.newPassword}
                        onChange={handlePasswordChange}
                        required
                    />
                    <button type="submit" className="btn btn-success me-2">Save New Password</button>
                    <button type="button" className="btn btn-secondary" onClick={() => setShowPasswordForm(false)}>Cancel</button>
                </form>
            ) : (
                <button className="btn btn-outline-warning mt-3" onClick={() => setShowPasswordForm(true)}>Change Password</button>
            )}
        </div>
    );
}

export default Profile;
