import { BrowserRouter as Router, Route } from "react-router-dom";
import Login from "./routes/Login";
import Dashboard from "./routes/Dashboard";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/dashboard" element={<Dashboard />} />
      </Routes>
    </Router>
  );
};

export default App;
