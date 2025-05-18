export const getToken = () => localStorage.getItem("token");

export const isLoggedIn = () => !!getToken();

export const logout = () => {
    localStorage.removeItem("token");
    window.location.href = "/login";
};

export const getUserRole = () => {
    const token = getToken();
    if (!token) return null;

    try {
        const payload = token.split('.')[1];
        const decoded = JSON.parse(atob(payload));

        return decoded["role"];

    } catch (e) {
        console.error("Failed to decode JWT", e);
        return null;
    }
};
