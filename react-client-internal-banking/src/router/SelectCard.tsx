import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import LoadingBar from "../components/LoadingBar";

interface BackendCard {
  firstName: string;
  lastname: string;
  creditCardNumber: string;
  expiryDate: string;
}

const SelectCard: React.FC = () => {
  const navigate = useNavigate();
  const [cards, setCards] = useState<BackendCard[]>([]);
  const [newCard, setNewCard] = useState({
    firstName: "",
    lastName: "",
    creditCardNumber: "",
    expiryDate: "",
    cvv: "",
  });
  const [showForm, setShowForm] = useState(false);
  const [loading, setLoading] = useState(false);

  const userEmail = localStorage.getItem("userEmail");
  const token = localStorage.getItem("jwtToken");

  const fetchCards = async () => {
    if (!userEmail || !token) return;

    setLoading(true);
    try {
      const response = await axios.get(
        `http://localhost:5026/api/CreditCard/GetCardsByMail/${encodeURIComponent(
          userEmail
        )}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setCards(response.data);
    } catch (err) {
      console.error("Failed to fetch cards:", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCards();
  }, []);

  const handleDeleteCard = async (cardNumber: string) => {
    if (!userEmail || !cardNumber || !token) return;

    setLoading(true);
    try {
      await axios.delete(
        `http://localhost:5026/api/CreditCard/DeleteCardByNumberAndMail/${encodeURIComponent(
          userEmail
        )}/${cardNumber}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      fetchCards(); // refresh list
    } catch (err) {
      console.error("Failed to delete card:", err);
    } finally {
      setLoading(false);
    }
  };

  const handleAddCard = async () => {
    if (
      !newCard.firstName ||
      !newCard.lastName ||
      !newCard.creditCardNumber ||
      !newCard.expiryDate ||
      !newCard.cvv ||
      !userEmail
    )
      return;

    const payload = {
      userMail: userEmail,
      firstName: newCard.firstName,
      lastName: newCard.lastName,
      creditCardNumber: newCard.creditCardNumber,
      expiryDate: newCard.expiryDate,
      cvv: newCard.cvv,
    };

    setLoading(true);
    try {
      await axios.post(
        "http://localhost:5026/api/CreditCard/CreateCardByMail",
        payload,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setNewCard({
        firstName: "",
        lastName: "",
        creditCardNumber: "",
        expiryDate: "",
        cvv: "",
      });
      setShowForm(false);
      fetchCards(); // refresh list
    } catch (err) {
      console.error("Failed to add card:", err);
    } finally {
      setLoading(false);
    }
  };

  const maskCardNumber = (fullNumber: string) => {
    const last4 = fullNumber.slice(-4);
    return `**** **** **** ${last4}`;
  };

  return (
    <div className="bg-black text-white min-vh-100 p-4">
      {loading && <LoadingBar message="Processing card data..." />}
      <h4 className="mb-4 text-info">Select Payment Card</h4>

      {cards.map((card, idx) => (
        <div
          key={idx}
          className="bg-dark p-3 mb-3 rounded border border-secondary d-flex justify-content-between align-items-center"
          role="button"
        >
          <div onClick={() => navigate("/topup")}>
            <strong>
              {card.firstName} {card.lastname}
            </strong>
            <br />
            <small>{maskCardNumber(card.creditCardNumber)}</small>
          </div>
          <button
            className="btn btn-sm btn-outline-danger ms-3"
            onClick={() => handleDeleteCard(card.creditCardNumber)}
          >
            🗑️
          </button>
        </div>
      ))}

      {showForm ? (
        <div className="bg-dark p-3 rounded border border-info mt-4">
          <div className="mb-2">
            <label className="form-label">First Name</label>
            <input
              className="form-control bg-black text-white border-secondary"
              value={newCard.firstName}
              onChange={(e) =>
                setNewCard({ ...newCard, firstName: e.target.value })
              }
              placeholder="e.g. Ana"
            />
          </div>
          <div className="mb-2">
            <label className="form-label">Last Name</label>
            <input
              className="form-control bg-black text-white border-secondary"
              value={newCard.lastName}
              onChange={(e) =>
                setNewCard({ ...newCard, lastName: e.target.value })
              }
              placeholder="e.g. Horvat"
            />
          </div>
          <div className="mb-2">
            <label className="form-label">Card Number</label>
            <input
              className="form-control bg-black text-white border-secondary"
              value={newCard.creditCardNumber}
              onChange={(e) =>
                setNewCard({ ...newCard, creditCardNumber: e.target.value })
              }
              placeholder="1234 5678 9012 3456"
            />
          </div>
          <div className="mb-2">
            <label className="form-label">Expiry Date</label>
            <input
              type="datetime-local"
              className="form-control bg-black text-white border-secondary"
              value={newCard.expiryDate}
              onChange={(e) =>
                setNewCard({ ...newCard, expiryDate: e.target.value })
              }
            />
          </div>
          <div className="mb-3">
            <label className="form-label">CVV</label>
            <input
              className="form-control bg-black text-white border-secondary"
              value={newCard.cvv}
              onChange={(e) => setNewCard({ ...newCard, cvv: e.target.value })}
              placeholder="e.g. 123"
              maxLength={4}
            />
          </div>
          <button className="btn btn-info w-100" onClick={handleAddCard}>
            Add Card
          </button>
        </div>
      ) : (
        <button
          className="btn btn-outline-info w-100 mt-4"
          onClick={() => setShowForm(true)}
        >
          + Add New Card
        </button>
      )}
    </div>
  );
};

export default SelectCard;
