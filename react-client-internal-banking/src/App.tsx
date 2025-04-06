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
import Friends from "./router/Friends";
import FriendDetails from "./router/FriendDetails";
import History from "./router/History";
import Settings from "./router/Settings";
import PaymentOptionScreen from "./router/PaymentOptionScreen";
import EnterManually from "./router/EnterManually";
import UserQrCode from "./router/UserQrCode";
import SelectCard from "./router/SelectCard";
import Profile from "./router/MyProfile";
import ParkingSettings from "./router/ParkingSettings";
import RequestMoney from "./router/RequestMoney";

const AppWrapper: React.FC = () => {
  const location = useLocation();
  const hideNavRoutes = ["/"];

  return (
    <>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/topup" element={<TopUp />} />
        <Route path="/qr" element={<QrScanner />} />
        <Route path="/friends" element={<Friends />} />
        <Route path="/friend-details" element={<FriendDetails />} />
        <Route path="/history" element={<History />} />
        <Route path="/settings" element={<Settings />} />
        <Route path="/enter-manually" element={<EnterManually />} />
        <Route path="/payment-options" element={<PaymentOptionScreen />} />
        <Route path="/user-qr" element={<UserQrCode />} />
        <Route path="/select-card" element={<SelectCard />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/parking-settings" element={<ParkingSettings />} />
        <Route path="/request-money" element={<RequestMoney />} />

      </Routes>

      {!hideNavRoutes.includes(location.pathname) && <BottomNav />}
    </>
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
