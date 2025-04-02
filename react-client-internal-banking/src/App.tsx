import { BrowserRouter as Router, Route } from "react-router-dom";
import Login from "./components/Login";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
      </Routes>
    </Router>
  );
};

export default App;
