import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import algebraLogo from "../assets/algebra-logo.png";

const RequestMoney: React.FC = () => {
  const [amount, setAmount] = useState<number>(0);
  const [selectedReason, setSelectedReason] = useState<string>("");
  const [recipientName, setRecipientName] = useState<string>("");
  const navigate = useNavigate();
  const location = useLocation();
  const friendName = location.state?.friend || "Friend";

  const increase = () => setAmount((prev) => prev + 1);
  const decrease = () => setAmount((prev) => (prev > 0 ? prev - 1 : 0));
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setAmount(parseFloat(e.target.value) || 0);
  };

  const handleSubmit = () => {
    console.log(`Requesting €${amount} from ${recipientName || friendName} for ${selectedReason}`);
    navigate("/dashboard");
  };

  const reasons = [
    { label: "Food", icon: "🍔", color: "warning" },
    { label: "Drinks", icon: "🍹", color: "info" },
    { label: "Parking", icon: "🅿️", color: "primary" },
    { label: "Bills", icon: "🧾", color: "secondary" },
    { label: "Tuition", icon: "🎓", color: "danger" },
    { label: "Other", icon: "💬", color: "light" },
  ];

  return (
    <div className="text-white bg-black vh-100 d-flex flex-column">
      {/* Header */}
      <div className="bg-info text-center py-3 position-relative">
        <button
          onClick={() => navigate(-1)}
          className="position-absolute start-0 top-50 translate-middle-y btn btn-link text-white"
        >
          ✕
        </button>
        <h5 className="mb-0">Request from {friendName}</h5>
      </div>

      {/* Recipient Input */}
      <div className="px-3 pt-4">
        <label className="form-label">Recipient Name</label>
        <input
          type="text"
          className="form-control bg-dark text-white"
          placeholder="Enter name"
          value={recipientName}
          onChange={(e) => setRecipientName(e.target.value)}
        />
      </div>

      {/* Amount Section */}
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
            onChange={handleChange}
            style={{ width: "120px" }}
          />
          <button className="btn btn-outline-primary fs-3" onClick={increase}>
            +
          </button>
        </div>
      </div>

      {/* Description Reason */}
      <div className="px-3 mb-4">
        <label className="form-label">Reason</label>
        <div className="d-flex flex-wrap gap-2">
          {reasons.map((reason) => (
            <button
              key={reason.label}
              className={`btn btn-sm btn-${selectedReason === reason.label ? reason.color : "outline-light"}`}
              onClick={() => setSelectedReason(reason.label)}
            >
              {reason.icon} {reason.label}
            </button>
          ))}
        </div>
      </div>

      {/* Submit Button */}
      <div className="p-3" style={{marginBottom:"80px"}}>
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
