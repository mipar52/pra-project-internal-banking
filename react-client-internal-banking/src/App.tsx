import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./router/Login";
import Dashboard from "./router/Dashboard";
import TopUp from "./router/TopUp";
import QrScanner from "./components/QrScanner";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/topup" element={<TopUp />} />
        <Route path="/qr" element={<QrScanner />} />
      </Routes>
    </Router>
  );
};

export default App;
