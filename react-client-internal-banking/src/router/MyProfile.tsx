import React, { useEffect, useState } from "react";
import algebraLogo from "../assets/algebra-logo.png";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import QRCode from "react-qr-code";
import LoadingBar from "../components/LoadingBar";
import ErrorPopup from "../components/ErrorPopup";

interface UserData {
  firstName: string;
  lastName: string;
  emailAddress: string;
  phoneNumber: string;
  studyProgramme: string;
  role: string;
  profilePictureUrl: string | null;
}

const Profile: React.FC = () => {
  const navigate = useNavigate();
  const [user, setUser] = useState<UserData | null>(null);
  const [loading, setLoading] = useState(true);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  useEffect(() => {
    const fetchUser = async () => {
      const email = localStorage.getItem("userEmail");
      const token = localStorage.getItem("jwtToken");

      if (!email || !token) {
        setErrorMsg("User not logged in.");
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
        setErrorMsg("Failed to load user data.");
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, []);

  if (loading) return <LoadingBar message="Loading profile..." />;
  if (errorMsg || !user)
    return (
      <div className="text-white p-4">
        <ErrorPopup
          message={errorMsg || "Unknown error."}
          onClose={() => navigate("/")}
        />
      </div>
    );

  const qrData = `AlgebraQR${JSON.stringify({
    type: "ALGEBRAQR",
    friendEmail: user.emailAddress,
  })}`;

  return (
    <div
      className="min-vh-100 bg-black text-white d-flex flex-column align-items-center px-3 py-5"
      style={{ backgroundColor: "#1D1D1B" }}
    >
      <img
        src={algebraLogo}
        alt="Algebra Logo"
        className="mb-4"
        style={{ height: 60 }}
      />
      <button
        className="btn btn-outline-light align-self-start mb-3"
        onClick={() => navigate(-1)}
        style={{ borderRadius: "999px", fontSize: "0.9rem" }}
      >
        ← Back
      </button>

      <div
        className="bg-dark text-white rounded shadow-lg p-4 w-100"
        style={{ maxWidth: "540px" }}
      >
        {/* Profile Picture */}
        <div className="text-center mb-4">
          <img
            src={
              user.profilePictureUrl ||
              `https://i.pravatar.cc/150?u=${user.emailAddress}`
            }
            alt="Profile"
            className="rounded-circle"
            style={{
              width: 110,
              height: 110,
              objectFit: "cover",
              border: "3px solid #F58220",
            }}
          />
          <h4 className="mt-3 fw-bold">
            {user.firstName} {user.lastName}
          </h4>
          <p className="text-muted mb-0">{user.role}</p>
        </div>

        {/* User Info Fields */}
        <div className="row g-3 mb-4">
          <div className="col-md-6">
            <label className="form-label text-info mb-1">First Name</label>
            <div
              className="form-control bg-black text-white border-secondary"
              readOnly
            >
              {user.firstName}
            </div>
          </div>
          <div className="col-md-6">
            <label className="form-label text-info mb-1">Last Name</label>
            <div
              className="form-control bg-black text-white border-secondary"
              readOnly
            >
              {user.lastName}
            </div>
          </div>
          <div className="col-md-12">
            <label className="form-label text-info mb-1">Email</label>
            <div
              className="form-control bg-black text-white border-secondary"
              readOnly
            >
              {user.emailAddress}
            </div>
          </div>
          <div className="col-md-6">
            <label className="form-label text-info mb-1">Phone Number</label>
            <div
              className="form-control bg-black text-white border-secondary"
              readOnly
            >
              {user.phoneNumber}
            </div>
          </div>
          <div className="col-md-6">
            <label className="form-label text-info mb-1">Study Programme</label>
            <div
              className="form-control bg-black text-white border-secondary"
              readOnly
            >
              {user.studyProgramme}
            </div>
          </div>
          <div className="col-md-12">
            <label className="form-label text-info mb-1">Role</label>
            <div
              className="form-control bg-black text-white border-secondary"
              readOnly
            >
              {user.role}
            </div>
          </div>
        </div>

        {/* QR Code Section */}
        <div className="text-center mt-4">
          <h5 className="text-info mb-3">Your QR Code</h5>
          <div className="bg-white p-3 rounded d-inline-block border border-info shadow-sm">
            <QRCode
              value={qrData}
              size={200}
              fgColor="#000000"
              bgColor="#FFFFFF"
            />
          </div>
          <p className="mt-3 text-secondary" style={{ fontSize: "0.9rem" }}>
            Share this code to let others add you as a friend.
          </p>
        </div>
      </div>
    </div>
  );
};

export default Profile;
