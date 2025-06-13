import React, { useEffect, useRef, useState } from "react";
import { Html5Qrcode, Html5QrcodeScannerState } from "html5-qrcode";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../axiosHelper/axiosInstance";
import LoadingBar from "../components/LoadingBar";
import ErrorPopup from "../components/ErrorPopup";
import SuccessPopup from "./SuccessPopup";

const QrScanner: React.FC = () => {
  const qrRegionId = "qr-reader";
  const scannerRef = useRef<Html5Qrcode | null>(null);
  const isTransitioningRef = useRef(false);
  const [scanned, setScanned] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [showLoading, setShowLoading] = useState(false);
  const navigate = useNavigate();

  const startScanner = async () => {
    if (isTransitioningRef.current) return;

    const container = document.getElementById(qrRegionId);
    if (!container) {
      console.error("QR container not found.");
      return;
    }
    container.innerHTML = "";

    if (scannerRef.current === null) {
      scannerRef.current = new Html5Qrcode(qrRegionId);
    }

    const scanner = scannerRef.current;

    const state = scanner.getState();
    if (
      state !== Html5QrcodeScannerState.NOT_STARTED &&
      state !== Html5QrcodeScannerState.PAUSED
    ) {
      console.warn("Scanner is currently transitioning or already started.");
      return;
    }

    try {
      const devices = await Html5Qrcode.getCameras();
      if (devices.length === 0) {
        setError("No cameras found.");
        return;
      }

      isTransitioningRef.current = true;

      await scanner.start(
        devices[0].id,
        {
          fps: 10,
          qrbox: (vw, vh) => {
            const minEdge = Math.min(vw, vh);
            return {
              width: Math.floor(minEdge * 0.8),
              height: Math.floor(minEdge * 0.8),
            };
          },
        },
        async (decodedText) => {
          if (scanned || isTransitioningRef.current) return;

          isTransitioningRef.current = true;
          // scanner.pause();
          setScanned(decodedText);

          try {
            const cleaned = decodedText.replace("AlgebraQR", "");
            console.log(`cleaned: ${cleaned}`);
            const parsed = JSON.parse(cleaned);
            console.log(`parsed: ${parsed}`);

            if (!parsed.type || parsed.type !== "ALGEBRAQR") {
              setError("Not an official Algebra QR code.");
              scanner.resume();
              return;
            }
            console.log("scanner paused!");
            //   scanner.pause();
            const userEmail = localStorage.getItem("userEmail");
            const jwt = localStorage.getItem("jwtToken");

            if (!userEmail || !jwt) {
              setError("User session not found.");
              return;
            }

            if (parsed.friendEmail) {
              // Friend QR
              setShowLoading(true);
              await axiosInstance.post(
                "http://localhost:5026/api/Friend/CreateFriendByMail",
                {
                  userEmail,
                  friendEmail: parsed.friendEmail,
                },
                {
                  headers: { Authorization: `Bearer ${jwt}` },
                }
              );
              setSuccess(
                `Successfully added ${parsed.friendEmail} to friends!\nRedirecting to your friends...`
              );
              setTimeout(() => {
                navigate("/all-friends");
              }, 4000);
            } else if (parsed.amount && parsed.transactionTypeId) {
              // Payment QR
              setSuccess(
                `Successfully obtained the payment QR code! Good job!\nRedirecting to the payment screen...`
              );
              setTimeout(() => {
                navigate("/enter-manually", {
                  state: {
                    amount: parsed.amount,
                    transactionTypeId: parsed.transactionTypeId,
                  },
                });
              }, 4000);
            } else {
              setError("Unsupported QR content.");
            }
          } catch (err) {
            console.error("QR parse error:", err);
            scanner.pause();
            scanner.resume();
            setError("Invalid QR format.");
            navigate("/dashboard");
          } finally {
            setShowLoading(false);
            try {
              await scanner.pause();
            } catch (err) {
              console.warn("Scanner stop/clear failed", err);
            }
            isTransitioningRef.current = false;
          }
        },
        (scanErr) => {
          console.warn("QR scan error:", scanErr);
        }
      );
    } catch (err) {
      console.error("Scanner error:", err);
      setError("Camera error: " + err);
      isTransitioningRef.current = false;
    }
  };

  useEffect(() => {
    requestAnimationFrame(() => {
      startScanner();
    });

    return () => {
      scannerRef.current?.stop().catch(() => {});
    };
  }, []);

  return (
    <div
      className="d-flex flex-column align-items-center justify-content-center position-relative"
      style={{
        width: "100vw",
        height: "100vh",
        backgroundColor: "black",
        overflow: "hidden",
      }}
    >
      {showLoading && <LoadingBar message="Processing QR code..." />}
      {success && (
        <SuccessPopup message={success} onClose={() => setSuccess(null)} />
      )}
      {error && <ErrorPopup message={error} onClose={() => setError(null)} />}

      <div
        className="position-absolute top-0 start-0 end-0 py-3 px-4 text-white"
        style={{ backgroundColor: "#00AEEF", zIndex: 10 }}
      >
        <h5 className="mb-0 text-center">Skeniraj i plati</h5>
      </div>

      <button
        onClick={() => navigate("/dashboard")}
        className="btn btn-link position-absolute top-0 start-0 text-white fs-3"
        style={{ padding: "1rem", zIndex: 11 }}
      >
        ✕
      </button>

      <div
        id={qrRegionId}
        style={{
          width: "100%",
          height: "100%",
          marginTop: "3.5rem",
          position: "relative",
        }}
      />
    </div>
  );
};

export default QrScanner;
