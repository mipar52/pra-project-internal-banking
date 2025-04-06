import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

interface Card {
  id: number;
  type: string;
  last4: string;
}

const SelectCard: React.FC = () => {
  const navigate = useNavigate();
  const [cards, setCards] = useState<Card[]>([
    { id: 1, type: "VISA", last4: "6141" },
    { id: 2, type: "Mastercard", last4: "8820" },
    { id: 3, type: "Revolut", last4: "1029" },
  ]);

  const [newCard, setNewCard] = useState({ type: "", last4: "" });
  const [showForm, setShowForm] = useState(false);

  const handleAddCard = () => {
    if (!newCard.type || !newCard.last4) return;
    const nextId = cards.length + 1;
    setCards([...cards, { id: nextId, type: newCard.type, last4: newCard.last4 }]);
    setNewCard({ type: "", last4: "" });
    setShowForm(false);
  };

  return (
    <div className="bg-black text-white min-vh-100 p-4">
      <h4 className="mb-4 text-info">Select Payment Card</h4>

      {cards.map((card) => (
        <div
          key={card.id}
          className="bg-dark p-3 mb-3 rounded border border-secondary"
          role="button"
          onClick={() => navigate("/topup")}
        >
          <strong>{card.type}</strong>
          <br />
          <small> 1234-***-****-1234 {card.last4}</small>
        </div>
      ))}

      {showForm ? (
        <div className="bg-dark p-3 rounded border border-info">
          <div className="mb-2">
            <label className="form-label">Card Type</label>
            <input
              className="form-control bg-black text-white border-secondary"
              value={newCard.type}
              onChange={(e) => setNewCard({ ...newCard, type: e.target.value })}
              placeholder="e.g. VISA, Mastercard"
            />
          </div>
          <div className="mb-3">
            <label className="form-label">Last 4 Digits</label>
            <input
              className="form-control bg-black text-white border-secondary"
              maxLength={4}
              value={newCard.last4}
              onChange={(e) => setNewCard({ ...newCard, last4: e.target.value })}
              placeholder="1234"
            />
          </div>
          <button className="btn btn-info w-100" onClick={handleAddCard}>
            Add Card
          </button>
        </div>
      ) : (
        <button
          className="btn btn-outline-info w-100"
          onClick={() => setShowForm(true)}
        >
          + Add New Card
        </button>
      )}
    </div>
  );
};

export default SelectCard;
