import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";


function Login({ onLogin }) {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loggedIn, setLoggedIn] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            setLoggedIn(true);
        }
    }, []);

    const handleLogin = async (e) => {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch(`https://localhost:7056/api/user/login?email=${email}&password=${password}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                }
            });

            if (response.ok) {
                const data = await response.json();
                const token = data.token;

                localStorage.setItem("token", token);
                setLoggedIn(true);
                onLogin && onLogin();
                navigate("/profile");
            } else {
                const msg = await response.text();
                setError(msg || "Login failed");
            }
        } catch (err) {
            setError("Server error");
        }
    };

    const handleLogout = () => {
        localStorage.removeItem("token");
        setLoggedIn(false);
    };

    return (
        <div className="container mt-4">
            <h2>{loggedIn ? "Welcome Back!" : "Login"}</h2>

            {loggedIn ? (
                <div>
                    <div className="alert alert-success">✅ You are logged in!</div>
                    <button className="btn btn-warning" onClick={handleLogout}>Logout</button>
                </div>
            ) : (
                <form onSubmit={handleLogin}>
                    {error && <div className="alert alert-danger">❌ {error}</div>}
                    <input
                        className="form-control mb-2"
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                    <input
                        className="form-control mb-2"
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    <button className="btn btn-primary" type="submit">Login</button>
                </form>
            )}
        </div>
    );
}

export default Login;
