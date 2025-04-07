// src/router/AllFriends.tsx
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import algebraLogo from "../assets/algebra-logo.png";

interface Friend {
  id: number;
  username: string;
  image: string;
}

const dummyFriends: Friend[] = [
  {
    id: 1,
    username: "domagoj.algebra",
    image: "https://i.pravatar.cc/150?img=1",
  },
  {
    id: 2,
    username: "milica.k",
    image: "https://i.pravatar.cc/150?img=2",
  },
  {
    id: 3,
    username: "robert.robic",
    image: "https://i.pravatar.cc/150?img=3",
  },
  {
    id: 4,
    username: "franjo.cicak",
    image: "https://i.pravatar.cc/150?img=4",
  },
];

const AllFriends: React.FC = () => {
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  const filteredFriends = dummyFriends.filter((f) =>
    f.username.toLowerCase().includes(search.toLowerCase())
  );

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
          placeholder="Search by username..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      {/* Friends List */}
      <div className="flex-grow-1 overflow-auto px-3">
        {filteredFriends.length === 0 ? (
          <div className="text-center text-muted mt-5">No users found.</div>
        ) : (
          filteredFriends.map((friend) => (
            <div
              key={friend.id}
              className="d-flex align-items-center gap-3 border-bottom border-secondary py-3"
              style={{ cursor: "pointer" }}
              onClick={() => navigate("/friend-details", { state: friend })}
            >
              <img
                src={friend.image}
                alt={friend.username}
                className="rounded-circle"
                style={{ width: 50, height: 50, objectFit: "cover" }}
              />
              <span className="fw-semibold">{friend.username}</span>
            </div>
          ))
        )}
        <div style={{ height: "100px" }} />
      </div>
    </div>
  );
};

export default AllFriends;
