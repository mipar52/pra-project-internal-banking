import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import useWindowSize from "../hooks/useWindowSize";
import "../css/Dashboard.css";
import LogoutModal from "../components/LogoutModal";

const Dashboard: React.FC = () => {
  const navigate = useNavigate();
  const { width } = useWindowSize();
  const isDesktop = width >= 768;

  const [balance, setBalance] = useState<string>("0.23");
  const [showLogoutModal, setShowLogoutModal] = useState(false);
  const [activeFilter, setActiveFilter] = useState("all");

  useEffect(() => {
    const stored = localStorage.getItem("balance");
    if (stored) setBalance(stored);
  }, []);

  const sections = {
    friends: (
      <div className="dashboard-section">
        <div
          className="card mb-3 p-3 border"
          style={{ backgroundColor: "#A00062", color: "#FFFFFF", borderColor: "#F58220" }}
        >
          <h5>💸 Friends</h5>
          <p>Send or receive money with classmates</p>
          <div className="d-flex gap-2">
            <button className="btn btn-success flex-fill" onClick={() => navigate("/request-money")}>Request</button>
            <button
              className="btn btn-success flex-fill"
              onClick={() => navigate("/payment-options", { state: { title: "Send Money" } })}
            >
              Send
            </button>
          </div>
        </div>
      </div>
    ),
    food: (
      <div className="dashboard-section">
        <div
          className="card mb-3 p-3 border"
          style={{ backgroundColor: "#F58220", color: "#FFFFFF", borderColor: "#A00062" }}
        >
          <h5>🍽️ Food</h5>
          <div className="d-grid gap-2">
            <button className="btn btn-light" onClick={() => navigate("/payment-options")}>Pay Cafeteria Food</button>
            <button className="btn btn-dark" onClick={() => navigate("/payment-options")}>Pay Vending Machine</button>
            <button className="btn btn-secondary" onClick={() => navigate("/payment-options")}>Pay Coffee Machine</button>
          </div>
        </div>
      </div>
    ),
    parking: (
      <div className="dashboard-section">
        <div
          className="card mb-3 p-3 border"
          style={{ backgroundColor: "#00AEEF", color: "#FFFFFF", borderColor: "#FFFFFF" }}
        >
          <h5>🅿️ Parking</h5>
          <p>Pay for parking at Algebra University</p>
          <button className="btn btn-light" onClick={() => navigate("/payment-options")}>Pay Parking</button>
        </div>
      </div>
    ),
    scanpay: (
      <div className="dashboard-section">
        <div
          className="card mb-3 p-3 border"
          style={{ backgroundColor: "#1D1D1B", color: "#FFFFFF", borderColor: "#00AEEF" }}
        >
          <h5>📱 Scan and Pay</h5>
          <div className="d-grid gap-2">
            <button className="btn btn-info" onClick={() => navigate("/qr")}>Scan a Colleague's QR</button>
            <button className="btn btn-secondary" onClick={() => navigate("/payment-options")}>Pay Someone's Coffee</button>
            <button className="btn btn-danger" onClick={() => navigate("/payment-options")}>Pay Tuition</button>
          </div>
        </div>
      </div>
    ),
    tuition: (
      <div className="dashboard-section">
        <div
          className="card mb-3 p-3 border"
          style={{ backgroundColor: "#A00062", color: "#FFFFFF", borderColor: "#F58220" }}
        >
          <h5>🎓 Tuition</h5>
          <p>Handle your tuition payments securely</p>
          <button className="btn btn-light" onClick={() => navigate("/payment-options")}>Pay Tuition</button>
        </div>
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
      {/* Header */}
      <div className="sticky-top py-3 px-4 d-flex justify-content-between align-items-center shadow" style={{ backgroundColor: "#1D1D1B" }}>
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
        className={`py-3 px-3 sticky-top ${isDesktop ? "justify-content-center d-flex" : "overflow-auto d-flex gap-2"}`}
        style={{ top: "84px", zIndex: 10, backgroundColor: "#1D1D1B" }}
      >
        {["All", "Friends", "Tuition", "Food", "Parking", "Scan & Pay"].map((label) => {
          const key = label.toLowerCase().replace(/ & | /g, "");
          return (
            <button
              key={key}
              className={`btn btn-sm mx-1 filter-tab-btn ${activeFilter === key ? "btn-warning" : "btn-secondary"}`}
              onClick={() => setActiveFilter(key)}
            >
              {label}
            </button>
          );
        })}
      </div>

      {/* Content */}
      <div className={`dashboard-scroll ${isDesktop ? "px-5 pt-4" : "px-3 pt-3"} flex-grow-1 overflow-auto`}>
        <div className="card bg-dark text-white mb-3 p-3 border border-info">
          <h5 className="text-info">🎫 My QR Code</h5>
          <p>Show your QR code so others can pay you directly or add you as a friend.</p>
          <button className="btn btn-info" onClick={() => navigate("/user-qr")}>Show QR Code</button>
        </div>

        <div className={isDesktop ? "row" : ""}>
          {visibleSections().map((section, idx) => (
            <div key={idx} className={isDesktop ? "col-md-6 col-lg-4" : ""}>
              {section}
            </div>
          ))}
        </div>

        <div style={{ height: "100px" }} />
      </div>

      <LogoutModal show={showLogoutModal} onClose={() => setShowLogoutModal(false)} />
    </div>
  );
};

export default Dashboard;
