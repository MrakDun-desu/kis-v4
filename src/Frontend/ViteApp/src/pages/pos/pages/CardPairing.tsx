import { Box, Button, Typography } from "@mui/material";
import { useReader } from "../../../contexts/ReaderContext";
import { useEffect, useState } from "react";
import { useLoading } from "../../../contexts/LoadingContext";
import { useSnackbar } from "../../../contexts/SnackbarContext";

const CardPairing = () => {
  const { readerState, requestCard, addReadListener } = useReader();
  const [pairingId, setPairingId] = useState<string>();
  const [newCard, setNewCard] = useState<boolean>();
  const { startLoading, stopLoading } = useLoading();
  const { showSnackbar } = useSnackbar();

  useEffect(() => {
    const removeListener = addReadListener(async (evt) => {
      setNewCard(!evt.hasUser);
      if (evt.hasUser) {
        requestCard();
        return;
      }
      startLoading();
      const pairingResponse = await fetch(
        import.meta.env.BASE_URL + `/auth/users/rfids/pool`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "X-CSRF": "1",
            "Content-Type": "application/json",
          },
          body: JSON.stringify(evt.cardData),
        },
      );
      stopLoading();
      if (pairingResponse.ok) {
        const pairingResponseData = await pairingResponse.json();
        setPairingId(pairingResponseData);
      }
    });
    requestCard();

    return removeListener;
  }, []);

  return (
    <Box
      padding={1}
      flex="1"
      minHeight={0}
      overflow="auto"
      display="flex"
      flexDirection="column"
      alignItems="stretch"
      gap={3}
    >
      <Typography variant="h5" component="h2" marginBottom={3}>
        Párování karty
      </Typography>

      {readerState === "disconnected" && (
        <Typography variant="h3" textAlign="center">
          Čtečka nepřipojena
        </Typography>
      )}
      {readerState === "connecting" && (
        <Typography variant="h3" textAlign="center">
          Čtečka se připájí...
        </Typography>
      )}
      {readerState === "reading" && (
        <Typography variant="h3" textAlign="center">
          Čekám na kartu...
        </Typography>
      )}
      {readerState === "idle" && pairingId !== undefined && (
        <>
          <Typography variant="h4" textAlign="center">
            Kód pro zákazníka:
          </Typography>

          <Typography variant="h3" textAlign="center">
            {pairingId}
          </Typography>

          <Button
            variant="outlined"
            size="large"
            sx={{ alignSelf: "center", fontSize: 20 }}
            onClick={async () => {
              if (pairingId === undefined) {
                return;
              }
              startLoading();
              try {
                const res = await fetch(
                  `/food/pairing_code/${pairingId}/print`,
                  {
                    method: "POST",
                    headers: {
                      "X-CSRF": "1",
                    },
                  },
                );

                if (res.ok) {
                  showSnackbar("Požadavka na tištení přijata", "success");
                } else {
                  showSnackbar("Chyba v požadavku");
                }
              } catch (err) {
                showSnackbar("Chyba v požadavku");
                console.error(err);
              }
              stopLoading();
            }}
          >
            Vytiskni kód
          </Button>

          <Button
            variant="outlined"
            size="large"
            sx={{ alignSelf: "center", fontSize: 20 }}
            onClick={() => {
              setPairingId(undefined);
              setNewCard(undefined);
              requestCard();
            }}
          >
            Načti jinou kartu
          </Button>
        </>
      )}
      {newCard === false && (
        <Typography variant="h4" textAlign="center">
          Načtená karta už má přiřazeného vlastníka
        </Typography>
      )}
    </Box>
  );
};

export default CardPairing;
