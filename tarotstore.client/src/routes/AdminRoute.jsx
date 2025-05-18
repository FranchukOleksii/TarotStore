import React from "react";
import { Navigate } from "react-router-dom";
import { isLoggedIn, getUserRole } from "../utils/auth";

function AdminRoute({ children }) {
    const role = getUserRole();

    if (!isLoggedIn()) return <Navigate to="/login" />;
    if (role !== "Admin") return <Navigate to="/" />;

    return children;
}

export default AdminRoute;
