import {
  AccountCircle,
  ArrowBack,
  ArrowUpward,
  Cancel,
  Done,
  GridView,
  Logout,
  OpenWith,
  PlayForWork,
  PointOfSale,
  Replay,
  Store,
} from "@mui/icons-material";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  Paper,
  Typography,
} from "@mui/material";
import { useEffect, useState, type ReactNode } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import { usePosStore } from "../../stores/posStore";
import { useShallow } from "zustand/react/shallow";
import OrderFinishForm from "../../components/forms/OrderFinishForm";
import { useAuth } from "../../auth/AuthContext";
import CustomerChecker from "../../components/CustomerChecker";

interface Link {
  label: string;
  url: string;
  icon?: ReactNode;
}

const links: Link[] = [
  {
    label: "Objednávky",
    url: "orders",
  },
  {
    label: "Nedávné transakce",
    url: "recent-transactions",
  },
  {
    label: "Párování karty",
    url: "card-pairing",
  },
  {
    label: "Nastavení",
    url: "settings",
  },
];

const PosPage = () => {
  const navigate = useNavigate();
  const {
    transactionItems,
    currentStore,
    currentCashBox,
    currentLayout,
    layoutHistory,
    customerData,
    clearTransactionItems,
    popLayoutHistory,
    setLayoutId,
    setCustomerData,
    setSellForFree,
  } = usePosStore(
    useShallow((state) => ({
      transactionItems: state.transactionItems,
      currentStore: state.currentStore,
      currentCashBox: state.currentCashBox,
      currentLayout: state.currentLayout,
      layoutHistory: state.layoutHistory,
      customerData: state.customerData,
      clearTransactionItems: state.clearTransactionItems,
      popLayoutHistory: state.popLayoutHistory,
      setLayoutId: state.setLayoutId,
      setCustomerData: state.setCustomerData,
      setSellForFree: state.setSellForFree,
    })),
  );
  const [finishingOrder, setFinishingOrder] = useState(false);
  const [time, setTime] = useState(new Date());
  const auth = useAuth();

  useEffect(() => {
    const interval = setInterval(() => {
      setTime(new Date());
    }, 1000);

    return () => clearInterval(interval);
  }, []);

  return (
    <Box display="flex" gap={1} padding={1} width="100vw" height="100vh">
      <Paper
        sx={{
          display: "flex",
          gap: 1,
          flexDirection: "column",
          alignItems: "center",
          width: "300px",
          padding: 1,
        }}
      >
        {layoutHistory.length > 1 && (
          <Button
            variant="outlined"
            size="large"
            sx={{
              height: "5rem",
            }}
            startIcon={<ArrowUpward />}
            onClick={() => {
              console.log(layoutHistory);
              const prevLayoutId = layoutHistory[layoutHistory.length - 2];
              popLayoutHistory();
              setLayoutId(prevLayoutId);
            }}
          >
            Předchozí rozložení
          </Button>
        )}
        {links.map((l) => (
          <Button
            key={l.url}
            variant="contained"
            size="large"
            sx={{
              width: "100%",
              height: "10rem",
              fontSize: 20,
            }}
            onClick={() => navigate(l.url)}
          >
            {l.label}
          </Button>
        ))}
        <Box flexGrow={1} />
        <Button
          startIcon={<ArrowBack />}
          size="large"
          onClick={() => navigate("/admin/store-items")}
        >
          Administrace
        </Button>
      </Paper>

      <Paper
        sx={{
          flexGrow: 1,
          display: "flex",
          flexDirection: "column",
          overflow: "hidden",
        }}
      >
        <Box display="flex" gap={2} padding={1} flexWrap="wrap" flexShrink={0}>
          <Paper
            elevation={4}
            sx={{
              padding: "0.5em 1em",
              display: "flex",
              alignItems: "center",
              gap: "0.5em",
            }}
          >
            <Store /> Aktivní sklad: {currentStore?.name ?? "Nenastaven"}
          </Paper>

          <Paper
            elevation={4}
            sx={{
              padding: "0.5em 1em",
              display: "flex",
              alignItems: "center",
              gap: "0.5em",
            }}
          >
            <PointOfSale /> Aktivní kasa:{" "}
            {currentCashBox?.name ?? "Nenastavena"}
          </Paper>

          <Paper
            elevation={4}
            sx={{
              padding: "0.5em 1em",
              display: "flex",
              alignItems: "center",
              gap: "0.5em",
            }}
          >
            <GridView />{" "}
            <div>Rozložení: {currentLayout?.name ?? "Nenastaveno"}</div>
          </Paper>
        </Box>
        <Outlet />
      </Paper>

      <Paper
        sx={{
          width: "400px",
          padding: 1,
          display: "flex",
          flexDirection: "column",
          gap: 2,
        }}
      >
        <Box display="flex" flexDirection="column" gap={1}>
          <Box display="flex" alignItems="center" gap={1}>
            <Typography fontWeight="bold" fontSize={22}>
              {time.toLocaleTimeString("cs")}
            </Typography>

            <Box flex="1" />

            <AccountCircle />
            <Typography fontWeight="bold" fontSize={20}>
              {auth.userDetails?.nick}
            </Typography>
          </Box>

          <Box display="flex" gap={1}>
            <IconButton
              size="large"
              sx={{ border: "1px solid" }}
              onClick={auth.signOut}
            >
              <Logout />
            </IconButton>

            <IconButton
              size="large"
              sx={{ border: "1px solid" }}
              onClick={() => {
                setCustomerData(auth.userDetails);
              }}
            >
              <PlayForWork />
            </IconButton>

            <IconButton
              size="large"
              sx={{ border: "1px solid" }}
              onClick={async () => {
                try {
                  if (document.fullscreenElement !== null) {
                    document.exitFullscreen();
                  } else {
                    document.documentElement.requestFullscreen();
                  }
                } catch (err) {
                  console.error(err);
                }
              }}
            >
              <OpenWith />
            </IconButton>
          </Box>

          <Typography variant="h5" component="h2">
            Objednávka
          </Typography>

          <CustomerChecker />
        </Box>

        <Box
          display="flex"
          flexDirection="column"
          justifyContent="space-between"
          flex="1"
          minHeight="0"
        >
          <Box
            display="flex"
            flexDirection="column"
            gap={1}
            flex="1"
            minHeight="0"
            overflow="auto"
            marginBottom="1em"
          >
            {transactionItems.map((sti, i) => (
              <Paper key={i} elevation={4}>
                <Box
                  display="flex"
                  padding={1}
                  justifyContent="space-between"
                  alignItems="center"
                  gap={2}
                >
                  <Typography>
                    {sti.amount}ks <b>{sti.saleItemName}</b>
                  </Typography>
                </Box>
                {sti.modifications?.map((mod, i) => (
                  <Box
                    key={i}
                    display="flex"
                    padding={1}
                    paddingTop={0}
                    gap={1}
                  >
                    <Typography>
                      +{mod.amount} <b>{mod.modifierName}</b>
                    </Typography>
                  </Box>
                ))}
              </Paper>
            ))}
          </Box>

          <Box display="flex" flexDirection="column" gap={2}>
            <Button
              variant="contained"
              color="success"
              size="large"
              startIcon={<Done />}
              sx={{ height: "100px", fontSize: 20 }}
              disabled={
                transactionItems.length === 0 || customerData === undefined
              }
              onClick={() => {
                if (!currentStore || !currentCashBox) {
                  alert(
                    "Pro dokončení objednávky musíte nastavit kasu a sklad!",
                  );
                } else {
                  setFinishingOrder(true);
                }
              }}
            >
              Dokončit objednávku
            </Button>

            <Button
              variant="contained"
              size="large"
              startIcon={<Replay />}
              sx={{ height: "70px", fontSize: 20 }}
              disabled={customerData === undefined}
              onClick={() => {
                setCustomerData();
              }}
            >
              Změnit zákazníka
            </Button>

            <Button
              variant="contained"
              color="error"
              size="large"
              startIcon={<Cancel />}
              sx={{ height: "70px", fontSize: 20 }}
              disabled={transactionItems.length === 0}
              onClick={() => {
                clearTransactionItems();
                setCustomerData();
              }}
            >
              Zrušit objednávku
            </Button>
          </Box>
        </Box>
      </Paper>

      <Dialog fullScreen open={finishingOrder}>
        <DialogTitle>Dokončení objednávky</DialogTitle>

        <DialogContent>
          <OrderFinishForm
            formId="orderFinishForm"
            afterSubmit={() => setFinishingOrder(false)}
          />
        </DialogContent>

        <DialogActions>
          <Button
            size="large"
            variant="contained"
            color="error"
            sx={{ fontSize: "20px", padding: "1em 2em" }}
            onClick={() => setFinishingOrder(false)}
          >
            Zrušit
          </Button>

          <Button
            type="submit"
            form="orderFinishForm"
            color="warning"
            variant="contained"
            sx={{ fontSize: "20px", padding: "1em 2em" }}
            onClick={(evt) => {
              const confirmed = confirm(
                "Opravdu chcete tuto objednávku prodat zadarmo?",
              );
              if (!confirmed) {
                evt.preventDefault();
                return;
              }
              setSellForFree(true);
            }}
          >
            Prodat zadarmo
          </Button>

          <Button
            type="submit"
            form="orderFinishForm"
            variant="contained"
            sx={{ fontSize: "20px", padding: "1em 2em" }}
          >
            Prodat
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default PosPage;
