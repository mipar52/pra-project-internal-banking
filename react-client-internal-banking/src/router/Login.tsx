// src/Login.tsx
import React, { useState, FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import algebraLogo from "../assets/algebra-logo.png";

const Login: React.FC = () => {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();

  const handleLogin = (e: FormEvent) => {
    e.preventDefault();
    setLoading(true);

    // Simulated login
    setTimeout(() => {
      setLoading(false);
      localStorage.setItem("isLoggedIn", "true");
      navigate("/dashboard");
    }, 2000);
  };

  return (
    <div
      className="d-flex flex-column align-items-center justify-content-center text-white bg-black"
      style={{ minHeight: "100vh", padding: "1rem" }}
    >
      <img
        src={algebraLogo}
        alt="Algebra Logo"
        className="mb-4"
        style={{ width: 100 }}
      />

      <div
        className="card bg-dark text-white p-4 shadow"
        style={{ width: "100%", maxWidth: "400px" }}
      >
        <h3 className="text-center mb-4">Login</h3>
        <form onSubmit={handleLogin}>
          <div className="mb-3">
            <label htmlFor="email" className="form-label">
              Email address
            </label>
            <input
              type="email"
              className="form-control bg-black text-white border-secondary"
              id="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              disabled={loading}
            />
          </div>
          <div className="mb-3">
            <label htmlFor="password" className="form-label">
              Password
            </label>
            <input
              type="password"
              className="form-control bg-black text-white border-secondary"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              disabled={loading}
            />
          </div>
          <button
            type="submit"
            className="btn btn-primary w-100"
            disabled={loading}
          >
            {loading ? (
              <>
                <span
                  className="spinner-border spinner-border-sm me-2"
                  role="status"
                  aria-hidden="true"
                ></span>
                Logging in...
              </>
            ) : (
              "Login"
            )}
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;
