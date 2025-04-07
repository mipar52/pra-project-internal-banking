// src/components/BottomNav.tsx
import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import {
  FaUserFriends,
  FaChartBar,
  FaHistory,
  FaCog,
  FaBars,
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
    { icon: <FaHistory />, route: "/history", label: "Transaction History" },
    {
      icon: <FaUserFriends />,
      route: "/friends-overview",
      label: "Friends Overview",
    },
    { icon: <FaChartBar />, route: "/charts", label: "Charts" },
    { icon: <FaCog />, route: "/settings", label: "Settings" },
  ];

  if (isDesktop) {
    return (
      <div className="position-fixed top-0 start-0 p-3 bg-light shadow">
        <button
          className="btn btn-dark d-flex align-items-center gap-2 mb-2"
          onClick={() => setShowLogoutModal(true)}
        >
          <FaBars />
          <span>Logout</span>
        </button>

        <LogoutModal
          show={showLogoutModal}
          onClose={() => setShowLogoutModal(false)}
        />
      </div>
    );
  }

  return (
    <>
      <div className="bottom-nav d-flex justify-content-around align-items-center p-2">
        {/* Left icons */}
        <button
          className={`nav-btn ${
            location.pathname === "/transcations" ? "active" : ""
          }`}
          onClick={() => navigate("/transactions")}
        >
          <FaHistory />
        </button>

        <button
          className={`nav-btn ${
            location.pathname === "/all-friends" ? "active" : ""
          }`}
          onClick={() => navigate("/all-friends")}
        >
          <FaUserFriends />
        </button>

        {/* Center Logo: navigates to dashboard */}
        <div
          className="nav-logo"
          onClick={() => navigate("/dashboard")}
          style={{ cursor: "pointer" }}
        >
          <img src={algebraLogo} alt="Algebra Logo" />
        </div>

        {/* Right icons */}
        <button
          className={`nav-btn ${
            location.pathname === "/history" ? "active" : ""
          }`}
          onClick={() => navigate("/history")}
        >
          <FaChartBar />
        </button>

        <button
          className={`nav-btn ${
            location.pathname === "/settings" ? "active" : ""
          }`}
          onClick={() => navigate("/settings")}
        >
          <FaCog />
        </button>
      </div>

      {/* 🔐 Logout Modal on mobile too */}
      <LogoutModal
        show={showLogoutModal}
        onClose={() => setShowLogoutModal(false)}
      />
    </>
  );
};

export default BottomNav;
