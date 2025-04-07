// src/components/BottomNav.tsx
import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import {
  FaUserFriends,
  FaChartBar,
  FaHistory,
  FaCog,
  FaSignOutAlt,
  FaTachometerAlt,
} from "react-icons/fa";
import useWindowSize from "../hooks/useWindowSize";
import "../css/BottomNav.css";
import algebraLogo from "../assets/algebra-logo.png";
import LogoutModal from "./LogoutModal";

const BottomNav: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { width } = useWindowSize();
  const [showLogoutModal, setShowLogoutModal] = useState(false);

  const isDesktop = width >= 768;

  const tabs = [
    { icon: <FaHistory />, route: "/transactions", label: "Transactions" },
    { icon: <FaUserFriends />, route: "/all-friends", label: "Friends" },
    { icon: <FaTachometerAlt />, route: "/dashboard", label: "Dashboard", isLogo: true },
    { icon: <FaChartBar />, route: "/history", label: "Graph" },
    { icon: <FaCog />, route: "/settings", label: "Settings" },
  ];

  if (isDesktop) {
    return (
      <>
        <div className="desktop-nav position-fixed top-0 start-0 h-100 bg-dark text-white d-flex flex-column align-items-start p-3" style={{ width: '220px', zIndex: 1050 }}>
          <div className="nav-logo-big mb-4 w-100 text-center" onClick={() => navigate("/dashboard")}> 
            <img src={algebraLogo} alt="Algebra Logo" style={{ width: 80 }} />
          </div>

          {tabs.map((tab, idx) => (
            <button
              key={idx}
              className={`btn text-start w-100 mb-2 text-white ${location.pathname === tab.route ? "btn-primary" : "btn-outline-light"}`}
              onClick={() => navigate(tab.route)}
            >
              {tab.icon} <span className="ms-2">{tab.label}</span>
            </button>
          ))}

          <button className="btn btn-danger mt-auto w-100" onClick={() => setShowLogoutModal(true)}>
            <FaSignOutAlt className="me-2" /> Logout
          </button>
        </div>

        <LogoutModal
          show={showLogoutModal}
          onClose={() => setShowLogoutModal(false)}
        />
      </>
    );
  }

  return (
    <>
      <div className="bottom-nav d-flex justify-content-around align-items-center p-2">
        <button
          className={`nav-btn ${location.pathname === "/transactions" ? "active" : ""}`}
          onClick={() => navigate("/transactions")}
        >
          <FaHistory />
        </button>

        <button
          className={`nav-btn ${location.pathname === "/all-friends" ? "active" : ""}`}
          onClick={() => navigate("/all-friends")}
        >
          <FaUserFriends />
        </button>

        <div
          className="nav-logo"
          onClick={() => navigate("/dashboard")}
          style={{ cursor: "pointer" }}
        >
          <img src={algebraLogo} alt="Algebra Logo" />
        </div>

        <button
          className={`nav-btn ${location.pathname === "/history" ? "active" : ""}`}
          onClick={() => navigate("/history")}
        >
          <FaChartBar />
        </button>

        <button
          className={`nav-btn ${location.pathname === "/settings" ? "active" : ""}`}
          onClick={() => navigate("/settings")}
        >
          <FaCog />
        </button>
      </div>

      <LogoutModal
        show={showLogoutModal}
        onClose={() => setShowLogoutModal(false)}
      />
    </>
  );
};

export default BottomNav;
