import React, { useEffect, useState } from "react";

function AdminPanel() {
    const [users, setUsers] = useState([]);
    const [error, setError] = useState("");

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) return;

        fetch("https://localhost:7056/api/user/all", {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
            .then(res => {
                if (!res.ok) throw new Error("Not authorized");
                return res.json();
            })
            .then(data => setUsers(data))
            .catch(err => setError("❌ Access denied or not admin"));
    }, []);

    const changeRole = async (userId, newRoleId) => {
        const token = localStorage.getItem("token");
        const res = await fetch(`https://localhost:7056/api/user/changerole/${userId}?newRoleId=${newRoleId}`, {
            method: "PUT",
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        if (res.ok) {
            alert("✅ Role updated");
            window.location.reload();
        } else {
            alert("❌ Failed to update role");
        }
    };

    const deleteUser = async (userId) => {
        const token = localStorage.getItem("token");
        const res = await fetch(`https://localhost:7056/api/user/${userId}`, {
            method: "DELETE",
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        if (res.ok) {
            alert("🗑️ User deleted");
            window.location.reload();
        } else {
            alert("❌ Failed to delete");
        }
    };


    return (
        <div className="container mt-4">
            <h2>Admin Panel 🛡️</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            {users.length > 0 && (
                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Email</th>
                            <th>Role ID</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((u) => (
                            <tr key={u.id}>
                                <td>{u.id}</td>
                                <td>{u.email}</td>
                                <td>{u.role}</td>
                                <td>
                                    <button className="btn btn-sm btn-warning me-2" onClick={() => changeRole(u.id, 1)}>
                                        Make Admin
                                    </button>
                                    <button className="btn btn-sm btn-danger" onClick={() => deleteUser(u.id)}>
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default AdminPanel;
