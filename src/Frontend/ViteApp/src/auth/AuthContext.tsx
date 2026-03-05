import React from "react";

interface UserClaim {
  type: string;
  value: string;
  valueType: string | null;
}

interface AuthContextType {
  userClaims: UserClaim[] | null;
  signIn: () => void;
  signOut: () => void;
}

const AuthContext = React.createContext<AuthContextType>(null!);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [userClaims, setUserClaims] = React.useState<UserClaim[] | null>(null);

  React.useEffect(() => {
    const auth = async () => {
      try {
        const authResponse = await fetch(
          new Request("/bff/user", {
            headers: new Headers({
              "X-CSRF": "1",
            }),
          }),
        );
        if (authResponse.ok) {
          var respValue = await authResponse.json();
          setUserClaims(respValue);

          console.log("User logged in", userClaims);
        } else if (authResponse.status === 401) {
          console.log("User not logged in");
        }
      } catch (e) {
        alert("Nepovedlo se zjistit status přihlášení");
        console.error("Error checking user status: ", e);
      }
    };
    auth();
  }, []);

  const signIn = () => {
    const currentOrigin = encodeURIComponent(window.location.origin);
    window.location.href = `/bff/login?returnUrl=${currentOrigin}`;
  };

  const signOut = () => {
    const currentOrigin = encodeURIComponent(window.location.origin);
    if (userClaims) {
      const logoutUrlClaim = userClaims.find(
        (claim) => claim["type"] === "bff:logout_url",
      );
      if (logoutUrlClaim) {
        window.location.href = `${logoutUrlClaim.value}?returnUrl=${currentOrigin}`;
      }
      return;
    }
    window.location.href = `/bff/logout?returnUrl=${currentOrigin}`;
  };

  const value = { userClaims, signIn, signOut };

  return <AuthContext value={value}>{children}</AuthContext>;
};

export const useAuth = () => {
  return React.useContext(AuthContext);
};
