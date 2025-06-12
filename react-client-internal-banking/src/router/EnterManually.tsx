import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import algebraLogo from "../assets/algebra-logo.png";
import SuccessPopup from "../components/SuccessPopup";
import ErrorPopup from "../components/ErrorPopup";
import LoadingBar from "../components/LoadingBar";
import "../css/EnterManully.css";

const categories = [
  { label: "Food", emoji: "🍔", id: 1 },
  { label: "Parking", emoji: "🅿️", id: 2 },
  { label: "Money transfer", emoji: "📄", id: 4 },
  { label: "Tuition", emoji: "🎓", id: 3 },
];

const EnterManually: React.FC = () => {
  const [amount, setAmount] = useState<string>("");
  const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
  const location = useLocation();
  const [showSuccess, setShowSuccess] = useState(false);
  const [showError, setShowError] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [showLoading, setShowLoading] = useState(false);

  const handleCategorySelect = (id: number) => {
    setSelectedCategory(id);
  };

  const handleSubmit = async () => {
    setShowLoading(true);
    const userEmail = localStorage.getItem("userEmail");

    if (!userEmail || !amount || selectedCategory === null) {
      setErrorMessage("Please fill all fields.");
      //alert("Please fill all fields.");
      setShowError(true);
      setTimeout(() => setShowError(false), 4000);
      return;
    }

    const payload = {
      userId: 0, // server may ignore this
      userEmail: userEmail,
      transactionTypeId: selectedCategory,
      amount: parseFloat(amount),
      date: new Date().toISOString(),
    };

    try {
      await axios.post(
        "http://localhost:5026/api/Transaction/TransactionCreateByMail",
        payload,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("jwtToken")}`,
          },
        }
      );
      setTimeout(() => setShowSuccess(true), 4000); // Auto-dismiss
    } catch (err: any) {
      console.error("Error submitting payment:", err);

      const message =
        err.response?.data?.message || // backend error message
        err.response?.data || // fallback to raw response
        err.message || // Axios-level message
        "Something went wrong with the payment.";

      setErrorMessage(message);
      setShowError(true);
      setTimeout(() => setShowError(false), 4000);
    } finally {
      setTimeout(() => setShowLoading(false), 4000);
    }
  };

  return (
    <div className="enter-manually-wrapper">
      {/* Header */}
      <div className="enter-manually-logo">
        <img src={algebraLogo} alt="Algebra Logo" style={{ height: 40 }} />
      </div>

      {/* Form Fields */}
      <div className="enter-manually-form overflow-auto px-3">
        <div className="mb-4">
          <label className="form-label">Amount (€)</label>
          <input
            className="form-control"
            type="number"
            step="0.01"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            placeholder="Enter amount"
          />
        </div>
        {/* Payment Category */}
        <div className="mb-3">
          <label className="form-label">Payment Category</label>
          <div className="d-grid gap-2">
            {categories.map((cat) => (
              <button
                key={cat.id}
                className={`btn btn-outline-light text-start ${
                  selectedCategory === cat.id ? "active bg-info" : ""
                }`}
                onClick={() => handleCategorySelect(cat.id)}
              >
                {cat.emoji} {cat.label}
              </button>
            ))}
          </div>
        </div>
        <div style={{ height: "100px" }} /> {/* Safe space for nav */}
      </div>
      {showSuccess && (
        <SuccessPopup
          message="Payment submitted successfully!"
          onClose={() => setShowSuccess(false)}
        />
      )}
      {showError && (
        <ErrorPopup
          message={errorMessage}
          onClose={() => setShowError(false)}
        />
      )}
      {showLoading && (
        <LoadingBar message="Processing your payment... Hang tight!" />
      )}

      {/* Pay Button */}
      <div className="enter-manually-button-container">
        <button className="btn" onClick={handleSubmit}>
          💸 Pay
        </button>
      </div>
    </div>
  );
};

export default EnterManually;
