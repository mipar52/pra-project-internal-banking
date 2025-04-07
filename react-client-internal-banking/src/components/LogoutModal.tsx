// src/components/LogoutModal.tsx
import React from "react";
import { useNavigate } from "react-router-dom";

interface Props {
  show: boolean;
  onClose: () => void;
}

const LogoutModal: React.FC<Props> = ({ show, onClose }) => {
  const navigate = useNavigate();

  const logout = () => {
    localStorage.removeItem("isLoggedIn");
    localStorage.removeItem("balance");
    onClose();
    navigate("/");
  };

  return (
    <div
      className={`modal fade ${show ? "show d-block" : ""}`}
      tabIndex={-1}
      role="dialog"
      style={{ backgroundColor: "rgba(0, 0, 0, 0.7)", zIndex: 1055 }}
    >
      <div className="modal-dialog modal-dialog-centered" role="document">
        <div
          className="modal-content text-white"
          style={{
            backgroundColor: "#1D1D1B",
            border: "1px solid #F58220",
            borderRadius: "1rem",
          }}
        >
          <div className="modal-header border-0">
            <h5 className="modal-title w-100 text-center">
              Are you sure you want to log out?
            </h5>
          </div>
          <div className="modal-body text-center">
            <button
              className="btn w-100 mb-3"
              style={{ backgroundColor: "#A00062", color: "#fff" }}
              onClick={logout}
            >
              Yes, log me out
            </button>
            <button
              className="btn btn-outline-light w-100"
              onClick={onClose}
            >
              No, stay logged in
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LogoutModal;
