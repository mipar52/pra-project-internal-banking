import React from "react";
import algebraLogo from "../assets/algebra-logo.png";
import { useNavigate } from "react-router-dom";

const settings = [
  { label: "Računi i kartice", route: "/cards", icon: "💳" },
  { label: "Moj profil", route: "/profile", icon: "👤" },
  { label: "Algebra Parking", route: "/plus", icon: "🆕", tag: "NOVO" },
  { label: "Limiti", route: "/limits", icon: "📊" },
  { label: "Postavke", route: "/preferences", icon: "⚙️" },
  { label: "Naknade", route: "/fees", icon: "💰" },
  { label: "Pomoć", route: "/help", icon: "💡" },
  { label: "Odjava", route: "/", icon: "⏻", logout: true },
];

const Settings: React.FC = () => {
  const navigate = useNavigate();

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
        <div className="fs-2 fw-bold">€0,23</div>
        <div className="d-flex justify-content-center gap-2 mt-3">
          <button className="btn btn-primary px-4">NADOPLATI</button>
          <button className="btn btn-outline-light px-4">DETALJI</button>
        </div>
      </div>

      {/* Settings List */}
      <div className="flex-grow-1 overflow-auto pt-3 px-3">
        {settings.map((item, idx) => (
          <div
            key={idx}
            className="d-flex justify-content-between align-items-center border-bottom border-secondary py-3"
            style={{ cursor: "pointer" }}
            onClick={() => {
              if (item.logout) {
                localStorage.clear();
              }
              navigate(item.route);
            }}
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
    </div>
  );
};

export default Settings;
