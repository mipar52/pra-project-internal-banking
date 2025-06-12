import React, { useEffect, useRef, useState } from "react";
import { Html5Qrcode } from "html5-qrcode";
import { Html5QrcodeScannerState } from "html5-qrcode";
import { useNavigate } from "react-router-dom";

const QrScanner: React.FC = () => {
  const qrRegionId = "qr-reader";
  const scannerRef = useRef<Html5Qrcode | null>(null);
  const [scanned, setScanned] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const isTransitioningRef = useRef(false);
  let isMounted = true;

  const startScanner = async () => {
    if (isTransitioningRef.current) return;

    const element = document.getElementById(qrRegionId);
    if (element) {
      element.innerHTML = "";
    }

    if (!scannerRef.current) {
      scannerRef.current = new Html5Qrcode(qrRegionId);
    }

    const devices = await Html5Qrcode.getCameras();
    if (devices.length === 0) {
      setError("No cameras found.");
      return;
    }

    const scanner = scannerRef.current;

    if (
      scanner.getState &&
      scanner.getState() === Html5QrcodeScannerState.SCANNING
    ) {
      return;
    }

    try {
      await scanner.start(
        devices[0].id,
        {
          fps: 10,
          qrbox: (viewportWidth, viewportHeight) => {
            const minEdge = Math.min(viewportWidth, viewportHeight);
            return {
              width: Math.floor(minEdge * 0.8),
              height: Math.floor(minEdge * 0.8),
            };
          },
        },
        (decodedText) => {
          if (!scanned && isMounted && !isTransitioningRef.current) {
            setScanned(decodedText);
            isTransitioningRef.current = true;

            // Re-route to enter-manually screen
            // Example QR: 
            // AlgebraQR{"recipientName":"Algebra", "iban":"HR1234567890", "amount":"123.45", "model":"HR01", "referenceNumber":"2024-12345", "description":"Tuition"}
            if (decodedText.includes("AlgebraQR")) {
              try {
                const data = JSON.parse(decodedText.replace("AlgebraQR", ""));
                navigate("/enter-manually", { state: data });

                /**
                 * navigate("/enter-manually", {
  state: {
    recipientName: "Algebra",
    iban: "HR1234567890",
    amount: "123.45",
    model: "HR01",
    referenceNumber: "2024-12345",
    description: "Tuition"
  }
});
                 */
              } catch (err) {
                setError("Invalid QR format.");
              }
            }

            if (!scanner.isScanning) {
              scanner
                .stop()
                .then(() => {
                  console.log("Scanner stopped");
                  return scanner.clear();
                })
                .then(() => {
                  isTransitioningRef.current = false;
                })
                .catch((err) => {
                  console.warn("Error during stop/clear", err);
                  isTransitioningRef.current = false;
                });
            }
          }
        },
        (errorMessage) => {
          if (isMounted) console.warn("QR error", errorMessage);
        }
      );

      isTransitioningRef.current = false;
    } catch (err) {
      console.error("Scanner error:", err);
      setError("Camer error: " + err);
      isTransitioningRef.current = false;
    }
  };

  useEffect(() => {
    startScanner();
    return () => {
      isMounted = false;
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

      {scanned && (
        <div
          className="alert alert-success position-absolute bottom-0 start-0 end-0 m-3 text-center"
          style={{ zIndex: 10 }}
        >
          <strong>Scanned:</strong> {scanned}
          <br />
          <button
            className="btn btn-primary mt-2"
            onClick={() => navigate("/dashboard")}
          >
            Back to Dashboard
          </button>
        </div>
      )}

      {error && (
        <div
          className="alert alert-danger position-absolute bottom-0 start-0 end-0 m-3 text-center"
          style={{ zIndex: 10 }}
        >
          {error}
          <br />
          <button
            className="btn btn-secondary mt-2"
            onClick={() => navigate("/dashboard")}
          >
            Back
          </button>
        </div>
      )}
    </div>
  );
};

export default QrScanner;
