import React, { useEffect, useState } from "react";

function GiftRecipientSelect({ selectedId, onChange }) {
    const [recipients, setRecipients] = useState([]);

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) return;

        fetch("https://localhost:7056/api/giftrecipient", {
            headers: { Authorization: `Bearer ${token}` }
        })
            .then(res => res.json())
            .then(data => setRecipients(data))
            .catch(err => console.error("? Failed to load recipients:", err));
    }, []);

    return (
        <div className="mb-3">
            <label className="form-label">?? Gift Recipient</label>
            <select
                className="form-select"
                value={selectedId || ""}
                onChange={(e) => onChange(e.target.value)}
            >
                <option value="">Select recipient...</option>
                {recipients.map(r => (
                    <option key={r.id} value={r.id}>
                        {r.name}, {r.address}
                    </option>
                ))}
            </select>
        </div>
    );
}

export default GiftRecipientSelect;
