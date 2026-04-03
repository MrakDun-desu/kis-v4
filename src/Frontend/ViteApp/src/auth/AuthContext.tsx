import React, { useEffect, useState } from "react";
import z from "zod";
import { authEvents } from "./authEvents";

const UserClaimSchema = z.array(
  z.object({
    type: z.string(),
    value: z.union([z.string(), z.number()]),
    valueType: z.string().nullable(),
  }),
);

type UserClaims = z.infer<typeof UserClaimSchema>;

interface AuthContextType {
  userClaims: UserClaims | null;
  loading: boolean;
  signIn: () => void;
  signOut: () => void;
}

const AuthContext = React.createContext<AuthContextType>(null!);

// 20 minutes auth refresh timeout
const authRefreshTimeout = 1000 * 60 * 20;

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [userClaims, setUserClaims] = useState<UserClaims | null>(null);
  const [loading, setLoading] = useState(true);
  const [lastVisibilityChange, setLastVisibilityChange] = useState(Date.now());

  useEffect(() => {
    const periodicAuthRefresher = setInterval(async () => {
      await refreshAuth(true);
    }, authRefreshTimeout);
    const visibilityAuthRefresher = async () => {
      if (document.visibilityState === "visible") {
        const now = Date.now();
        if (lastVisibilityChange + authRefreshTimeout < now) {
          setLastVisibilityChange(now);
          await refreshAuth(true);
        }
      }
    };
    const unsubscribe = authEvents.on("unauthorized", handleUnauthorized);

    document.addEventListener("visibilitychange", visibilityAuthRefresher);

    refreshAuth();

    return () => {
      unsubscribe();
      clearInterval(periodicAuthRefresher);
      document.removeEventListener("visibilitychange", visibilityAuthRefresher);
    };
  }, []);

  const handleUnauthorized = async () => {
    const authKeys = Object.keys(sessionStorage).filter(
      (key) =>
        key.startsWith(".AspNetCore") ||
        key.startsWith("oidc") ||
        key.startsWith("bff") ||
        key.startsWith("user") ||
        key.startsWith("token") ||
        key.startsWith("auth"),
    );

    authKeys.forEach((key) => sessionStorage.removeItem(key));

    const lsAuthKeys = Object.keys(localStorage).filter(
      (key) =>
        key.startsWith(".AspNetCore") ||
        key.startsWith("oidc") ||
        key.startsWith("bff") ||
        key.startsWith("user") ||
        key.startsWith("token") ||
        key.startsWith("auth"),
    );

    lsAuthKeys.forEach((key) => localStorage.removeItem(key));

    let signOutUrl = "/bff/logout";

    try {
      await fetch(signOutUrl, {
        headers: {
          "X-CSRF": "1",
        },
        keepalive: true,
      });
    } catch {}

    setTimeout(() => {
      window.location.href = "/bff/login";
    }, 500);
  };

  const refreshAuth = async (autoRelogin: boolean = false) => {
    try {
      setLoading(true);
      const authResponse = await fetch(
        new Request("/bff/user", {
          headers: new Headers({
            "X-CSRF": "1",
          }),
          keepalive: true,
        }),
      );
      if (authResponse.ok) {
        const respValue = await authResponse.json();
        const respClaims = UserClaimSchema.parse(respValue);
        setUserClaims(respClaims);
        setLoading(false);
        return respValue;
      } else {
        if (authResponse.status === 401) {
          if (autoRelogin) {
            handleUnauthorized();
          }
        }
      }
    } catch (e) {
      alert("Nepovedlo se zjistit status přihlášení");
      console.error("Error checking user status: ", e);
    }

    setLoading(false);
    return null;
  };

  const signIn = () => {
    window.location.href = `/bff/login`;
  };

  const signOut = () => {
    let signOutUrl = "/bff/logout";
    if (userClaims) {
      const logoutUrlClaim = userClaims.find(
        (claim) => claim["type"] === "bff:logout_url",
      );
      if (logoutUrlClaim) {
        signOutUrl = logoutUrlClaim.value as string;
      }
    }

    setUserClaims(null);
    window.location.href = signOutUrl;
  };

  const value = { userClaims, signIn, signOut, loading };

  return <AuthContext value={value}>{children}</AuthContext>;
};

export const useAuth = () => {
  return React.useContext(AuthContext);
};
