import React, { useEffect, useState } from "react";
import QRCode from "react-qr-code";
import algebraLogo from "../assets/algebra-logo.png";
import { useNavigate } from "react-router-dom";
import axios from "axios";

interface UserData {
  firstName: string;
  lastName: string;
  emailAddress: string;
  phoneNumber: string;
  studyProgramme: string;
  role: string;
  profilePictureUrl: string | null;
}

const UserQrCode: React.FC = () => {
  const [user, setUser] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUser = async () => {
      const email = localStorage.getItem("userEmail");
      const token = localStorage.getItem("jwtToken");

      if (!email || !token) {
        setError("User not logged in.");
        setLoading(false);
        return;
      }

      try {
        const response = await axios.get(
          `http://localhost:5026/api/User/GetUserByMail?email=${encodeURIComponent(
            email
          )}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setUser(response.data);
      } catch (err) {
        console.error("Error fetching user:", err);
        setError("Failed to load user data.");
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, []);

  if (loading) return <p className="text-white">Loading...</p>;
  if (error || !user)
    return <p className="text-danger">{error || "Unknown error."}</p>;

  const qrData = JSON.stringify({
    email: user.emailAddress,
    firstName: user.firstName,
    lastName: user.lastName,
    phoneNumber: user.phoneNumber,
    studyProgramme: user.studyProgramme,
    role: user.role,
  });

  return (
    <div
      className="min-vh-100 d-flex flex-column align-items-center text-white px-4 py-5"
      style={{ backgroundColor: "#1D1D1B" }}
    >
      <img
        src={algebraLogo}
        alt="Algebra Logo"
        className="mb-4"
        style={{ height: 60 }}
      />

      <img
        src={
          user.profilePictureUrl ||
          "https://i.pravatar.cc/150?u=" + user.emailAddress
        }
        alt="User Avatar"
        className="rounded-circle mb-3"
        style={{ width: 100, height: 100, objectFit: "cover" }}
      />

      <h5 className="fw-bold">
        {user.firstName} {user.lastName}
      </h5>
      <p className="text-secondary mb-1">{user.emailAddress}</p>
      <p className="text-secondary mb-1">📞 {user.phoneNumber}</p>
      <p className="text-secondary mb-1">🎓 {user.studyProgramme}</p>
      <p className="text-secondary mb-3">🧑‍💼 {user.role}</p>

      <div className="bg-white p-3 rounded" style={{ maxWidth: 240 }}>
        <QRCode value={qrData} size={200} fgColor="#000000" bgColor="#FFFFFF" />
      </div>

      <hr className="w-100 my-5 border-secondary" />

      <div className="text-center">
        <h5 className="text-info mb-2">👫 Add a Friend</h5>
        <p className="text-secondary">
          Scan someone's QR code to send or request money.
        </p>
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
