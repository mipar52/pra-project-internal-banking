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
