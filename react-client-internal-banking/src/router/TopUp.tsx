// src/TopUp.tsx
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import SuccessPopup from "../components/SuccessPopup";
import ErrorPopup from "../components/ErrorPopup";
import LoadingBar from "../components/LoadingBar";

const TopUp: React.FC = () => {
  const navigate = useNavigate();
  const [amount, setAmount] = useState<number>(0);
  const [isLoading, setIsLoading] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  const balance = parseFloat(localStorage.getItem("balance") || "0.00");

  const increase = () => setAmount((prev) => prev + 1);
  const decrease = () => setAmount((prev) => (prev > 0 ? prev - 1 : 0));

  const handleTopUp = async () => {
    const email = localStorage.getItem("userEmail");
    const jwt = localStorage.getItem("jwtToken");

    if (!email || !jwt) {
      setErrorMsg("Missing user credentials. Please log in again.");
      return;
    }

    if (amount <= 0) {
      setErrorMsg("Amount must be greater than 0.");
      return;
    }

    setIsLoading(true);

    try {
      const response = await fetch(
        "http://localhost:5026/api/Transaction/AddBalanceByMail",
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${jwt}`,
          },
          body: JSON.stringify({
            emailUser: email,
            amount,
            date: new Date().toISOString(),
            transactionTypeId: 5,
          }),
        }
      );

      if (!response.ok) {
        const err = await response.json();
        throw new Error(err?.message || "Top-up failed.");
      }

      // Update local balance
      const newBalance = balance + amount;
      localStorage.setItem("balance", newBalance.toFixed(2));

      setShowSuccess(true);
    } catch (err: any) {
      setErrorMsg(err.message || "An error occurred while topping up.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="text-white bg-black vh-100 d-flex flex-column">
      {/* Header */}
      <div className="bg-info text-center py-3 position-relative">
        <button
          onClick={() => navigate("/dashboard")}
          className="position-absolute start-0 top-50 translate-middle-y btn btn-link text-white"
        >
          ✕
        </button>
        <h5 className="mb-0">My KEKSICA</h5>
      </div>

      {/* Amount Section */}
      <div className="flex-grow-1 d-flex flex-column justify-content-center align-items-center">
        <h6 className="mb-3">Topping up:</h6>
        <div className="d-flex align-items-center gap-4">
          <button className="btn btn-outline-primary fs-3" onClick={decrease}>
            −
          </button>

          <input
            type="number"
            className="form-control text-center fs-2"
            value={amount}
            onChange={(e) => {
              const val = parseFloat(e.target.value);
              setAmount(isNaN(val) ? 0 : val);
            }}
            style={{
              width: "100px",
              backgroundColor: "#000",
              color: "#fff",
              border: "1px solid #444",
            }}
          />

          <button className="btn btn-outline-primary fs-3" onClick={increase}>
            +
          </button>
        </div>
        <p className="mt-3">
          Available: <strong>{balance.toFixed(2)} €</strong>
        </p>
      </div>

      {/* Card Info + Button */}
      <div className="bg-dark p-3" style={{ marginBottom: "80px" }}>
        <div
          className="d-flex align-items-center justify-content-between bg-black text-white p-2 rounded mb-3"
          role="button"
          onClick={() => navigate("/select-card")}
        >
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/4/41/Visa_Logo.png"
            alt="Visa"
            width={40}
          />
          <div className="ms-2">
            <small>Paying with:</small>
            <br />
            <strong>VISA 439649******6141</strong>
          </div>
          <span className="ms-auto">›</span>
        </div>
        <button
          className="btn btn-info text-white w-100 py-3 rounded-pill fw-bold"
          onClick={handleTopUp}
        >
          Press to Top Up
        </button>
      </div>

      {/* Popups */}
      {isLoading && <LoadingBar message="Processing your top-up..." />}
      {showSuccess && (
        <SuccessPopup
          message="Top-up successful!"
          onClose={() => {
            setShowSuccess(false);
            navigate("/dashboard");
          }}
        />
      )}
      {errorMsg && (
        <ErrorPopup message={errorMsg} onClose={() => setErrorMsg(null)} />
      )}
    </div>
  );
};

export default TopUp;
