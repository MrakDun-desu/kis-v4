import { KisReaderClient, ReaderState } from "kis-reader-lib";
import { createContext, useContext, useEffect, useRef, useState } from "react";
import type { IKisReaderClient } from "kis-reader-lib/distES6/IClient";
import { readerStates } from "../constants/readerStates";
import type { Disposable } from "kis-reader-lib/distES6/TypedEvent";
import { useLoading } from "./LoadingContext";
import type { UserDetails } from "../auth/AuthContext";

export type CardReadEvent =
  | {
      hasUser: false;
      cardData: string;
    }
  | {
      hasUser: true;
      cardData: string;
      userData: UserDetails;
    };

export type ExternalReaderState =
  | "disconnected"
  | "connecting"
  | "idle"
  | "reading";

type ReaderContextType = {
  readerUri?: string;
  readerState: ExternalReaderState;
  setReaderUri: (val?: string) => void;
  addReadListener: (fn: (userData: CardReadEvent) => void) => () => void;
  requestCard: () => void;
};

const ReaderContext = createContext<ReaderContextType>(null!);

const readerUriLsKey = "readerUri";

const ReaderProvider = ({ children }: { children: React.ReactNode }) => {
  const [readerUri, setReaderUri] = useState(
    () => localStorage.getItem(readerUriLsKey) ?? undefined,
  );
  const [client, setClient] = useState<KisReaderClient>();
  const [readerState, setReaderState] =
    useState<ExternalReaderState>("disconnected");
  const cardReadListeners = useRef(
    new Set<(userData: CardReadEvent) => void>(),
  );
  const { startLoading, stopLoading } = useLoading();

  useEffect(() => {
    if (!readerUri) {
      setClient(undefined);
      return;
    }
    if (client && readerUri === client.url) {
      return;
    }
    if (client) {
      client.disconnect();
    }
    const realReaderUri =
      readerUri.startsWith("ws://") || readerUri.startsWith("wss://")
        ? readerUri
        : `wss://${readerUri}`;
    const newClient = new KisReaderClient(realReaderUri);

    const onCardRead = async ({
      cardData,
    }: {
      client: IKisReaderClient;
      cardData: string;
    }) => {
      startLoading();
      try {
        const authResponse = await fetch(
          import.meta.env.BASE_URL +
            `/auth/legacy/user_for_rfid?cardId=${cardData}`,
          {
            credentials: "include",
            headers: { "X-CSRF": "1" },
          },
        );
        if (!authResponse.ok) {
          const evt: CardReadEvent = { hasUser: false, cardData: cardData };
          cardReadListeners.current.forEach((fn) => fn(evt));
        } else {
          const body = await authResponse.json();
          const evt: CardReadEvent = {
            hasUser: true,
            cardData: cardData,
            userData: {
              nick: body["nick"],
              gamification: body["gam"],
              userId: body["sub"],
            },
          };
          cardReadListeners.current.forEach((fn) => fn(evt));
        }
      } catch (err) {
        console.error("Chyba zjišťování uživatele pro kartu " + cardData, err);
      }

      setReaderState("idle");
      stopLoading();
    };
    const onReaderError = ({
      error,
    }: {
      client: IKisReaderClient;
      error: any;
    }) => {
      console.error("Reader error: ", error);
    };

    const listeners: Disposable[] = [];

    const onReaderStateChange = (activeClient: IKisReaderClient) => {
      const state = activeClient.getState();
      const externalState: ExternalReaderState =
        state === ReaderState.ST_IDLE
          ? "idle"
          : state === ReaderState.ST_RECONNECTING
            ? "connecting"
            : state === ReaderState.ST_SINGLE_READ
              ? "reading"
              : "disconnected";

      setReaderState(externalState);
      if (import.meta.env.DEV) {
        console.log(
          "Reader state changed: ",
          readerStates[activeClient.getState()],
        );
      }
    };
    listeners.push(newClient.connectedEvent.on(onReaderStateChange));
    listeners.push(newClient.disconnectedEvent.on(onReaderStateChange));
    listeners.push(newClient.reconnectingEvent.on(onReaderStateChange));
    listeners.push(newClient.errorEvent.on(onReaderError));
    listeners.push(newClient.cardReadEvent.on(onCardRead));

    const connectToReader = () => {
      try {
        newClient.connect();
      } catch (err) {
        console.error("Error connecting to client:", err);
        setClient(undefined);
        return;
      }
      setClient(newClient);
    };
    const connectTimeout = setTimeout(connectToReader, 200);

    return () => {
      clearTimeout(connectTimeout);
      listeners.forEach((l) => l.dispose());
      client?.disconnect();
    };
  }, [readerUri]);

  const addReadListener = (fn: (userData: CardReadEvent) => void) => {
    cardReadListeners.current.add(fn);
    return () => cardReadListeners.current.delete(fn);
  };

  return (
    <ReaderContext.Provider
      value={{
        addReadListener,
        readerState,
        readerUri,
        setReaderUri: (val) => {
          if (val !== undefined) {
            localStorage.setItem(readerUriLsKey, val);
          }
          setReaderUri(val);
        },
        requestCard: () => {
          if (client !== undefined && client.isReady()) {
            client.modeSingleRead();
            setReaderState("reading");
          }
        },
      }}
    >
      {children}
    </ReaderContext.Provider>
  );
};

export function useReader() {
  const ctx = useContext(ReaderContext);
  if (!ctx) {
    throw new Error("useReader must be used within a ReaderProvider");
  }
  return ctx;
}

export default ReaderProvider;
