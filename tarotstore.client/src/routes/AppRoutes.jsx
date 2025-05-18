import React from "react";
import { Routes, Route } from "react-router-dom";
import Home from "../pages/Home";
import Login from "../components/Login";
import Register from "../components/Register";
import Profile from "../components/Profile";
import AdminPanel from "../components/AdminPanel";
import PrivateRoute from "../routes/PrivateRoute";
import AdminRoute from "./AdminRoute";

function AppRoutes() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />

            <Route
                path="/profile"
                element={
                    <PrivateRoute>
                        <Profile />
                    </PrivateRoute>
                }
            />
            <Route
                path="/admin"
                element = {
                    <AdminRoute>
                        <AdminPanel />
                    </AdminRoute>
                }
            />
        </Routes>
    );
}

export default AppRoutes;
