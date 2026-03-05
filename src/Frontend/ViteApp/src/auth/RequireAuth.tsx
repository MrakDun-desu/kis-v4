import { useAuth } from "./AuthContext";
import { Button } from "@mui/material";

export const RequireAuth = ({ children }: { children: React.ReactNode }) => {
  const auth = useAuth();

  console.log(auth);

  if (auth.userClaims) {
    return children;
  }

  return (
    <>
      <h1>Nejste přihlášen</h1>
      <Button variant="outlined" onClick={auth.signIn}>
        Přihlásit
      </Button>
    </>
  );
};
