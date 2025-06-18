// src/TransactionDetails.tsx
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

  // Helpers
  const getInitials = (label: string) =>
    label
      .split(" ")
      .map((word: string) => word[0])
      .join("")
      .toUpperCase();

  const getColor = (label: string) => {
    const colors = ["#A066FF", "#FF784A", "#4AC6FF", "#47D764", "#FFCC00"];
    let hash = 0;
    for (let i = 0; i < label.length; i++) {
      hash = label.charCodeAt(i) + ((hash << 5) - hash);
    }
    return colors[Math.abs(hash) % colors.length];
  };

  const isMoneyTransfer = transaction.typeName === "Money Transfer";
  const initials = getInitials(transaction.typeName);
  const color = getColor(transaction.typeName);

  const formattedDate = new Date(transaction.date).toLocaleString("hr-HR", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });

  return (
    <div className="friend-details-screen text-white bg-black min-vh-100 d-flex flex-column align-items-center justify-content-start pt-5 px-3">
      <div
        className="avatar-lg d-flex align-items-center justify-content-center mb-4"
        style={{
          backgroundColor: color,
          width: 100,
          height: 100,
          borderRadius: "50%",
          fontSize: "2rem",
        }}
      >
        {initials}
      </div>

      <h3>{transaction.typeName}</h3>
      <p className="text-warning fw-semibold fs-5">
        {transaction.transactionTypeId === 0 ? "Outgoing" : "Incoming"}
      </p>

      <div className="mt-4 text-center">
        <h1 className="text-success">€{transaction.amount}</h1>
        <div className="text-secondary mt-2">{formattedDate}</div>
        <div className="mt-4">
          {transaction.transactionTypeId === 0 ? (
            <span className="badge bg-info">Outgoing</span>
          ) : (
            <span className="badge bg-success">Incoming</span>
          )}
        </div>
      </div>

      {isMoneyTransfer && (
        <div className="mt-5 d-flex flex-column gap-3 w-100">
          <button
            className="btn btn-success w-100"
            onClick={() =>
              navigate("/request-money", {
                state: { friend: transaction.name || transaction.typeName },
              })
            }
          >
            Request Money
          </button>
          <button
            className="btn btn-primary w-100"
            onClick={() =>
              navigate("/payment-options", {
                state: {
                  title: `Send to ${transaction.name || transaction.typeName}`,
                },
              })
            }
          >
            Send Money
          </button>
        </div>
      )}

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
