// src/router/Profile.tsx
import React, { useState } from "react";
import algebraLogo from "../assets/algebra-logo.png";
import { useNavigate } from "react-router-dom";

const Profile: React.FC = () => {
  const navigate = useNavigate();

  const [firstName, setFirstName] = useState<string>("");
  const [lastName, setLastName] = useState<string>("");
  const [username, setUsername] = useState<string>("");
  const [profileImage, setProfileImage] = useState<string>("");

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setProfileImage(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSave = () => {
    console.log("Saved:", { firstName, lastName, username, profileImage });
    // Save logic or API call can go here
    navigate("/settings");
  };

  return (
    <div
      className="min-vh-100 bg-black text-white d-flex flex-column align-items-center p-4"
      style={{ paddingBottom: "100px" }}
    >
      <img src={algebraLogo} alt="Algebra Logo" className="mb-4" style={{ height: 60 }} />
      <h4 className="mb-4">Edit Profile</h4>

      {/* Profile Image */}
      <div className="text-center mb-4">
        {profileImage ? (
          <img
            src={profileImage}
            alt="Profile"
            className="rounded-circle mb-2"
            style={{ width: 100, height: 100, objectFit: "cover" }}
          />
        ) : (
          <div
            className="rounded-circle bg-secondary mb-2 d-flex justify-content-center align-items-center"
            style={{ width: 100, height: 100 }}
          >
            👤
          </div>
        )}
        <input type="file" accept="image/*" onChange={handleImageChange} className="form-control bg-dark text-white" />
      </div>

      {/* Form Fields */}
      <div className="w-100" style={{ maxWidth: 400 }}>
        <div className="mb-3">
          <label className="form-label">First Name</label>
          <input
            className="form-control bg-dark text-white"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Last Name</label>
          <input
            className="form-control bg-dark text-white"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Username</label>
          <input
            className="form-control bg-dark text-white"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <button className="btn btn-info w-100" onClick={handleSave}>
          Save Profile
        </button>
      </div>
    </div>
  );
};

export default Profile;