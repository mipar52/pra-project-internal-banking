import React, { useEffect, useState } from "react";
import algebraLogo from "../assets/algebra-logo.png";
import { useNavigate } from "react-router-dom";
import LogoutModal from "../components/LogoutModal";

const settings = [
  { label: "Cards Settings", route: "/select-card", icon: "💳" },
  { label: "My Profile", route: "/profile", icon: "👤" },
  { label: "Log Out", route: "/", icon: "⏻", logout: true },
];

const Settings: React.FC = () => {
  const navigate = useNavigate();
  const [showLogoutModal, setShowLogoutModal] = useState(false);
  const [balance, setBalance] = useState("0.00");

  useEffect(() => {
    const stored = localStorage.getItem("balance");
    setBalance(parseFloat(stored || "0").toFixed(2));
  }, []);

  const handleItemClick = (item: any) => {
    if (item.logout) {
      setShowLogoutModal(true);
    } else {
      navigate(item.route);
    }
  };

  const handleLogoutConfirm = () => {
    localStorage.clear();
    navigate("/");
  };

  return (
    <div className="bg-black text-white d-flex flex-column min-vh-100">
      {/* Logo and Balance */}
      <div className="text-center py-4 bg-dark">
        <img
          src={algebraLogo}
          alt="Algebra"
          style={{ height: "60px", objectFit: "contain" }}
        />
        <h5 className="mt-3">Moja ALGEBRA KARTICA</h5>
        <div className="fs-2 fw-bold">€{balance}</div>
        <div className="d-flex justify-content-center gap-2 mt-3">
          <button
            className="btn btn-primary px-4"
            onClick={() => navigate("/topup")}
          >
            NADOPLATI
          </button>
          <button
            className="btn btn-outline-light px-4"
            onClick={() => navigate("/history")}
          >
            DETALJI
          </button>
        </div>
      </div>

      {/* Settings List */}
      <div className="flex-grow-1 overflow-auto pt-3 px-3">
        {settings.map((item, idx) => (
          <div
            key={idx}
            className="d-flex justify-content-between align-items-center border-bottom border-secondary py-3"
            style={{ cursor: "pointer" }}
            onClick={() => handleItemClick(item)}
          >
            <div className="d-flex align-items-center gap-3">
              <span className="fs-5">{item.icon}</span>
              <span className="fw-semibold">{item.label}</span>
            </div>
            {item.tag && (
              <span className="badge bg-warning text-dark fw-bold">
                {item.tag}
              </span>
            )}
          </div>
        ))}
        <div style={{ height: "100px" }} />
      </div>

      {/* Logout Modal */}
      <LogoutModal
        show={showLogoutModal}
        onClose={() => setShowLogoutModal(false)}
        onConfirm={handleLogoutConfirm}
      />
    </div>
  );
};

export default Settings;
