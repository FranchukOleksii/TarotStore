import React, { useState } from "react";

function SecretZone() {
    const [message, setMessage] = useState("");
    const [error, setError] = useState("");

    const handleFetch = async () => {
        const token = localStorage.getItem("token");

        if (!token) {
            setError("You are not logged in.");
            return;
        }

        try {
            const response = await fetch("https://localhost:7056/api/user/me", {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            if (response.ok) {
                const data = await response.json();
                setMessage(data.message);
            } else {
                const text = await response.text();
                setError(text || "Unauthorized");
            }
        } catch (err) {
            setError("Server error");
        }
    };

    return (
        <div className="container mt-4">
            <h2>Secret Zone</h2>
            <button className="btn btn-dark mb-2" onClick={handleFetch}>
                Fetch Protected Data
            </button>

            {message && <div className="alert alert-success">✅ {message}</div>}
            {error && <div className="alert alert-danger">❌ {error}</div>}
        </div>
    );
}

export default SecretZone;
