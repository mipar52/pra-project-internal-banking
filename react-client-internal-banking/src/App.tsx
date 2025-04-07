import {
  BrowserRouter as Router,
  Routes,
  Route,
  useLocation,
} from "react-router-dom";
import Login from "./router/Login";
import Dashboard from "./router/Dashboard";
import TopUp from "./router/TopUp";
import QrScanner from "./components/QrScanner";
import BottomNav from "./components/BottomNav";
import Transactions from "./router/Transactions";
import TransactionDetails from "./router/TransactionDetails";
import History from "./router/History";
import Settings from "./router/Settings";
import PaymentOptionScreen from "./router/PaymentOptionScreen";
import EnterManually from "./router/EnterManually";
import UserQrCode from "./router/UserQrCode";
import SelectCard from "./router/SelectCard";
import Profile from "./router/MyProfile";
import ParkingSettings from "./router/ParkingSettings";
import RequestMoney from "./router/RequestMoney";
import AllFriends from "./router/AllFriends";
import FriendDetails from "./router/FriendDetails";
import useWindowSize from "./hooks/useWindowSize";

const AppWrapper: React.FC = () => {
  const location = useLocation();
  const { width } = useWindowSize();
  const isDesktop = width >= 768;
  const hideNavRoutes = ["/"];

  return (
    <div className={isDesktop ? "d-flex" : ""}>
      {/* BottomNav (sidebar or mobile nav) */}
      {!hideNavRoutes.includes(location.pathname) && <BottomNav />}

      {/* This container shifts content when sidebar is shown on desktop */}
      <div
        style={{
          width: "100%",
          marginLeft: isDesktop ? "220px" : 0, // 🧠 Adjust for sidebar width
          transition: "margin-left 0.3s ease",
        }}
      >
        <Routes>
          <Route path="/" element={<Login />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/topup" element={<TopUp />} />
          <Route path="/qr" element={<QrScanner />} />
          <Route path="/transactions" element={<Transactions />} />
          <Route path="/transaction-details" element={<TransactionDetails />} />
          <Route path="/history" element={<History />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/enter-manually" element={<EnterManually />} />
          <Route path="/payment-options" element={<PaymentOptionScreen />} />
          <Route path="/user-qr" element={<UserQrCode />} />
          <Route path="/select-card" element={<SelectCard />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/parking-settings" element={<ParkingSettings />} />
          <Route path="/request-money" element={<RequestMoney />} />
          <Route path="/all-friends" element={<AllFriends />} />
          <Route path="/friend-details" element={<FriendDetails />} />
        </Routes>
      </div>
    </div>
  );
};


const App: React.FC = () => {
  return (
    <Router>
      <AppWrapper />
    </Router>
  );
};

export default App;
