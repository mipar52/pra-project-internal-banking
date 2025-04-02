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
      style={{ backgroundColor: "rgba(0,0,0,0.5)" }}
    >
      <div className="modal-dialog modal-dialog-centered" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Are you sure you want to logout?</h5>
          </div>
          <div className="modal-body text-center">
            <button className="btn btn-danger me-3" onClick={logout}>
              Yes
            </button>
            <button className="btn btn-secondary" onClick={onClose}>
              No
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LogoutModal;
