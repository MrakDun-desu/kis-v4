import { Box, Button } from "@mui/material";
import { useNavigate } from "react-router-dom";

export const HomePage = () => {
  const navigate = useNavigate();

  return (
    <Box
      display="flex"
      alignItems="center"
      flexDirection="column"
      justifyContent="center"
      height="100vh"
    >
      <h1>KISv4</h1>
      <Box
        display="flex"
        gap="1.5em"
        flexDirection="column"
        alignItems="center"
      >
        <Button
          variant="outlined"
          size="large"
          onClick={() => navigate("admin/store-items")}
        >
          Admin
        </Button>
        <Button variant="outlined" size="large" onClick={() => navigate("pos")}>
          POS
        </Button>
      </Box>
    </Box>
  );
};
