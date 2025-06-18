import React, { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import algebraLogo from "../assets/algebra-logo.png";
import "../css/Friends.css";

interface Transaction {
  typeName: string;
  amount: number;
  date: string;
  transactionTypeId: number;
}

const Transactions: React.FC = () => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState("");
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [loading, setLoading] = useState(true);
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const fetchTransactions = async () => {
      const email = localStorage.getItem("userEmail");
      const token = localStorage.getItem("jwtToken");

      if (!email || !token) return;

      try {
        const response = await axios.get(
          `http://localhost:5026/api/Transaction/GetTransactionsByEmail/${encodeURIComponent(
            email
          )}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setTransactions(response.data);
      } catch (err) {
        console.error("Failed to load transactions", err);
      } finally {
        setLoading(false);
      }
    };

    fetchTransactions();
  }, []);

  const filteredData = transactions.filter((item) => {
    const lower = searchTerm.toLowerCase();
    return item.typeName.toLowerCase().includes(lower);
  });

  const formatDate = (isoDate: string): string => {
    const date = new Date(isoDate);
    const today = new Date();

    const isToday =
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear();

    if (isToday) {
      return date.toLocaleTimeString("hr-HR", {
        hour: "2-digit",
        minute: "2-digit",
      });
    }

    return date.toLocaleDateString("hr-HR", {
      day: "2-digit",
      month: "2-digit",
    });
  };

  const getInitials = (label: string) => {
    return label
      .split(" ")
      .map((word) => word[0])
      .join("")
      .toUpperCase();
  };

  const getColor = (label: string) => {
    const colors = ["#A066FF", "#FF784A", "#4AC6FF", "#47D764", "#FFCC00"];
    let hash = 0;
    for (let i = 0; i < label.length; i++) {
      hash = label.charCodeAt(i) + ((hash << 5) - hash);
    }
    return colors[Math.abs(hash) % colors.length];
  };

  useEffect(() => {
    const handleKey = (e: KeyboardEvent) => {
      if (e.key === "/" && inputRef.current) {
        e.preventDefault();
        inputRef.current.focus();
      }
    };
    window.addEventListener("keydown", handleKey);
    return () => window.removeEventListener("keydown", handleKey);
  }, []);

  return (
    <div className="friends-screen">
      {/* Top bar */}
      <div className="top-bar">
        <img src={algebraLogo} alt="Algebra" className="algebra-logo" />
      </div>

      {/* Static header + search bar */}
      <div className="friends-header px-3 py-2">
        <h4 className="fw-bold mb-3">Daj te pare</h4>
        <div className="position-relative">
          <input
            ref={inputRef}
            type="text"
            className="form-control bg-dark text-white border-secondary pe-5"
            placeholder="Search by type..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          {searchTerm && (
            <button
              className="btn btn-sm btn-outline-light position-absolute top-50 end-0 translate-middle-y me-3"
              onClick={() => setSearchTerm("")}
            >
              ✕
            </button>
          )}
        </div>
      </div>

      {/* Transactions list */}
      <div className="friend-list-scroll flex-grow-1 overflow-auto">
        {loading ? (
          <div className="text-center text-muted pt-4">Loading...</div>
        ) : filteredData.length === 0 ? (
          <div className="text-center text-muted pt-4">No results found.</div>
        ) : (
          filteredData.map((item, idx) => (
            <div
              className="friend-item d-flex justify-content-between align-items-center px-3 py-2"
              key={idx}
              onClick={() => navigate("/transaction-details", { state: item })}
              style={{ cursor: "pointer" }}
            >
              <div className="d-flex align-items-center gap-3">
                <div
                  className="avatar d-flex align-items-center justify-content-center"
                  style={{
                    backgroundColor: getColor(item.typeName),
                  }}
                >
                  {getInitials(item.typeName)}
                </div>
                <div>
                  <div className="fw-bold">{item.typeName}</div>
                  <div className="small text-warning fw-semibold">
                    {item.transactionTypeId === 0 ? "Outgoing" : "Incoming"}
                  </div>
                </div>
              </div>
              <div className="text-end">
                <div className="fw-bold text-success fs-5">{item.amount} €</div>
                <div className="small text-secondary opacity-75">
                  {formatDate(item.date)}
                </div>
              </div>
            </div>
          ))
        )}
        <div style={{ height: "100px" }} />
      </div>
    </div>
  );
};

export default Transactions;
