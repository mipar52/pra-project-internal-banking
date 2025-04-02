import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "../css/Dashboard.css";
import LogoutModal from "../components/LogoutModal";

const Dashboard: React.FC = () => {
  const navigate = useNavigate();
  const [balance, setBalance] = useState<string>("0.23");
  const [showLogoutModal, setShowLogoutModal] = useState(false);

  useEffect(() => {
    const stored = localStorage.getItem("balance");
    if (stored) setBalance(stored);
  }, []);

  return (
    <div className="dashboard-wrapper bg-black text-white">
      {/* Sticky Top */}
      <div className="dashboard-header sticky-top bg-dark text-white p-3 d-flex justify-content-between align-items-center shadow">
        <div>
          <h6 className="mb-1">ALGEBRA BANKICA</h6>
          <h5>€{balance}</h5>
        </div>
        <button className="btn btn-primary" onClick={() => navigate("/topup")}>
          TOP UP
        </button>
      </div>

      {/* Scrollable Content */}
      <div className="dashboard-scroll px-3 pt-3 pb-5">
        {/* Tabs */}
        <div className="d-flex gap-2 mb-3 overflow-auto">
          <button className="btn btn-warning">⭐ Favorites</button>
          <button className="btn btn-secondary">All</button>
          <button className="btn btn-dark">Friends</button>
          <button className="btn btn-dark">Buy</button>
          <button className="btn btn-dark">Car</button>
        </div>

        {/* Grid of Actions */}
        <div className="row g-3">
          <div className="col-6">
            <div className="card bg-dark text-white h-100 p-3">
              <h5>Pay the tuition fee</h5>
              <p>Poseri se u gace</p>
            </div>
          </div>
          <div className="col-6">
            <div className="card bg-dark text-white h-100 p-3">
              <h5>Parking</h5>
              <p>Pay for parking in our facility</p>
            </div>
          </div>
          <div className="col-12">
            <div className="card bg-dark text-white h-100 p-3">
              <h5>Send or Request Money</h5>
              <p>Request or send money easily and fee-free</p>
              <div className="d-flex gap-2 mt-2">
                <button className="btn btn-success flex-fill">Request</button>
                <button className="btn btn-primary flex-fill">Send</button>
              </div>
            </div>
          </div>
          <div className="col-12">
            <div className="card bg-dark text-white h-100 p-3">
              <h5>Scan and Pay</h5>
              <p>Scan QR codes to pay or handle bills</p>
              <div className="d-flex gap-2 mt-2">
                <button className="btn btn-secondary flex-fill">
                  Payment Slip
                </button>
                <button
                  className="btn btn-secondary flex-fill"
                  onClick={() => navigate("/qr")}
                >
                  QR Code
                </button>
              </div>
            </div>
          </div>
          <div className="col-6">
            <div className="card bg-dark text-white h-100 p-3">
              <h5>Food</h5>
            </div>
          </div>
          <div className="col-6">
            <div className="card bg-dark text-white h-100 p-3">
              <h5>Credit</h5>
            </div>
          </div>
        </div>
      </div>

      <LogoutModal
        show={showLogoutModal}
        onClose={() => setShowLogoutModal(false)}
      />
    </div>
  );
};

export default Dashboard;
