// src/components/BottomNav.tsx
import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import {
  FaUserFriends,
  FaHome,
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
    { icon: <FaUserFriends />, route: "/friends", label: "Friends" },
    { icon: <FaHome />, route: "/dashboard", label: "Dashboard" },
    { icon: <FaHistory />, route: "/history", label: "History" },
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
        {tabs.slice(0, 2).map((tab) => (
          <button
            key={tab.route}
            className={`nav-btn ${
              location.pathname === tab.route ? "active" : ""
            }`}
            onClick={() => navigate(tab.route)}
          >
            {tab.icon}
          </button>
        ))}

        {/* Logo = Logout trigger */}
        <div
          className="nav-logo"
          onClick={() => setShowLogoutModal(true)}
          style={{ cursor: "pointer" }}
        >
          <img src={algebraLogo} alt="Algebra Logo" />
        </div>

        {tabs.slice(2).map((tab) => (
          <button
            key={tab.route}
            className={`nav-btn ${
              location.pathname === tab.route ? "active" : ""
            }`}
            onClick={() => navigate(tab.route)}
          >
            {tab.icon}
          </button>
        ))}
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
