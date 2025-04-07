import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "../css/FriendDetails.css";

const FriendDetails: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const friend = location.state;

  if (!friend) {
    return (
      <div className="container text-center text-white mt-5">
        <h4>Friend not found</h4>
        <button
          className="btn btn-secondary mt-3"
          onClick={() => navigate("/payment-options")}
        >
          Go back
        </button>
      </div>
    );
  }

  return (
    <div className="friend-details-screen text-white bg-black min-vh-100 d-flex flex-column align-items-center justify-content-start pt-5 px-3">
      <img
        src={friend.image}
        alt={friend.username}
        className="rounded-circle mb-4"
        style={{ width: 100, height: 100, objectFit: "cover" }}
      />
      <h3>{friend.username}</h3>
      <p className="text-muted">
        You've been friends since {friend.since || "2024-01-01"}
      </p>

      <div className="d-flex gap-3 mt-4">
        <button
          className="btn btn-success"
          onClick={() =>
            navigate("/request-money", { state: { friend: friend.name } })
          }
        >
          Request money
        </button>
        <button
          className="btn btn-primary"
          onClick={() => navigate("/payment-options")}
        >
          Send money
        </button>
      </div>
    </div>
  );
};

export default FriendDetails;
