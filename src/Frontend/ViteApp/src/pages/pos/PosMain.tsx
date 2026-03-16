import { ArrowBack, Cancel, Done } from "@mui/icons-material";
import { Box, Button, Paper, Typography } from "@mui/material";
import type { ReactNode } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import usePosStore from "../../stores/posStore";

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
    label: "Kegy",
    url: "containers",
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

export const PosMain = () => {
  const navigate = useNavigate();
  const transactionItems = usePosStore((state) => state.transactionItems);

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
        {links.map((l) => (
          <Button
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
          onClick={() => navigate("/admin")}
        >
          Administrace
        </Button>
      </Paper>
      <Paper
        sx={{
          flexGrow: 1,
        }}
      >
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
          flexGrow="1"
        >
          <Box display="flex" flexDirection="column">
            {transactionItems.map((sti, i) => (
              <Paper key={i} elevation={4}>
                <Box display="flex" padding={1} gap={1}>
                  {sti.amount}ks <b>{sti.saleItemName}</b>
                </Box>
                {sti.modifications?.map((mod, i) => (
                  <Box
                    key={i}
                    display="flex"
                    padding={1}
                    paddingTop={0}
                    gap={1}
                  >
                    <Box>
                      {mod.amount > 0 ? "+" : "-"}
                      {mod.amount} <b>{mod.modifierName}</b>
                    </Box>
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
            >
              Zaplatit
            </Button>
            <Button
              variant="contained"
              color="error"
              size="large"
              startIcon={<Cancel />}
              sx={{ height: "100px", fontSize: 20 }}
            >
              Zrušit objednávku
            </Button>
          </Box>
        </Box>
      </Paper>
    </Box>
  );
};
