import React, { useState, useEffect, useRef } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import "../css/History.css";

const History: React.FC = () => {
  const [selectedTab, setSelectedTab] = useState("last");
  const [searchTerm, setSearchTerm] = useState("");
  const searchInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const handleKey = (e: KeyboardEvent) => {
      if (e.key === "/") {
        e.preventDefault();
        searchInputRef.current?.focus();
      }
    };
    document.addEventListener("keydown", handleKey);
    return () => document.removeEventListener("keydown", handleKey);
  }, []);

  const tabs = [
    { label: "Last Month", value: "last" },
    { label: "This Month", value: "this" },
    { label: "All", value: "all" },
  ];

  const transactions = [
    {
      id: 1,
      merchant: "Algebra parking",
      amount: 5,
      date: "26.03. 09:22",
      note: "Kešššš",
      status: "Completed",
      icon: "🚌",
    },
    {
      id: 2,
      merchant: "Kafe aparat",
      amount: 3,
      date: "25.03. 20:04",
      note: "Plaćeno AlgebraPayom",
      status: "Completed",
      icon: "🍔",
    },
    {
      id: 3,
      merchant: "Tvoja stara",
      amount: 3,
      date: "25.03. 20:04",
      note: "Plaćeno karticom",
      status: "Completed",
      icon: "🍔",
    },
    {
      id: 4,
      merchant: "Kafe aparat",
      amount: 7,
      date: "25.03. 20:04",
      note: "Plaćeno u naturi ;))",
      status: "Completed",
      icon: "🍔",
    },
  ];

  const filteredTransactions = transactions.filter(
    (t) =>
      t.merchant.toLowerCase().includes(searchTerm.toLowerCase()) ||
      t.note.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="history-wrapper bg-black text-white d-flex flex-column h-100">
      {/* Sticky Top */}
      <div className="sticky-top bg-dark p-3 shadow-sm">
        <h4 className="mb-2">Transaction history</h4>
        <div className="d-flex gap-2">
          {tabs.map((tab) => (
            <button
              key={tab.value}
              className={`btn ${
                selectedTab === tab.value
                  ? "btn-primary"
                  : "btn-outline-secondary text-white"
              } btn-sm`}
              onClick={() => setSelectedTab(tab.value)}
            >
              {tab.label}
            </button>
          ))}
        </div>
      </div>

      {/* Scrollable Content */}
      <div className="history-content px-3 pt-3 overflow-auto bg-black flex-grow-1">
        {/* Search */}
        <div className="mb-3 position-relative">
          <input
            ref={searchInputRef}
            className="form-control bg-dark text-white border-secondary pe-5"
            type="text"
            placeholder="🔍 Search transactions"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          {searchTerm && (
            <button
              className="btn btn-sm btn-secondary position-absolute end-0 top-0 mt-1 me-2"
              onClick={() => setSearchTerm("")}
              style={{ zIndex: 1 }}
            >
              ✕
            </button>
          )}
        </div>
        {/* Chart */}
        <div className="bg-dark border rounded mb-4 p-3">
          <ResponsiveContainer width="100%" height={200}>
            <LineChart data={transactions}>
              <XAxis dataKey="date" tick={{ fill: "#aaa" }} />
              <YAxis tick={{ fill: "#aaa" }} />
              <Tooltip
                contentStyle={{ backgroundColor: "#333", borderColor: "#666" }}
                labelStyle={{ color: "#fff" }}
                itemStyle={{ color: "#fff" }}
              />
              <Line
                type="monotone"
                dataKey="amount"
                stroke="#00bfff"
                strokeWidth={2}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
        {/* Transactions */}
        <h6 className="text-muted mb-2">Recent</h6>
        {filteredTransactions.length > 0 ? (
          <div className="list-group">
            {filteredTransactions.map((tx) => (
              <div
                key={tx.id}
                className="list-group-item bg-dark text-white border-secondary d-flex justify-content-between align-items-start"
              >
                <div>
                  <div className="fw-bold">
                    {tx.icon} {tx.merchant}
                  </div>
                  <div className="small text-warning fw-semibold">
                    {tx.note}
                  </div>
                  <div className="small text-muted mt-1">{tx.date}</div>
                </div>
                <div className="text-end">
                  <div className="text-info fw-bold">€{tx.amount}</div>
                  <div className="small text-success">{tx.status}</div>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <div className="text-muted text-center">No results found.</div>
        )}
        <div style={{ height: "100px" }} /> {/* for padding above nav */}
      </div>
    </div>
  );
};

export default History;
