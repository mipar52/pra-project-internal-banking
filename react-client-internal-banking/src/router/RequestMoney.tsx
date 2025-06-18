// src/RequestMoney.tsx
import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import axios from "axios";
import algebraLogo from "../assets/algebra-logo.png";
import SuccessPopup from "../components/SuccessPopup";
import ErrorPopup from "../components/ErrorPopup";
import LoadingBar from "../components/LoadingBar";

interface Friend {
  firstName: string;
  lastName: string;
  emailAddress: string;
}

const RequestMoney: React.FC = () => {
  const [amount, setAmount] = useState<number>(0);
  const [selectedReason, setSelectedReason] = useState<string>("");
  const [recipientEmail, setRecipientEmail] = useState<string>("");
  const [friends, setFriends] = useState<Friend[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const navigate = useNavigate();
  const location = useLocation();
  const friendName = location.state?.friend || "Friend";

  const increase = () => setAmount((prev) => prev + 1);
  const decrease = () => setAmount((prev) => (prev > 0 ? prev - 1 : 0));

  useEffect(() => {
    const fetchFriends = async () => {
      const userEmail = localStorage.getItem("userEmail");
      const token = localStorage.getItem("jwtToken");
      if (!userEmail || !token) return;

      try {
        const response = await axios.get(
          `http://localhost:5026/api/Friend/GetFriendsByEmail/${encodeURIComponent(
            userEmail
          )}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setFriends(response.data);
      } catch (err) {
        setError("Failed to load friends list.");
      }
    };

    fetchFriends();
  }, []);

  const handleSubmit = async () => {
    const userSenderEmail = localStorage.getItem("userEmail");
    const token = localStorage.getItem("jwtToken");
    if (!recipientEmail || !userSenderEmail || !amount || !token) {
      setError("Please fill out all fields correctly.");
      return;
    }

    setLoading(true);
    setError("");
    setSuccess("");

    try {
      await axios.post(
        "http://localhost:5026/api/Transaction/RequestTransferCreateWithMain",
        {
          userReceiverEmail: recipientEmail,
          userSenderEmail,
          date: new Date().toISOString(),
          amount,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setSuccess("Request sent successfully!");
      setTimeout(() => navigate("/dashboard"), 2000);
    } catch (err) {
      setError("Failed to send request.");
    } finally {
      setLoading(false);
    }
  };

  const reasons = [
    { label: "Food", icon: "🍔", color: "warning" },
    { label: "Drinks", icon: "🍹", color: "info" },
    { label: "Parking", icon: "🅿️", color: "primary" },
    { label: "Bills", icon: "🗾", color: "secondary" },
    { label: "Tuition", icon: "🎓", color: "danger" },
    { label: "Other", icon: "💬", color: "light" },
  ];

  return (
    <div className="text-white bg-black vh-100 d-flex flex-column">
      {loading && <LoadingBar message="Sending money request..." />}
      {success && (
        <SuccessPopup message={success} onClose={() => setSuccess("")} />
      )}
      {error && <ErrorPopup message={error} onClose={() => setError("")} />}

      <div className="bg-info text-center py-3 position-relative">
        <button
          onClick={() => navigate(-1)}
          className="position-absolute start-0 top-50 translate-middle-y btn btn-link text-white"
        >
          ✕
        </button>
        <h5 className="mb-0">Request from {friendName}</h5>
      </div>

      <div className="px-3 pt-4">
        <label className="form-label">Select Friend</label>
        <select
          className="form-select bg-dark text-white"
          value={recipientEmail}
          onChange={(e) => setRecipientEmail(e.target.value)}
        >
          <option value="">Choose a friend</option>
          {friends.map((friend) => (
            <option key={friend.emailAddress} value={friend.emailAddress}>
              {friend.firstName} {friend.lastName}
            </option>
          ))}
        </select>
      </div>

      <div className="flex-grow-1 d-flex flex-column justify-content-center align-items-center">
        <h6 className="mb-3">Requesting:</h6>
        <div className="d-flex align-items-center gap-4">
          <button className="btn btn-outline-primary fs-3" onClick={decrease}>
            −
          </button>
          <input
            type="number"
            className="form-control text-center fs-3 bg-dark text-white border-0"
            value={amount}
            onChange={(e) => setAmount(parseFloat(e.target.value) || 0)}
            style={{ width: "120px" }}
          />
          <button className="btn btn-outline-primary fs-3" onClick={increase}>
            +
          </button>
        </div>
      </div>

      <div className="px-3 mb-4">
        <label className="form-label">Reason</label>
        <div className="d-flex flex-wrap gap-2">
          {reasons.map((reason) => (
            <button
              key={reason.label}
              className={`btn btn-sm btn-${
                selectedReason === reason.label ? reason.color : "outline-light"
              }`}
              onClick={() => setSelectedReason(reason.label)}
            >
              {reason.icon} {reason.label}
            </button>
          ))}
        </div>
      </div>

      <div className="p-3" style={{ marginBottom: "80px" }}>
        <button
          className="btn btn-info w-100 text-white py-3 fw-bold mb-2"
          onClick={handleSubmit}
        >
          📤 Request Now
        </button>
      </div>
    </div>
  );
};

export default RequestMoney;
