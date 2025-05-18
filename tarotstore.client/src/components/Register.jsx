import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

function Register() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        email: "",
        password: "",
        name: "",
        surname: "",
        lastName: "",
        birthDay: "",
        phoneNumber: "",
        address: ""
    });

    const [success, setSuccess] = useState(false);
    const [error, setError] = useState("");

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm({ ...form, [name]: value });
    };

    const handleRegister = async (e) => {
        e.preventDefault();
        setError("");
        setSuccess(false);

        const payload = {
            ...form,
            birthDay: form.birthDay ? new Date(form.birthDay).toISOString() : null,
            phoneNumber: form.phoneNumber ? parseFloat(form.phoneNumber) : null
        };

        try {
            const response = await fetch("https://localhost:7056/api/user", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (response.status === 204) {
                setSuccess(true);
                setForm({
                    email: "",
                    password: "",
                    name: "",
                    surname: "",
                    lastName: "",
                    birthDay: "",
                    phoneNumber: "",
                    address: ""
                });

                setTimeout(() => navigate("/login"), 2000);
            } else {
                const msg = await response.text();
                setError(msg || "Registration failed");
            }
        } catch (err) {
            setError("Server error");
        }
    };

    return (
        <div className="container mt-4">
            <h2>Register</h2>
            {success && <div className="alert alert-success">✅ Registration successful! Redirecting to login...</div>}
            {error && <div className="alert alert-danger">❌ {error}</div>}

            <form onSubmit={handleRegister}>
                <input className="form-control mb-2" type="email" name="email" placeholder="Email" value={form.email} onChange={handleChange} required />
                <input className="form-control mb-2" type="password" name="password" placeholder="Password" value={form.password} onChange={handleChange} required />

                <input className="form-control mb-2" name="name" placeholder="Name" value={form.name} onChange={handleChange} />
                <input className="form-control mb-2" name="surname" placeholder="Surname" value={form.surname} onChange={handleChange} />
                <input className="form-control mb-2" name="lastName" placeholder="Last Name" value={form.lastName} onChange={handleChange} />
                <input className="form-control mb-2" name="birthDay" type="date" placeholder="Birthday" value={form.birthDay} onChange={handleChange} />
                <input className="form-control mb-2" name="phoneNumber" placeholder="Phone Number" value={form.phoneNumber} onChange={handleChange} />
                <input className="form-control mb-2" name="address" placeholder="Address" value={form.address} onChange={handleChange} />

                <button className="btn btn-primary" type="submit">Register</button>
            </form>
        </div>
    );
}

export default Register;
