import React, { useState, FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import algebraLogo from "../assets/algebra-logo.png";
import axios from "axios";
import ErrorPopup from "../components/ErrorPopup";
import SuccessPopup from "../components/SuccessPopup";

const Login: React.FC = () => {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const [twoFA, setTwoFA] = useState<string>("sms");
  const [show2FA, setShow2FA] = useState(false);
  const [twoFACode, setTwoFACode] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [showSuccess, setShowSuccess] = useState(false);

  const navigate = useNavigate();

  const handleLogin = async (e: FormEvent) => {
    e.preventDefault();

    // ✅ Check email domain
    const emailDomain = email.split("@")[1];
    if (emailDomain !== "algebra.hr") {
      setError("Only users with an @algebra.hr email can log in.");
      return;
    }

    setLoading(true);

    try {
      const response = await axios.post(
        "http://localhost:5026/api/User/LoginUser",
        {
          email: email,
          userPassword: password,
          codeSenderOption: twoFA,
        }
      );
      console.log(`Response: `, response);
      setLoading(false);
      setShow2FA(true);
    } catch (err) {
      console.log(err);
      setLoading(false);
      setError(
        "Login failed. The email or password is not correct.\nUse the credentials received for Infoeduka."
      );
    }
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

          {/* ✅ Two-Factor Selection */}
          <div className="mb-3 text-center">
            <label className="form-label d-block">
              Choose a two-factor authentication (2FA) method:{" "}
            </label>
            <div className="form-check form-check-inline">
              <input
                className="form-check-input"
                type="radio"
                name="twoFA"
                id="sms"
                value="sms"
                checked={twoFA === "sms"}
                onChange={(e) => setTwoFA(e.target.value)}
                disabled={loading}
              />
              <label className="form-check-label" htmlFor="sms">
                SMS
              </label>
            </div>
            <div className="form-check form-check-inline">
              <input
                className="form-check-input"
                type="radio"
                name="twoFA"
                id="email"
                value="email"
                checked={twoFA === "email"}
                onChange={(e) => setTwoFA(e.target.value)}
                disabled={loading}
              />
              <label className="form-check-label" htmlFor="email">
                Email
              </label>
            </div>
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

      {show2FA && (
        <div
          className="position-fixed top-0 start-0 w-100 h-100 bg-dark bg-opacity-75 d-flex justify-content-center align-items-center"
          style={{ zIndex: 1050 }}
        >
          <div
            className="card bg-black text-white p-4 shadow-lg"
            style={{ maxWidth: 360, width: "90%" }}
          >
            <h5 className="text-center mb-3">Enter 2FA Code</h5>
            <p className="text-muted text-center mb-4">
              A 6-digit code has been sent via{" "}
              <strong>{twoFA.toUpperCase()}</strong>
            </p>
            <input
              type="text"
              maxLength={6}
              className="form-control text-center fs-4 mb-3"
              value={twoFACode}
              onChange={(e) => setTwoFACode(e.target.value)}
              placeholder="Enter 6-digit code"
            />

            {showSuccess && (
              <SuccessPopup
                message="Login successful! Welcome back 🎉"
                onClose={() => {
                  setShowSuccess(false);
                  navigate("/dashboard");
                }}
              />
            )}
            <button
              type="button"
              className="btn-close btn-close-white position-absolute top-0 end-0 m-3"
              aria-label="Close"
              onClick={() => setShow2FA(false)}
            ></button>
            <button
              className="btn btn-info w-100"
              onClick={async () => {
                try {
                  const now = new Date().toISOString();
                  const response = await axios.post(
                    "http://localhost:5026/api/User/Verify2FA",
                    {
                      email: email,
                      authCode: twoFACode,
                      authCodeTime: now,
                    }
                  );
                  console.log(`Got the response: ${response.data}`);
                  const token = response.data;

                  if (token) {
                    localStorage.setItem("isLoggedIn", "true");
                    localStorage.setItem("jwtToken", token);
                    localStorage.setItem("userEmail", email);
                    setShowSuccess(true);
                  } else {
                    setError("No token received.");
                    alert();
                  }
                } catch (error) {
                  console.error("2FA verification failed:", error);
                  setError(`2FA verification failed: ${error}`);
                }
              }}
            >
              Verify
            </button>
          </div>
        </div>
      )}
      {error && <ErrorPopup message={error} onClose={() => setError(null)} />}
    </div>
  );
};

export default Login;
