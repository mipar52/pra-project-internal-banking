import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "../css/Dashboard.css";
import LogoutModal from "../components/LogoutModal";

const Dashboard: React.FC = () => {
  const navigate = useNavigate();
  const [balance, setBalance] = useState<string>("0.23");
  const [showLogoutModal, setShowLogoutModal] = useState(false);
  const [activeFilter, setActiveFilter] = useState("all");

  useEffect(() => {
    const stored = localStorage.getItem("balance");
    if (stored) setBalance(stored);
  }, []);

  const sections = {
    friends: (
      <div
        className="card mb-3 p-3 border"
        style={{
          backgroundColor: "#A00062",
          color: "#FFFFFF",
          borderColor: "#F58220",
        }}
      >
        <h5>💸 Friends</h5>
        <p>Send or receive money with classmates</p>
        <div className="d-flex gap-2">
          <button className="btn btn-success flex-fill">Request</button>
          <button
            className="btn btn-success flex-fill"
            onClick={() =>
              navigate("/payment-options", { state: { title: "Send Money" } })
            }
          >
            Send
          </button>
        </div>
      </div>
    ),
    food: (
      <div
        className="card mb-3 p-3 border"
        style={{
          backgroundColor: "#F58220",
          color: "#FFFFFF",
          borderColor: "#A00062",
        }}
      >
        <h5>🍽️ Food</h5>
        <div className="d-grid gap-2">
          <button className="btn btn-light">Pay Cafeteria Food</button>
          <button className="btn btn-dark">Pay Vending Machine</button>
          <button className="btn btn-secondary">Pay Coffee Machine</button>
        </div>
      </div>
    ),
    parking: (
      <div
        className="card mb-3 p-3 border"
        style={{
          backgroundColor: "#00AEEF",
          color: "#FFFFFF",
          borderColor: "#FFFFFF",
        }}
      >
        <h5>🅿️ Parking</h5>
        <p>Pay for parking at Algebra University</p>
        <button className="btn btn-light">Pay Parking</button>
      </div>
    ),
    scanpay: (
      <div
        className="card mb-3 p-3 border"
        style={{
          backgroundColor: "#1D1D1B",
          color: "#FFFFFF",
          borderColor: "#00AEEF",
        }}
      >
        <h5>📱 Scan and Pay</h5>
        <div className="d-grid gap-2">
          <button className="btn btn-info">Scan a Colleague's QR</button>
          <button className="btn btn-secondary">Pay Someone's Coffee</button>
          <button className="btn btn-danger">Pay Tuition</button>
        </div>
      </div>
    ),
    tuition: (
      <div
        className="card mb-3 p-3 border"
        style={{
          backgroundColor: "#A00062",
          color: "#FFFFFF",
          borderColor: "#F58220",
        }}
      >
        <h5>🎓 Tuition</h5>
        <p>Handle your tuition payments securely</p>
        <button className="btn btn-light">Pay Tuition</button>
      </div>
    ),
  };

  const visibleSections = () => {
    if (activeFilter === "all") return Object.values(sections);
    if (sections[activeFilter as keyof typeof sections]) {
      return [sections[activeFilter as keyof typeof sections]];
    }
    return [];
  };

  return (
    <div
      className="dashboard-wrapper d-flex flex-column min-vh-100"
      style={{ backgroundColor: "#1D1D1B", color: "#FFFFFF" }}
    >
      {/* Sticky TOP UP bar */}
      <div
        className="sticky-top py-3 px-4 d-flex justify-content-between align-items-center shadow"
        style={{ backgroundColor: "#1D1D1B" }}
      >
        <div>
          <h6 className="mb-0">ALGEBRA BANKICA</h6>
          <h4 className="mb-0">€{balance}</h4>
        </div>
        <button
          className="btn"
          style={{ backgroundColor: "#00AEEF", color: "#FFFFFF" }}
          onClick={() => navigate("/topup")}
        >
          TOP UP
        </button>
      </div>

      {/* Filter Tabs */}
      <div
        className="px-3 py-2 d-flex gap-2 overflow-auto sticky-top"
        style={{ top: "84px", zIndex: 10, backgroundColor: "#1D1D1B" }}
      >
        {[
          { label: "All", key: "all" },
          { label: "Friends", key: "friends" },
          { label: "Tuition", key: "tuition" },
          { label: "Food", key: "food" },
          { label: "Parking", key: "parking" },
          { label: "Scan & Pay", key: "scanpay" },
        ].map((tab) => (
          <button
            key={tab.key}
            className={`btn btn-sm filter-tab-btn ${
              activeFilter === tab.key ? "btn-warning" : "btn-secondary"
            }`}
            onClick={() => setActiveFilter(tab.key)}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {/* Scrollable Dashboard */}
      <div className="dashboard-scroll px-3 pt-3 flex-grow-1 overflow-auto">
        {visibleSections()}
        <div style={{ height: "100px" }} />
      </div>

      {/* Logout modal */}
      <LogoutModal
        show={showLogoutModal}
        onClose={() => setShowLogoutModal(false)}
      />
    </div>
  );
};

export default Dashboard;
