import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "../css/FriendDetails.css";

const TransactionDetails: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const transaction = location.state;

  if (!transaction) {
    return (
      <div className="container text-center text-white mt-5">
        <h4>Transaction not found</h4>
        <button className="btn btn-secondary mt-3" onClick={() => navigate(-1)}>
          Go back
        </button>
      </div>
    );
  }

  return (
    <div className="friend-details-screen text-white bg-black min-vh-100 d-flex flex-column align-items-center justify-content-start pt-5 px-3">
      <div
        className="avatar-lg d-flex align-items-center justify-content-center mb-4"
        style={{ backgroundColor: transaction.color }}
      >
        {transaction.initials}
      </div>
      <h3>{transaction.name}</h3>
      <p className="text-warning fw-semibold fs-5">{transaction.label}</p>

      <div className="mt-4 text-center">
        <h1 className="text-success">€{transaction.amount}</h1>
        <div className="text-secondary mt-2">{transaction.time}</div>
        <div className="mt-4">
          {transaction.message.includes("ti šalje") ? (
            <span className="badge bg-success">Incoming</span>
          ) : (
            <span className="badge bg-info">Outgoing</span>
          )}
        </div>
      </div>

      {/* Action Buttons */}
      <div className="mt-5 d-flex flex-column gap-3 w-100">
        <button
          className="btn btn-success w-100"
          onClick={() =>
            navigate("/request-money", { state: { friend: transaction.name } })
          }
        >
          Request Money
        </button>
        <button
          className="btn btn-primary w-100"
          onClick={() =>
            navigate("/payment-options", {
              state: { title: `Send to ${transaction.name}` },
            })
          }
        >
          Send Money
        </button>
      </div>

      <button
        className="btn btn-outline-light mt-5"
        onClick={() => navigate(-1)}
      >
        ← Back
      </button>
    </div>
  );
};

export default TransactionDetails;
