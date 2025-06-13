// src/router/PaymentOptionScreen.tsx
import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import algebraLogo from "../assets/algebra-logo.png";

const PaymentOptionScreen: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { title = "Payment" } = location.state || {};

  return (
    <div className="min-vh-100 bg-black text-white p-4 d-flex flex-column align-items-center">
      <img
        src={algebraLogo}
        alt="Algebra Logo"
        className="mb-4"
        style={{ height: 70 }}
      />
      <h4 className="text-center mb-4">{title}</h4>

      <div className="card bg-dark text-white mb-3 w-100 p-3 border border-primary">
        <h5>Enter manually</h5>
        <p className="small text-secondary">
          Choose to enter the information manually. You’ll need to provide the
          IBAN, amount, recipient name, and reason (or choose a predefined one).
        </p>
        <button
          className="btn btn-primary w-100"
          onClick={() => navigate("/enter-manually")}
        >
          Enter Manually
        </button>
      </div>

      <div className="card bg-dark text-white w-100 p-3 border border-info">
        <h5>Scan the QR Code</h5>
        <p className="small text-secondary">
          Choose to pay by scanning a QR code. If it’s valid, all payment
          details will be filled in for you.
        </p>
        <button className="btn btn-info w-100" onClick={() => navigate("/qr")}>
          Scan QR Code
        </button>
      </div>
    </div>
  );
};

export default PaymentOptionScreen;
