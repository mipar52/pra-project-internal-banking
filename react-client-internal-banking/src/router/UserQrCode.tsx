// src/router/UserQrCode.tsx
import React, { useState } from "react";
import QRCode from "react-qr-code";
import algebraLogo from "../assets/algebra-logo.png";
import { useNavigate } from "react-router-dom";

const mockUser = {
  username: "milica.krmpotic",
  firstName: "Milica",
  lastName: "Krmpotic",
  avatarUrl: "https://i.pravatar.cc/150?img=47",
};

const UserQrCode: React.FC = () => {
  const [showScanner, setShowScanner] = useState(false);
  const navigate = useNavigate();

  const qrData = JSON.stringify({
    username: mockUser.username,
    firstName: mockUser.firstName,
    lastName: mockUser.lastName,
  });

  return (
    <div
      className="min-vh-100 d-flex flex-column align-items-center text-white px-4 py-5"
      style={{ backgroundColor: "#1D1D1B" }}
    >
      <img src={algebraLogo} alt="Algebra Logo" className="mb-4" style={{ height: 60 }} />

      <img
        src={mockUser.avatarUrl}
        alt="User Avatar"
        className="rounded-circle mb-3"
        style={{ width: 100, height: 100, objectFit: "cover" }}
      />

      <h5 className="fw-bold">{mockUser.username}</h5>
      <p className="text-secondary">
        {mockUser.firstName} {mockUser.lastName}
      </p>

      <div className="bg-white p-3 mt-4 rounded" style={{ maxWidth: 240 }}>
        <QRCode value={qrData} size={200} fgColor="#000000" bgColor="#FFFFFF" />
      </div>

      {/* Divider */}
      <hr className="w-100 my-5 border-secondary" />

      {/* Scan QR to Add Friend */}
      <div className="text-center">
        <h5 className="text-info mb-2">👫 Add a Friend</h5>
        <p className="text-secondary">Scan someone's QR code to send or request money.</p>
        <button
          className="btn btn-outline-info"
          onClick={() => navigate("/qr")}
        >
          Scan QR Code
        </button>
      </div>
    </div>
  );
};

export default UserQrCode;
