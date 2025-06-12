import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import algebraLogo from "../assets/algebra-logo.png";
import axios from "axios";

interface Friend {
  firstName: string;
  lastName: string;
  emailAddress: string;
  phoneNumber: string;
  profilePictureUrl: string | null;
}

const AllFriends: React.FC = () => {
  const [search, setSearch] = useState("");
  const [friends, setFriends] = useState<Friend[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchFriends = async () => {
      const email = localStorage.getItem("userEmail");
      const token = localStorage.getItem("jwtToken");

      if (!email || !token) {
        setError("Not logged in.");
        setLoading(false);
        return;
      }

      try {
        const response = await axios.get(
          `http://localhost:5026/api/Friend/GetFriendsByEmail/${encodeURIComponent(
            email
          )}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setFriends(response.data);
      } catch (err) {
        console.error("Error loading friends:", err);
        setError("Could not load friends list.");
      } finally {
        setLoading(false);
      }
    };

    fetchFriends();
  }, []);

  const filteredFriends = friends.filter((f) => {
    const fullName = `${f.firstName} ${f.lastName}`.toLowerCase();
    return fullName.includes(search.toLowerCase());
  });

  return (
    <div className="bg-black text-white min-vh-100 d-flex flex-column">
      {/* Header */}
      <div className="text-center py-3 shadow-sm bg-dark">
        <img src={algebraLogo} alt="Algebra Logo" style={{ height: 40 }} />
        <h5 className="mt-2">All Friends</h5>
      </div>

      {/* Search */}
      <div className="p-3">
        <input
          type="text"
          className="form-control bg-dark text-white border-secondary"
          placeholder="Search by name..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      {/* Friends List */}
      <div className="flex-grow-1 overflow-auto px-3">
        {loading ? (
          <div className="text-center text-muted mt-5">Loading friends...</div>
        ) : error ? (
          <div className="text-center text-danger mt-5">{error}</div>
        ) : filteredFriends.length === 0 ? (
          <div className="text-center text-muted mt-5">No friends found.</div>
        ) : (
          filteredFriends.map((friend, index) => (
            <div
              key={index}
              className="d-flex align-items-center gap-3 border-bottom border-secondary py-3"
              style={{ cursor: "pointer" }}
              onClick={() => navigate("/friend-details", { state: friend })}
            >
              <img
                src={
                  friend.profilePictureUrl ||
                  `https://i.pravatar.cc/150?u=${friend.emailAddress}`
                }
                alt={`${friend.firstName} ${friend.lastName}`}
                className="rounded-circle"
                style={{ width: 50, height: 50, objectFit: "cover" }}
              />
              <span className="fw-semibold">
                {friend.firstName} {friend.lastName}
              </span>
            </div>
          ))
        )}
        <div style={{ height: "100px" }} />
      </div>
    </div>
  );
};

export default AllFriends;
