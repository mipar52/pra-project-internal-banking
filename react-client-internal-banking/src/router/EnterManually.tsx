import React, { useState } from "react";
import algebraLogo from "../assets/algebra-logo.png";
import "../css/EnterManully.css";

const EnterManually: React.FC = () => {
  const [formData, setFormData] = useState({
    recipientName: "",
    iban: "",
    amount: "",
    currency: "EUR",
    model: "",
    referenceNumber: "",
    description: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = () => {
    console.log("Payment submitted:", formData);
  };

  return (
    <div className="enter-manually-wrapper">
      {/* Header */}
      <div className="enter-manually-logo">
        <img src={algebraLogo} alt="Algebra Logo" style={{ height: 40 }} />
      </div>

      {/* Form Fields */}
      <div className="enter-manually-form overflow-auto px-3">
        <div className="mb-3">
          <label className="form-label">Recipient Name</label>
          <input
            className="form-control"
            name="recipientName"
            value={formData.recipientName}
            onChange={handleChange}
            placeholder="Sveučilište Algebra"
          />
        </div>
        <div className="mb-3">
          <label className="form-label">IBAN</label>
          <input
            className="form-control"
            name="iban"
            value={formData.iban}
            onChange={handleChange}
            placeholder="HR7023600001102894251"
          />
        </div>
        <div className="row">
          <div className="col-6 mb-3">
            <label className="form-label">Amount</label>
            <input
              className="form-control"
              name="amount"
              value={formData.amount}
              onChange={handleChange}
              placeholder="385.62"
            />
          </div>
          <div className="col-6 mb-3">
            <label className="form-label">Currency</label>
            <input
              className="form-control"
              name="currency"
              value={formData.currency}
              readOnly
            />
          </div>
        </div>
        <div className="row">
          <div className="col-6 mb-3">
            <label className="form-label">Model</label>
            <input
              className="form-control"
              name="model"
              value={formData.model}
              onChange={handleChange}
              placeholder="HR01"
            />
          </div>
          <div className="col-6 mb-3">
            <label className="form-label">Reference Number</label>
            <input
              className="form-control"
              name="referenceNumber"
              value={formData.referenceNumber}
              onChange={handleChange}
              placeholder="102024-26292-8"
            />
          </div>
        </div>
        <div className="mb-3">
          <label className="form-label">Payment Description</label>
          <input
            className="form-control"
            name="description"
            value={formData.description}
            onChange={handleChange}
            placeholder="Rata 7."
          />
        </div>
        <div style={{ height: "100px" }} /> {/* Safe space for nav */}
      </div>

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
