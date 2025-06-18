import React from "react";

interface LoadingBarProps {
  message?: string;
}

const LoadingBar: React.FC<LoadingBarProps> = ({
  message = "Processing...",
}) => (
  <div
    className="position-fixed top-0 start-0 w-100 h-100 bg-black bg-opacity-75 d-flex flex-column justify-content-center align-items-center"
    style={{ zIndex: 9999 }}
  >
    <div
      className="spinner-border text-info mb-3"
      role="status"
      style={{ width: "3rem", height: "3rem" }}
    >
      <span className="visually-hidden">Loading...</span>
    </div>
    <div className="text-white fw-semibold">{message}</div>
  </div>
);

export default LoadingBar;
