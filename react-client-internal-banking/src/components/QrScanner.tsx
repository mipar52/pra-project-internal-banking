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
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const successNavigateRef = useRef<{ path: string; state?: any } | null>(null);

  const [scanned, setScanned] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [showLoading, setShowLoading] = useState(false);
  const [scannerPausedDueToError, setScannerPausedDueToError] = useState(false);
  const [viewportHeight, setViewportHeight] = useState(window.innerHeight);

  useEffect(() => {
    const handleResize = () => {
      setViewportHeight(window.innerHeight);
    };

    handleResize(); // set on mount
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  const navigate = useNavigate();

  const startScanner = async () => {
    if (isTransitioningRef.current) return;

    const container = document.getElementById(qrRegionId);
    if (!container) return;
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
          setScanned(decodedText);

          try {
            const cleaned = decodedText.replace("AlgebraQR", "");
            const parsed = JSON.parse(cleaned);

            if (!parsed.type || parsed.type !== "ALGEBRAQR") {
              setError("Not an official Algebra QR code.");
              await scanner.pause();
              setScannerPausedDueToError(true);
              return;
            }

            const userEmail = localStorage.getItem("userEmail");
            const jwt = localStorage.getItem("jwtToken");

            if (!userEmail || !jwt) {
              setError("User session not found.");
              return;
            }

            if (parsed.friendEmail) {
              // Friend QR
              setShowLoading(true);
              try {
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
                  `Successfully added ${parsed.friendEmail} to friends!`
                );
                successNavigateRef.current = { path: "/all-friends" };

                timeoutRef.current = setTimeout(() => {
                  navigate("/all-friends");
                }, 4000);
              } catch (error: any) {
                if (!parsed.friendEmail.endsWith("algebra.hr")) {
                  setError(
                    `This friend QR code is invalid.\nEmail ${parsed.friendEmail} is not a valid Algebra account.`
                  );
                  await scanner.pause();
                  setScannerPausedDueToError(true);
                  return;
                }
                console.log(error.response.data);
                const msg =
                  error?.response?.data === "Friend failed"
                    ? `You already have ${parsed.friendEmail} as a friend.`
                    : `This friend QR code is invalid.\nEmail ${parsed.friendEmail} is not a valid Algebra address.`;
                setError(error?.response?.data);
                await scanner.pause();
                setScannerPausedDueToError(true);
              }
            } else if (parsed.amount && parsed.transactionTypeId) {
              // Payment QR
              setSuccess("Payment QR scanned successfully!");
              successNavigateRef.current = {
                path: "/enter-manually",
                state: {
                  amount: parsed.amount,
                  transactionTypeId: parsed.transactionTypeId,
                },
              };

              timeoutRef.current = setTimeout(() => {
                navigate("/enter-manually", {
                  state: parsed,
                });
              }, 4000);
            } else {
              setError("Unsupported QR content.");
              await scanner.pause();
              setScannerPausedDueToError(true);
            }
          } catch (err) {
            console.error("QR parse error:", err);
            setError("Invalid QR format.");
            await scanner.pause();
            setScannerPausedDueToError(true);
          } finally {
            setShowLoading(false);
            try {
              await scanner.pause();
            } catch {}
            isTransitioningRef.current = false;
          }
        },
        (scanErr) => {
          console.warn("QR scan error:", scanErr);
        }
      );
    } catch (err) {
      console.error("Scanner error:", err);
      // setError("Camera error: " + err);
      isTransitioningRef.current = false;
    }
  };

  useEffect(() => {
    requestAnimationFrame(() => startScanner());

    return () => {
      if (timeoutRef.current) clearTimeout(timeoutRef.current);
      scannerRef.current?.stop().catch(() => {});
    };
  }, []);

  const handleSuccessClose = () => {
    setSuccess(null);
    if (timeoutRef.current) clearTimeout(timeoutRef.current);

    if (successNavigateRef.current) {
      navigate(successNavigateRef.current.path, {
        state: successNavigateRef.current.state,
      });
    }
  };

  return (
    <div
      className="position-absolute d-flex flex-column justify-content-start align-items-center"
      style={{
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: "black",
        zIndex: 0,
        paddingTop: "6rem", // Moves scanner further down
      }}
    >
      {showLoading && <LoadingBar message="Processing QR code..." />}
      {success && (
        <SuccessPopup message={success} onClose={handleSuccessClose} />
      )}
      {error && (
        <ErrorPopup
          message={error}
          onClose={async () => {
            setError(null);
            if (scannerPausedDueToError && scannerRef.current) {
              await scannerRef.current.resume();
              setScannerPausedDueToError(false);
            }
          }}
        />
      )}

      <div
        className="position-absolute top-0 start-0 end-0 py-3 px-4 text-white"
        style={{ backgroundColor: "black", zIndex: 10 }}
      >
        <h5 className="mb-0 text-center">Scan the QR code</h5>
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
          width: "90%",
          maxWidth: "400px",
          aspectRatio: "1 / 1",
          border: "2px solid white",
          borderRadius: "1rem",
          overflow: "hidden",
          position: "relative",
        }}
      />
      <h5 className="mb-0 text-center text-white" style={{ marginTop: "2rem" }}>
        Only official AlgebraQR QR codes are accepted.<p></p>The university is
        not responsible for unauthorized QR codes.
      </h5>
    </div>
  );
};

export default QrScanner;
