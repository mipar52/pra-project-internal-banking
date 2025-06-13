import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "../css/FriendDetails.css";
import axiosInstance from "../axiosHelper/axiosInstance";
import SuccessPopup from "../components/SuccessPopup";
import ErrorPopup from "../components/ErrorPopup";
import LoadingBar from "../components/LoadingBar";
import DeleteFriendModal from "../components/DeleteFriendModal"; // Custom modal

interface FriendData {
  firstName: string;
  lastName: string;
  emailAddress: string;
  phoneNumber: string;
  studyProgramme: string;
  role: string;
  profilePictureUrl: string | null;
}

const FriendDetails: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { emailAddress } = location.state || {};

  const [friend, setFriend] = useState<FriendData | null>(null);
  const [loading, setLoading] = useState(true);
  const [showLoading, setShowLoading] = useState(false);
  const [success, setSuccess] = useState("");
  const [errorMsg, setErrorMsg] = useState("");
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    const fetchFriend = async () => {
      const token = localStorage.getItem("jwtToken");

      if (!emailAddress || !token) {
        setErrorMsg("Invalid navigation or user session.");
        setLoading(false);
        return;
      }

      try {
        const response = await axiosInstance.get(
          `http://localhost:5026/api/User/GetUserByMail?email=${encodeURIComponent(
            emailAddress
          )}`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        setFriend(response.data);
      } catch (err) {
        console.error("Failed to fetch friend details:", err);
        setErrorMsg("Could not load friend details.");
      } finally {
        setLoading(false);
      }
    };

    fetchFriend();
  }, [emailAddress]);

  const handleConfirmDelete = async () => {
    const userEmail = localStorage.getItem("userEmail");
    const token = localStorage.getItem("jwtToken");
    if (!userEmail || !emailAddress || !token) {
      setErrorMsg("Invalid session.");
      return;
    }

    setShowLoading(true);
    setShowModal(false);

    try {
      await axiosInstance.delete(
        "http://localhost:5026/api/Friend/DeleteFriendByMail",
        {
          headers: { Authorization: `Bearer ${token}` },
          data: { userEmail, friendEmail: emailAddress },
        }
      );

      setSuccess("Friend deleted successfully.");
      setTimeout(() => navigate("/all-friends"), 2000);
    } catch (err) {
      console.error("Delete error:", err);
      setErrorMsg("Failed to delete friend.");
    } finally {
      setShowLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="container text-white text-center mt-5">
        Loading friend details...
      </div>
    );
  }

  if (errorMsg && !friend) {
    return (
      <div className="container text-center text-white mt-5">
        <h4>{errorMsg}</h4>
        <button
          className="btn btn-secondary mt-3"
          onClick={() => navigate("/all-friends")}
        >
          Go back
        </button>
      </div>
    );
  }

  return (
    <div className="friend-details-screen text-white bg-black min-vh-100 d-flex flex-column align-items-center justify-content-start pt-5 px-3">
      {showLoading && <LoadingBar message="Removing friend..." />}
      {success && (
        <SuccessPopup message={success} onClose={() => setSuccess("")} />
      )}
      {errorMsg && (
        <ErrorPopup message={errorMsg} onClose={() => setErrorMsg("")} />
      )}

      <DeleteFriendModal
        show={showModal}
        onClose={() => setShowModal(false)}
        onConfirm={handleConfirmDelete}
        friendName={`${friend?.firstName} ${friend?.lastName}`}
      />

      <img
        src={
          friend?.profilePictureUrl ||
          `https://i.pravatar.cc/150?u=${friend?.emailAddress}`
        }
        alt={`${friend?.firstName} ${friend?.lastName}`}
        className="rounded-circle mb-4"
        style={{ width: 120, height: 120, objectFit: "cover" }}
      />
      <h2 className="text-info fw-bold text-center mb-2">
        {friend?.firstName} {friend?.lastName}
      </h2>
      <p className="text-light fs-6 mb-1">📧 {friend?.emailAddress}</p>
      <p className="text-light fs-6 mb-1">📞 {friend?.phoneNumber}</p>
      <p className="text-light fs-6 mb-1">🎓 {friend?.studyProgramme}</p>
      <p className="text-light fs-6 mb-4">🧑‍💼 {friend?.role}</p>

      <div className="d-flex flex-column gap-3 mt-4 w-100 px-3">
        <button
          className="btn btn-success"
          onClick={() =>
            navigate("/request-money", {
              state: { friendEmail: friend?.emailAddress },
            })
          }
        >
          Request money
        </button>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate("/payment-options", {
              state: { friendEmail: friend?.emailAddress },
            })
          }
        >
          Send money
        </button>
        <button className="btn btn-danger" onClick={() => setShowModal(true)}>
          ❌ Delete Friend
        </button>
      </div>
    </div>
  );
};

export default FriendDetails;
