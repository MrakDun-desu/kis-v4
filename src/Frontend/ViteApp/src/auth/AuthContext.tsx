import React, { useEffect, useState } from "react";
import z from "zod";
import { authEvents } from "./authEvents";
import { redirect } from "../helpers";

const UserClaimSchema = z.array(
  z.object({
    type: z.string(),
    value: z.union([z.string(), z.number()]),
  }),
);

type UserClaims = z.infer<typeof UserClaimSchema>;
export type UserDetails = {
  nick: string;
  gamification: boolean;
  userId: string;
};

interface AuthContextType {
  userClaims?: UserClaims;
  userDetails?: UserDetails;
  loading: boolean;
  signIn: () => void;
  signOut: () => void;
}

const AuthContext = React.createContext<AuthContextType>(null!);

// 20 minutes auth refresh timeout
const authRefreshTimeout = 1000 * 60 * 20;

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [userClaims, setUserClaims] = useState<UserClaims>();
  const [userDetails, setUserDetails] = useState<UserDetails>();
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
          await refreshAuth(true);
        }
        setLastVisibilityChange(now);
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

    alert("Vypršelo přihlášení. Budete přesměrování na stránku odhlášení.");

    signOut();
  };

  const refreshAuth = async (autoRelogin: boolean = false) => {
    try {
      setLoading(true);
      const authResponse = await fetch(
        new Request(import.meta.env.BASE_URL + "/bff/user", {
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
        setUserDetails({
          gamification:
            respClaims
              .find((val) => val.type === "gam")
              ?.value.toString()
              .toLowerCase() === "true",
          nick: respClaims.find((val) => val.type === "nick")?.value as string,
          userId: respClaims.find((val) => val.type === "sub")?.value as string,
        });
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
    redirect("/bff/login");
  };

  const signOut = () => {
    const signOutUrl =
      (userClaims?.find((claim) => claim.type === "bff:logout_url")
        ?.value as string) ?? import.meta.env.BASE_URL + "/bff/logout";

    setUserClaims(undefined);
    setUserDetails(undefined);
    window.location.href = signOutUrl;
  };

  const value = { userClaims, signIn, signOut, loading, userDetails };

  return <AuthContext value={value}>{children}</AuthContext>;
};

export const useAuth = () => {
  return React.useContext(AuthContext);
};
