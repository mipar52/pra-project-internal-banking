import React, { useState, useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import algebraLogo from "../assets/algebra-logo.png";
import "../css/Friends.css";

const dummyData = [
  {
    name: "Franjo Cicak",
    message: "Ja šaljem 12 €",
    amount: "12 €",
    time: "10:20",
    label: "Ručak",
    color: "#A066FF",
    initials: "FC",
  },
  {
    name: "Domagoj Antic",
    message: "Domagoj ti šalje 3,50 €",
    amount: "3,50 €",
    time: "09:00",
    label: "Kava",
    color: "#FF784A",
    initials: "DA",
  },
  {
    name: "Milica Krmpotic",
    message: "Ja šaljem 50 €",
    amount: "50 €",
    time: "Jučer",
    label: "Poklon",
    color: "#4AC6FF",
    initials: "MK",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
  {
    name: "Srecko Mokric",
    message: "Srećko ti šalje 1 €",
    amount: "1 €",
    time: "05.12.",
    label: "Pizza",
    color: "#47D764",
    initials: "SM",
  },
];

const Transactions: React.FC = () => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState("");
  const inputRef = useRef<HTMLInputElement>(null);

  const filteredData = dummyData.filter((item) => {
    const lower = searchTerm.toLowerCase();
    return (
      item.name.toLowerCase().includes(lower) ||
      item.label.toLowerCase().includes(lower)
    );
  });

  // Handle "/" key to focus the search bar
  useEffect(() => {
    const handleKey = (e: KeyboardEvent) => {
      if (e.key === "/" && inputRef.current) {
        e.preventDefault();
        inputRef.current.focus();
      }
    };
    window.addEventListener("keydown", handleKey);
    return () => window.removeEventListener("keydown", handleKey);
  }, []);

  return (
    <div className="friends-screen">
      {/* Top bar */}
      <div className="top-bar">
        <img src={algebraLogo} alt="Algebra" className="algebra-logo" />
      </div>

      {/* Static header + search bar */}
      <div className="friends-header px-3 py-2">
        <h4 className="fw-bold mb-3">Daj te pare</h4>
        <div className="position-relative">
          <input
            ref={inputRef}
            type="text"
            className="form-control bg-dark text-white border-secondary pe-5"
            placeholder="Search by name or reason..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          {searchTerm && (
            <button
              className="btn btn-sm btn-outline-light position-absolute top-50 end-0 translate-middle-y me-3"
              onClick={() => setSearchTerm("")}
            >
              ✕
            </button>
          )}
        </div>
      </div>

      {/* Scrollable friend list */}
      <div className="friend-list-scroll flex-grow-1 overflow-auto">
        {filteredData.length === 0 ? (
          <div className="text-center opacity-50 pt-4">No results found.</div>
        ) : (
          filteredData.map((item, idx) => (
            <div
              className="friend-item d-flex justify-content-between align-items-center px-3 py-2"
              key={idx}
              onClick={() => navigate("/transaction-details", { state: item })}
              style={{ cursor: "pointer" }}
            >
              <div className="d-flex align-items-center gap-3">
                <div
                  className="avatar d-flex align-items-center justify-content-center"
                  style={{ backgroundColor: item.color }}
                >
                  {item.initials}
                </div>
                <div>
                  <div className="fw-bold">{item.name}</div>
                  <div className="small text-warning fw-semibold">
                    {item.label}
                  </div>
                </div>
              </div>
              <div className="text-end">
                <div className="fw-bold text-success fs-5">{item.amount}</div>
                <div className="small text-secondary opacity-75">
                  {item.time}
                </div>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};

export default Transactions;
