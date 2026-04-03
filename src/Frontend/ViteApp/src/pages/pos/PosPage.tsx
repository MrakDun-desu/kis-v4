import {
  ArrowBack,
  ArrowUpward,
  Cancel,
  Done,
  GridView,
  PointOfSale,
  Store,
} from "@mui/icons-material";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Paper,
  Typography,
} from "@mui/material";
import { useState, type ReactNode } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import { usePosStore } from "../../stores/posStore";
import { useShallow } from "zustand/react/shallow";
import OrderFinishForm from "../../components/forms/OrderFinishForm";

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
  // {
  //   label: "Nedávné transakce",
  //   url: "recent-transactions",
  // },
  {
    label: "Kegy",
    url: "containers",
  },
  // {
  //   label: "Párování karty",
  //   url: "card-pairing",
  // },
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
    clearTransactionItems,
    popLayoutHistory,
    setLayoutId,
    removeTransactionItem,
    updateTransactionItem,
  } = usePosStore(
    useShallow((state) => ({
      transactionItems: state.transactionItems,
      currentStore: state.currentStore,
      currentCashBox: state.currentCashBox,
      currentLayout: state.currentLayout,
      layoutHistory: state.layoutHistory,
      clearTransactionItems: state.clearTransactionItems,
      popLayoutHistory: state.popLayoutHistory,
      setLayoutId: state.setLayoutId,
      removeTransactionItem: state.removeTransactionItem,
      updateTransactionItem: state.updateTransactionItem,
    })),
  );
  const [finishingOrder, setFinishingOrder] = useState(false);

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
        <Typography variant="h5" component="h2" marginTop={1}>
          Aktuální objednávka
        </Typography>

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

                  <Button
                    color="error"
                    variant="contained"
                    sx={{
                      minWidth: 0,
                    }}
                    onClick={() => {
                      if (sti.amount > 1) {
                        updateTransactionItem(i, { amount: sti.amount - 1 });
                      } else {
                        removeTransactionItem(i);
                      }
                    }}
                  >
                    -1
                  </Button>
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
              disabled={transactionItems.length === 0}
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
              color="error"
              size="large"
              startIcon={<Cancel />}
              sx={{ height: "100px", fontSize: 20 }}
              disabled={transactionItems.length === 0}
              onClick={() => {
                clearTransactionItems();
              }}
            >
              Zrušit objednávku
            </Button>
          </Box>
        </Box>
      </Paper>

      <Dialog open={finishingOrder}>
        <DialogTitle>Dokončení objednávky</DialogTitle>
        <DialogContent>
          <OrderFinishForm
            formId="orderFinishForm"
            transactionItems={transactionItems}
            afterSubmit={() => setFinishingOrder(false)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setFinishingOrder(false)}>Zrušit</Button>
          <Button type="submit" form="orderFinishForm">
            Dokončit
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default PosPage;
