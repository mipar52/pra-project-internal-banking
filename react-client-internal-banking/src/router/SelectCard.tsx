// src/router/SelectCard.tsx
import React from "react";
import { useNavigate } from "react-router-dom";

const cards = [
  { id: 1, type: "VISA", last4: "6141" },
  { id: 2, type: "Mastercard", last4: "8820" },
  { id: 3, type: "Revolut", last4: "1029" },
];

const SelectCard: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="bg-black text-white min-vh-100 p-4">
      <h4 className="mb-4 text-info">Select Payment Card</h4>
      {cards.map((card) => (
        <div
          key={card.id}
          className="bg-dark p-3 mb-3 rounded border border-secondary"
          role="button"
          onClick={() => navigate("/topup")} // Optionally save selection
        >
          <strong>{card.type}</strong>
          <br />
          <small>•••• •••• •••• {card.last4}</small>
        </div>
      ))}
    </div>
  );
};

export default SelectCard;
