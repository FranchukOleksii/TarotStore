import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { isLoggedIn, getUserRole } from "../utils/auth";

function Navbar() {
    const navigate = useNavigate();
    const [logged, setLogged] = useState(isLoggedIn());

    useEffect(() => {
        const interval = setInterval(() => {
            setLogged(isLoggedIn());
        }, 500); 

        return () => clearInterval(interval); 
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("token");
        setLogged(false);
        navigate("/login");
    };

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light px-3">
            <Link to="/" className="navbar-brand">🔮 TarotStore</Link>
            <div className="navbar-nav ms-auto">
                {logged ? (
                    <>
                        <Link to="/profile" className="nav-link">Profile</Link>
                        {getUserRole() === "Admin" && (
                            <Link to="/admin" className="nav-link">Admin</Link>
                        )}

                        <button className="btn btn-outline-danger btn-sm ms-2" onClick={handleLogout}>Logout</button>
                    </>
                ) : (
                    <>
                        <Link to="/register" className="btn btn-outline-success btn-sm ms-2">Register</Link>
                        <Link to="/login" className="btn btn-outline-primary btn-sm ms-2">Login</Link>
                    </>
                )}
            </div>
        </nav>
    );
}

export default Navbar;
