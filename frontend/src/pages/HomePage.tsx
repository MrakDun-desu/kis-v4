import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom"
import "./HomePage.css"

export const HomePage = () => {
  const navigate = useNavigate();

  return (
    <div className="container">
      <h1>KISv4</h1>
      <Button
        variant="outlined"
        size="large"
        onClick={() => navigate("admin")}
      >
        Admin
      </Button>
      <Button
        variant="outlined"
        size="large"
        onClick={() => navigate("pos")}
      >
        POS
      </Button>
    </div>
  );
}

