import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";
import algebraLogo from "../assets/algebra-logo.png";
import SuccessPopup from "../components/SuccessPopup";
import ErrorPopup from "../components/ErrorPopup";
import LoadingBar from "../components/LoadingBar";
import "../css/EnterManully.css";
import axiosInstance from "../axiosHelper/axiosInstance";

const categories = [
  { label: "Food", emoji: "🍔", id: 1 },
  { label: "Parking", emoji: "🅿️", id: 2 },
  { label: "Money transfer", emoji: "📄", id: 4 },
  { label: "Tuition", emoji: "🎓", id: 3 },
];

interface Friend {
  firstName: string;
  lastName: string;
  emailAddress: string;
}

const EnterManually: React.FC = () => {
  const [amount, setAmount] = useState<string>("");
  const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
  const [friends, setFriends] = useState<Friend[]>([]);
  const [selectedFriendEmail, setSelectedFriendEmail] = useState<string>("");
  const [regNumber, setRegNumber] = useState<string>("");
  const [regCountry, setRegCountry] = useState<string>("");
  const [duration, setDuration] = useState<number>(0);

  const [showSuccess, setShowSuccess] = useState(false);
  const [showError, setShowError] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [showLoading, setShowLoading] = useState(false);

  const location = useLocation();

  useEffect(() => {
    const qrData = location.state as {
      amount?: number;
      transactionTypeId?: number;
    };

    if (qrData) {
      if (qrData.amount) {
        setAmount(qrData.amount.toString());
      }
      if (qrData.transactionTypeId) {
        setSelectedCategory(qrData.transactionTypeId);
      }
    }
  }, [location.state]);

  useEffect(() => {
    if (selectedCategory === 4) {
      const fetchFriends = async () => {
        const email = localStorage.getItem("userEmail");
        const token = localStorage.getItem("jwtToken");
        if (!email || !token) return;
        try {
          const response = await axios.get(
            `http://localhost:5026/api/Friend/GetFriendsByEmail/${encodeURIComponent(
              email
            )}`,
            {
              headers: { Authorization: `Bearer ${token}` },
            }
          );
          setFriends(response.data);
        } catch {
          setErrorMessage("Failed to load friends list.");
          setShowError(true);
        }
      };
      fetchFriends();
    }
  }, [selectedCategory]);

  const handleCategorySelect = (id: number) => {
    setSelectedCategory(id);
    setSelectedFriendEmail("");
    setRegNumber("");
    setRegCountry("");
    setDuration(0);
  };

  const handleSubmit = async () => {
    setShowLoading(true);
    const userEmail = localStorage.getItem("userEmail");
    const token = localStorage.getItem("jwtToken");

    if (!userEmail || !amount || selectedCategory === null || !token) {
      setErrorMessage("Please fill all fields.");
      setShowError(true);
      setShowLoading(false);
      return;
    }

    try {
      let endpoint = "";
      let payload: any = {};

      const commonHeaders = {
        headers: { Authorization: `Bearer ${token}` },
      };

      switch (selectedCategory) {
        case 1:
          endpoint =
            "http://localhost:5026/api/Transaction/TransactionCreateByMail";
          payload = {
            userEmail,
            transactionTypeId: 1,
            amount: parseFloat(amount),
            date: new Date().toISOString(),
          };
          break;
        case 2:
          if (!regNumber || !regCountry || duration <= 0) {
            throw new Error("Please fill in all parking details.");
          }
          endpoint =
            "http://localhost:5026/api/Transaction/ParkingTransactionCreateMail";
          const start = new Date();
          const end = new Date(start.getTime() + duration * 3600000);
          payload = {
            userEmail,
            transactionTypeId: 2,
            date: start.toISOString(),
            registrationNumber: regNumber,
            registrationCountryCode: regCountry,
            durationHours: duration,
            startTime: start.toISOString(),
            endTime: end.toISOString(),
          };
          break;
        case 3:
          endpoint =
            "http://localhost:5026/api/Transaction/ScholarshipTransactionCreateMail";
          payload = {
            userEmail,
            transactionTypeId: 3,
            amount: parseFloat(amount),
            date: new Date().toISOString(),
          };
          break;
        case 4:
          if (!selectedFriendEmail) throw new Error("Please select a friend.");
          endpoint =
            "http://localhost:5026/api/Transaction/MoneyTransferCreateMail";
          payload = {
            userRecieverEmail: selectedFriendEmail,
            userSenderEmail: userEmail,
            transactionTypeId: 4,
            amount: parseFloat(amount),
            date: new Date().toISOString(),
          };
          break;
      }
      if (selectedCategory == 4) {
        console.log(`payload: ${payload}`);
        await axiosInstance.post(endpoint, payload, commonHeaders);
      } else {
        await axiosInstance.post(endpoint, payload, commonHeaders);
      }
      setShowSuccess(true);
    } catch (err: any) {
      const message =
        err.response?.data?.message ||
        err.response?.data ||
        err.message ||
        "Something went wrong with the payment.";
      setErrorMessage(message);
      setShowError(true);
    } finally {
      setShowLoading(false);
    }
  };

  return (
    <div className="enter-manually-wrapper">
      <div className="enter-manually-logo">
        <img src={algebraLogo} alt="Algebra Logo" style={{ height: 40 }} />
      </div>

      <div className="enter-manually-form overflow-auto px-3">
        <div className="mb-4">
          <label className="form-label">Amount (€)</label>
          <input
            className="form-control"
            type="number"
            step="0.01"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            placeholder="Enter amount"
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Payment Category</label>
          <div className="d-grid gap-2">
            {categories.map((cat) => (
              <button
                key={cat.id}
                className={`btn btn-outline-light text-start ${
                  selectedCategory === cat.id ? "active bg-info" : ""
                }`}
                onClick={() => handleCategorySelect(cat.id)}
              >
                {cat.emoji} {cat.label}
              </button>
            ))}
          </div>
        </div>

        {selectedCategory === 4 && (
          <div className="mb-4">
            <label className="form-label">Select Friend</label>
            <select
              className="form-select bg-dark text-white"
              value={selectedFriendEmail}
              onChange={(e) => setSelectedFriendEmail(e.target.value)}
            >
              <option value="">Choose a friend</option>
              {friends.map((f) => (
                <option key={f.emailAddress} value={f.emailAddress}>
                  {f.firstName} {f.lastName}
                </option>
              ))}
            </select>
          </div>
        )}

        {selectedCategory === 2 && (
          <div className="mb-4">
            <label className="form-label">Registration Number</label>
            <input
              className="form-control"
              value={regNumber}
              onChange={(e) => setRegNumber(e.target.value)}
              placeholder="Enter registration number"
            />
            <label className="form-label mt-3">Registration Country Code</label>
            <input
              className="form-control"
              value={regCountry}
              onChange={(e) => setRegCountry(e.target.value)}
              placeholder="Enter country code (e.g. HR)"
            />
            <label className="form-label mt-3">Duration (hours)</label>
            <input
              type="number"
              className="form-control"
              value={duration}
              onChange={(e) => setDuration(Number(e.target.value))}
              placeholder="Enter duration in hours"
            />
          </div>
        )}

        <div style={{ height: "100px" }} />
      </div>

      {showSuccess && (
        <SuccessPopup
          message="Payment submitted successfully!"
          onClose={() => setShowSuccess(false)}
        />
      )}
      {showError && (
        <ErrorPopup
          message={errorMessage}
          onClose={() => setShowError(false)}
        />
      )}
      {showLoading && (
        <LoadingBar message="Processing your payment... Hang tight!" />
      )}

      <div className="enter-manually-button-container">
        <button className="btn" onClick={handleSubmit}>
          💸 Pay
        </button>
      </div>
    </div>
  );
};

export default EnterManually;
