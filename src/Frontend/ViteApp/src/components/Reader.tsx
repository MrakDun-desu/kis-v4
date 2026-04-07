import { Box, Typography } from "@mui/material";
import { useReaderStore } from "../stores/readerStore";
import { KisReaderClient, ReaderState } from "kis-reader-lib";
import { useEffect, useState } from "react";
import type { IKisReaderClient } from "kis-reader-lib/distES6/IClient";
import type { SocketError } from "kis-reader-lib/distES6/errors";
import { Circle } from "@mui/icons-material";

const Reader = () => {
  const {
    readerUri,
    currentCard,
    readerState,
    setCurrentCard,
    setReaderState,
  } = useReaderStore();

  useEffect(() => {
    if (!readerUri) {
      setReaderState(ReaderState.ST_DISCONNECTED);
      setCurrentCard();
      return;
    }
    const realReaderUri =
      readerUri.startsWith("ws://") || readerUri.startsWith("wss://")
        ? readerUri
        : `wss://${readerUri}`;
    console.log("Trying to connect to a reader with URL: ", realReaderUri);
    const client = new KisReaderClient(realReaderUri);
    console.log("Initial state: ", client.getState());

    const onReaderStateChange = (client: IKisReaderClient) => {
      const newState = client.getState();
      console.log("Reader state changed: ", client.getState());
      setReaderState(newState);
    };
    const onCardRead = ({
      cardData,
    }: {
      client: IKisReaderClient;
      cardData: string;
    }) => {
      console.log("Read card: ", cardData);
      setCurrentCard(cardData);
    };
    const onReaderError = ({
      client,
      error,
    }: {
      client: IKisReaderClient;
      error: any;
    }) => {
      console.log("Reader error: ", error);
      setReaderState(client.getState());
    };

    client.connectedEvent.on(onReaderStateChange);
    client.disconnectedEvent.on(onReaderStateChange);
    client.reconnectingEvent.on(onReaderStateChange);
    client.errorEvent.on(onReaderError);
    client.cardReadEvent.on(onCardRead);

    const connectTimeout = setTimeout(client.connect, 200);

    return () => {
      clearTimeout(connectTimeout);
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
