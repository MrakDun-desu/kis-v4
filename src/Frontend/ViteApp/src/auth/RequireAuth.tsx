import { useEffect } from "react";
import { useAuth } from "./AuthContext";
import { Backdrop, Box, Button, CircularProgress } from "@mui/material";

export const RequireAuth = ({ children }: { children: React.ReactNode }) => {
  const auth = useAuth();

  if (auth.loading) {
    return (
      <Backdrop open={true}>
        <CircularProgress />
      </Backdrop>
    );
  }

  if (auth.userClaims) {
    return children;
  }

  return (
    <Box
      display="flex"
      alignItems="center"
      justifyContent="center"
      flexDirection="column"
      height="100vh"
      gap={1.5}
    >
      <h1>Kachní Informační Systém</h1>
      <Button variant="outlined" onClick={auth.signIn}>
        Přihlásit
      </Button>
    </Box>
  );
};
