import { usePosStore } from "../stores/posStore";
import { useReader } from "../contexts/ReaderContext";
import { useEffect } from "react";
import { Box, Typography } from "@mui/material";
import { Circle } from "@mui/icons-material";

const CustomerChecker = () => {
  const customerData = usePosStore((state) => state.customerData);
  const setCustomerData = usePosStore((state) => state.setCustomerData);
  const { addReadListener, requestCard, readerState } = useReader();

  useEffect(() => {
    const removeListener = addReadListener((evt) => {
      if (customerData !== undefined) {
        return;
      }
      if (evt.hasUser) {
        setCustomerData(evt.userData);
      } else {
        requestCard();
      }
    });

    return removeListener;
  }, []);

  useEffect(() => {
    if (customerData === undefined && readerState === "idle") {
      requestCard();
    }
  }, [customerData, readerState]);

  return (
    <Box display="flex" gap={1}>
      <Circle
        color={
          readerState === "disconnected"
            ? "error"
            : readerState === "connecting"
              ? "warning"
              : readerState === "idle"
                ? "primary"
                : "success"
        }
      />
      {customerData !== undefined ? (
        <Typography>Zákazník: {customerData?.nick}</Typography>
      ) : readerState === "disconnected" ? (
        "Čtečka odpojena"
      ) : readerState === "connecting" ? (
        "Připájení..."
      ) : (
        readerState === "reading" && "Čekám na kartu..."
      )}
    </Box>
  );
};

export default CustomerChecker;
