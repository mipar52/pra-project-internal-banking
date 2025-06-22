import React, { useState, useEffect, useRef } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import axios from "axios";
import "../css/History.css";

interface Transaction {
  typeName: string;
  amount: number;
  date: string;
  transactionTypeId: number;
}

const History: React.FC = () => {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [filteredTransactions, setFilteredTransactions] = useState<
    Transaction[]
  >([]);
  const [selectedTab, setSelectedTab] = useState<string>("");
  const [monthTabs, setMonthTabs] = useState<string[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const searchInputRef = useRef<HTMLInputElement>(null);

  const getIconForType = (type: string) => {
    const map: Record<string, string> = {
      Food: "🍔",
      Drinks: "🧃",
      Parking: "🅿️",
      Bills: "📄",
      Tuition: "🎓",
      Other: "❓",
    };
    return map[type] || "💸";
  };

  const getMonthLabel = (dateStr: string) => {
    const d = new Date(dateStr);
    return d.toLocaleDateString("en-US", {
      month: "long",
      year: "numeric",
    }); // e.g., "May 2025"
  };

  useEffect(() => {
    const fetchTransactions = async () => {
      const email = localStorage.getItem("userEmail");
      const token = localStorage.getItem("jwtToken");

      if (!email || !token) return;

      try {
        const response = await axios.get(
          `http://localhost:5026/api/Transaction/GetTransactionsByEmail/${encodeURIComponent(
            email
          )}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        setTransactions(response.data);
      } catch (error) {
        console.error("Failed to load transactions:", error);
      }
    };

    fetchTransactions();
  }, []);

  // Generate dynamic month tabs
  useEffect(() => {
    const uniqueMonths = Array.from(
      new Set(transactions.map((t) => getMonthLabel(t.date)))
    );

    setMonthTabs(["All", ...uniqueMonths]);

    // Default to "All" if none selected
    if (!selectedTab || !["All", ...uniqueMonths].includes(selectedTab)) {
      setSelectedTab("All");
    }
  }, [transactions]);

  useEffect(() => {
    const filtered =
      selectedTab === "All"
        ? transactions
        : transactions.filter((t) => getMonthLabel(t.date) === selectedTab);

    setFilteredTransactions(filtered);
  }, [selectedTab, transactions]);

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

  const displayed = filteredTransactions
    .filter((t) => t.typeName.toLowerCase().includes(searchTerm.toLowerCase()))
    .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()); // 👈 sort newest first

  const chartData = filteredTransactions.map((t) => ({
    date: new Date(t.date).toLocaleDateString("hr-HR", {
      day: "2-digit",
      month: "2-digit",
    }),
    amount: t.amount,
  }));

  return (
    <div className="history-wrapper bg-black text-white d-flex flex-column h-100">
      {/* Sticky Top */}
      <div className="sticky-top bg-dark p-3 shadow-sm">
        <h4 className="mb-2">Transaction history</h4>
        <div className="d-flex gap-2 flex-wrap">
          {monthTabs.map((month) => (
            <button
              key={month}
              className={`btn ${
                selectedTab === month
                  ? "btn-primary"
                  : "btn-outline-secondary text-white"
              } btn-sm`}
              onClick={() => setSelectedTab(month)}
            >
              {month}
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
            placeholder="🔍 Search by type..."
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
          {chartData.length > 0 ? (
            <ResponsiveContainer width="100%" height={200}>
              <LineChart data={chartData}>
                <XAxis dataKey="date" tick={{ fill: "#aaa" }} />
                <YAxis tick={{ fill: "#aaa" }} />
                <Tooltip
                  contentStyle={{
                    backgroundColor: "#333",
                    borderColor: "#666",
                  }}
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
          ) : (
            <div className="text-muted text-center py-3">
              No data for selected month
            </div>
          )}
        </div>
        {/* Transaction List */}
        <h6 className="mb-2 fw-bold">Recent</h6>
        {displayed.length > 0 ? (
          <div className="list-group">
            {displayed.map((tx, idx) => (
              <div
                key={idx}
                className="list-group-item bg-dark text-white border-secondary d-flex justify-content-between align-items-start"
              >
                <div>
                  <div className="fw-bold">
                    {getIconForType(tx.typeName)} {tx.typeName}
                  </div>
                  <div className="small text-white mt-1 fw-bold">
                    {new Date(tx.date).toLocaleString("hr-HR", {
                      day: "2-digit",
                      month: "2-digit",
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </div>
                </div>
                <div className="text-end">
                  <div className="text-info fw-bold">€{tx.amount}</div>
                  <div className="small text-success">Completed</div>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <div className="text-muted text-center">No results found.</div>
        )}
        <div style={{ height: "100px" }} /> {/* bottom spacing */}
      </div>
    </div>
  );
};

export default History;
