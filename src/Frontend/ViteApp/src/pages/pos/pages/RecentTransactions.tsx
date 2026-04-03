import { useEffect, useState } from "react";
import type { SaleTransactionListModel } from "../../../api/apiTypes";
import { Box, Typography, Skeleton, Paper, Button } from "@mui/material";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const RecentTransactions = () => {
  const [transactions, setTransactions] =
    useState<SaleTransactionListModel[]>();

  useEffect(() => {
    const getTransactions = async () => {
      const { response, data, error } = await apiClient.GET(
        "/sale-transactions",
        {
          params: { query: { OnlySelfCancellable: true, PageSize: 100 } },
        },
      );

      if (!response.ok) {
        handleApiError(response, error);
      }
      setTransactions(data?.data);
    };
    getTransactions();
  }, []);

  if (!transactions) {
    return (
      <Box padding={1} display="flex" flexDirection="column" gap={2}>
        <Typography variant="h5" component="h2" marginBottom={3}>
          Nedávné transakce
        </Typography>
        <Skeleton variant="rounded" width="100%" height="5em" />
        <Skeleton variant="rounded" width="100%" height="5em" />
        <Skeleton variant="rounded" width="100%" height="5em" />
      </Box>
    );
  }

  return (
    <Box padding={1} flex="1" minHeight={0} overflow="auto">
      <Typography variant="h5" component="h2" marginBottom={3}>
        Nedávné transakce
      </Typography>

      {transactions.length === 0 ? (
        <Typography>Žádné nedávné transakce</Typography>
      ) : (
        <Box
          display="flex"
          flexDirection="column"
          gap={2}
          overflow="auto"
          flex={1}
        >
          {transactions.map((transaction) => (
            <Paper key={transaction.id} elevation={4} sx={{ padding: 2 }}>
              <Box
                display="flex"
                justifyContent="space-between"
                alignItems="center"
              >
                <Box display="flex" flexDirection="column" gap={1}>
                  <Typography fontWeight="bold" fontSize="large">
                    Transakce {transaction.id}
                  </Typography>

                  <Typography>
                    Čas vytvoření:{" "}
                    {new Date(transaction.startedAt).toLocaleTimeString("cs")}
                  </Typography>
                </Box>

                <Box display="flex" gap={1}>
                  <Button
                    variant="contained"
                    size="large"
                    color="error"
                    onClick={async () => {
                      const confirmed = confirm(
                        `Opravdu chcete prodejní transakci ${transaction.id} zrušit?`,
                      );
                      if (!confirmed) {
                        return;
                      }
                      const { response, error } = await apiClient.DELETE(
                        "/sale-transactions/{id}",
                        {
                          params: { path: { id: transaction.id } },
                        },
                      );

                      if (!response.ok) {
                        handleApiError(response, error);
                      } else {
                        setTransactions((prev) =>
                          prev?.filter((t) => t.id != transaction.id),
                        );
                      }
                    }}
                  >
                    Zrušit
                  </Button>
                </Box>
              </Box>
            </Paper>
          ))}
        </Box>
      )}
    </Box>
  );
};

export default RecentTransactions;
