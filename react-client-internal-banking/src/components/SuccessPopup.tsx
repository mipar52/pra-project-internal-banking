// src/components/SuccessPopup.tsx
import React from "react";
import { FaCheckCircle } from "react-icons/fa";

interface SuccessPopupProps {
  message: string;
  onClose: () => void;
}

const SuccessPopup: React.FC<SuccessPopupProps> = ({ message, onClose }) => {
  return (
    <div
      className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center"
      style={{ backgroundColor: "rgba(0,0,0,0.5)", zIndex: 1050 }}
    >
      <div
        className="bg-success text-white p-4 shadow rounded-4 text-center"
        style={{ maxWidth: 400, width: "90%" }}
      >
        <FaCheckCircle size={40} className="mb-3" />
        <p className="mb-3 fs-5 fw-bold">{message}</p>
        <button className="btn btn-light rounded-pill px-4" onClick={onClose}>
          Continue
        </button>
      </div>
    </div>
  );
};

export default SuccessPopup;
