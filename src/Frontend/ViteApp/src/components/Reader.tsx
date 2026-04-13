import { Box, Typography } from "@mui/material";
import { useReaderStore } from "../stores/readerStore";
import { KisReaderClient, ReaderState } from "kis-reader-lib";
import { useEffect, useState } from "react";
import type { IKisReaderClient } from "kis-reader-lib/distES6/IClient";
import type { SocketError } from "kis-reader-lib/distES6/errors";
import { Circle } from "@mui/icons-material";

const readerStates: Record<ReaderState, string> = {
  "0": "Disconnected",
  "1": "Reconnecting",
  "2": "Idle",
  "3": "Auto read",
  "4": "Single read",
  "5": "Unknown",
};

const Reader = () => {
  const {
    readerUri,
    currentCard,
    readerState,
    setCurrentCard,
    setReaderState,
  } = useReaderStore();
  const [client, setClient] = useState<KisReaderClient>();
  useEffect(() => {
    if (!readerUri) {
      setReaderState(ReaderState.ST_DISCONNECTED);
      setCurrentCard();
      setClient(undefined);
      return;
    }
    if (client) {
      client.disconnect();
    }
    const realReaderUri =
      readerUri.startsWith("ws://") || readerUri.startsWith("wss://")
        ? readerUri
        : `wss://${readerUri}`;
    console.log("Trying to connect to a reader with URL: ", realReaderUri);
    const newClient = new KisReaderClient(realReaderUri);
    console.log("Url passed to reader: ", newClient.url);

    const onReaderStateChange = (activeClient: IKisReaderClient) => {
      const newState = activeClient.getState();
      console.log(
        "Reader state changed: ",
        readerStates[activeClient.getState()],
      );
      setReaderState(newState);
      if (newState === ReaderState.ST_IDLE) {
        activeClient.modeSingleRead();
      }
    };
    const onCardRead = ({
      client: activeClient,
      cardData,
    }: {
      client: IKisReaderClient;
      cardData: string;
    }) => {
      console.log("Read card: ", cardData);
      setCurrentCard(cardData);
      activeClient.modeSingleRead();
    };
    const onReaderError = ({
      client: activeClient,
      error,
    }: {
      client: IKisReaderClient;
      error: any;
    }) => {
      console.error("Reader error: ", error);
      setReaderState(activeClient.getState());
    };

    newClient.connectedEvent.on(onReaderStateChange);
    newClient.disconnectedEvent.on(onReaderStateChange);
    newClient.reconnectingEvent.on(onReaderStateChange);
    newClient.errorEvent.on(onReaderError);
    newClient.cardReadEvent.on(onCardRead);

    const connectToReader = () => {
      try {
        newClient.connect();
      } catch (err) {
        console.error("Error connecting to client:", err);
        setClient(undefined);
        return;
      }
      setClient(newClient);
      setTimeout(() => {
        if (!client) {
          return;
        }

        client.modeSingleRead();
      }, 500);
    };
    const connectTimeout = setTimeout(connectToReader, 200);

    return () => {
      clearTimeout(connectTimeout);
      if (!client) {
        return;
      }
      client.connectedEvent.off(onReaderStateChange);
      client.disconnectedEvent.off(onReaderStateChange);
      client.reconnectingEvent.off(onReaderStateChange);
      client.cardReadEvent.off(onCardRead);
      client.cardReadEvent.off(onCardRead);
      client.disconnect();
    };
  }, [readerUri]);

  if (!readerUri) {
    return <Typography variant="h6">Nenastaveno URL čtečky</Typography>;
  }

  return (
    <Box display="flex" gap={2}>
      {(readerState === ReaderState.ST_AUTO_READ ||
        readerState === ReaderState.ST_SINGLE_READ ||
        readerState === ReaderState.ST_IDLE) && (
        <>
          <Circle color="success" />
          <Typography>Čtečka připojena</Typography>
          {currentCard}
        </>
      )}
      {(readerState === ReaderState.ST_DISCONNECTED ||
        readerState === ReaderState.ST_UNKNOWN) && (
        <>
          <Circle color="error" />
          <Typography>Čtečka nepřipojena</Typography>
        </>
      )}
      {readerState === ReaderState.ST_RECONNECTING && (
        <>
          <Circle color="warning" />
          <Typography>Čtečka se připájí...</Typography>
        </>
      )}
    </Box>
  );
};

export default Reader;
