import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "../css/FriendDetails.css";
import axios from "axios";

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
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchFriend = async () => {
      const token = localStorage.getItem("jwtToken");

      if (!emailAddress || !token) {
        setError("Invalid navigation or user session.");
        setLoading(false);
        return;
      }

      try {
        const response = await axios.get(
          `http://localhost:5026/api/User/GetUserByMail?email=${encodeURIComponent(
            emailAddress
          )}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setFriend(response.data);
      } catch (err) {
        console.error("Failed to fetch friend details:", err);
        setError("Could not load friend details.");
      } finally {
        setLoading(false);
      }
    };

    fetchFriend();
  }, [emailAddress]);

  if (loading) {
    return (
      <div className="container text-white text-center mt-5">
        Loading friend details...
      </div>
    );
  }

  if (error || !friend) {
    return (
      <div className="container text-center text-white mt-5">
        <h4>{error || "Friend not found."}</h4>
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
      <img
        src={
          friend.profilePictureUrl ||
          `https://i.pravatar.cc/150?u=${friend.emailAddress}`
        }
        alt={`${friend.firstName} ${friend.lastName}`}
        className="rounded-circle mb-4"
        style={{ width: 100, height: 100, objectFit: "cover" }}
      />
      <h3>
        {friend.firstName} {friend.lastName}
      </h3>
      <p className="text-muted mb-1">📧 {friend.emailAddress}</p>
      <p className="text-muted mb-1">📞 {friend.phoneNumber}</p>
      <p className="text-muted mb-1">🎓 {friend.studyProgramme}</p>
      <p className="text-muted mb-4">🧑‍💼 {friend.role}</p>

      <div className="d-flex gap-3 mt-4">
        <button
          className="btn btn-success"
          onClick={() =>
            navigate("/request-money", {
              state: { friendEmail: friend.emailAddress },
            })
          }
        >
          Request money
        </button>
        <button
          className="btn btn-primary"
          onClick={() =>
            navigate("/payment-options", {
              state: { friendEmail: friend.emailAddress },
            })
          }
        >
          Send money
        </button>
      </div>
    </div>
  );
};

export default FriendDetails;
