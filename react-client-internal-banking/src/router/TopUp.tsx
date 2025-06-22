import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import SuccessPopup from "../components/SuccessPopup";
import ErrorPopup from "../components/ErrorPopup";
import LoadingBar from "../components/LoadingBar";

const TopUp: React.FC = () => {
  const navigate = useNavigate();
  const [amount, setAmount] = useState<number>(0);
  const [isLoading, setIsLoading] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);
  const [cards, setCards] = useState<any[]>([]);
  const [selectedCardIndex, setSelectedCardIndex] = useState(0);
  const [showCardList, setShowCardList] = useState(false);

  const balance = parseFloat(localStorage.getItem("balance") || "0.00");

  const increase = () => setAmount((prev) => prev + 1);
  const decrease = () => setAmount((prev) => (prev > 0 ? prev - 1 : 0));

  const getCardLogo = (number: string | undefined) => {
    if (!number)
      return "https://download.logo.wine/logo/Mastercard/Mastercard-Logo.wine.png";
    const bin = number[0];
    if (bin === "4") return "https://cdn.worldvectorlogo.com/logos/visa-2.svg";
    if (bin === "5")
      return "https://download.logo.wine/logo/Mastercard/Mastercard-Logo.wine.png";
    return "https://download.logo.wine/logo/Mastercard/Mastercard-Logo.wine.png";
  };

  const maskCardNumber = (num: string | undefined): string => {
    if (!num || num.length < 8) return "•••• •••• •••• ••••";

    const first4 = num.slice(0, 4);
    const last4 = num.slice(-4);
    return `${first4} •••• •••• ${last4}`;
  };

  useEffect(() => {
    const fetchCards = async () => {
      const userEmail = localStorage.getItem("userEmail");
      const token = localStorage.getItem("jwtToken");

      if (!userEmail || !token) return;

      setIsLoading(true);
      try {
        const response = await axios.get(
          `http://localhost:5026/api/CreditCard/GetCardsByMail/${encodeURIComponent(
            userEmail
          )}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        setCards(response.data || []);
      } catch (err) {
        console.error("Failed to fetch cards:", err);
      } finally {
        setIsLoading(false);
      }
    };

    fetchCards();
  }, []);

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

    if (!cards[selectedCardIndex]) {
      setErrorMsg("No card selected.");
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
            amount: amount,
            date: new Date().toISOString(),
            transactionTypeId: 5,
          }),
        }
      );

      if (!response.ok) {
        const err = await response.json();
        throw new Error(err?.message || "Top-up failed.");
      }

      const newBalance = balance + amount;
      localStorage.setItem("balance", newBalance.toFixed(2));
      setShowSuccess(true);
    } catch (err: any) {
      setErrorMsg(err.message || "An error occurred while topping up.");
    } finally {
      setIsLoading(false);
    }
  };

  const selectedCard = cards[selectedCardIndex];

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
        <h5 className="mb-0">Add balance to account</h5>
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
          Available: <strong>{balance.toLocaleString("en-US")} €</strong>
        </p>
      </div>

      {/* Card Selector */}
      <div className="bg-dark p-3" style={{ marginBottom: "80px" }}>
        {selectedCard && (
          <div
            className="d-flex align-items-center justify-content-between bg-black text-white p-3 rounded mb-3"
            onClick={() => setShowCardList((prev) => !prev)}
            style={{ cursor: "pointer" }}
          >
            <img
              src={getCardLogo(selectedCard.creditCardNumber)}
              alt="Logo"
              width={40}
            />
            <div className="ms-2 flex-grow-1">
              <small>Paying with:</small>
              <br />
              <strong>{maskCardNumber(selectedCard.creditCardNumber)}</strong>
              <br />
              <span className="text-secondary" style={{ fontSize: "0.8rem" }}>
                {selectedCard.firstName} {selectedCard.lastname}
              </span>
            </div>
            <span className="ms-auto">▼</span>
          </div>
        )}

        {showCardList && cards.length > 1 && (
          <div className="d-flex flex-column gap-2">
            {cards.map((card, index) => (
              <div
                key={index}
                className={`d-flex align-items-center bg-black text-white p-2 rounded`}
                style={{
                  cursor: "pointer",
                  border:
                    index === selectedCardIndex
                      ? "2px solid #00AEEF"
                      : "1px solid #444",
                }}
                onClick={() => {
                  setSelectedCardIndex(index);
                  setShowCardList(false);
                }}
              >
                <img
                  src={getCardLogo(card.creditCardNumber)}
                  alt="Logo"
                  width={40}
                  className="me-2"
                />
                <div>
                  <div>{maskCardNumber(card.creditCardNumber)}</div>
                  <small className="text-secondary">
                    {card.firstName} {card.lastname}
                  </small>
                </div>
              </div>
            ))}
          </div>
        )}

        <button
          className="btn btn-info text-white w-100 py-3 rounded-pill fw-bold mt-3"
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
