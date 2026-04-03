import { Link, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Box, Button, Skeleton, Typography } from "@mui/material";
import { transactionReasons } from "../../../constants/transactionReasons";
import StoreTransactionItemListView from "../../../components/views/StoreTransactionItemListView";
import type { StoreTransactionReadResponse } from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const StoreTransactionDetail = () => {
  const [storeTransaction, setStoreTransaction] =
    useState<StoreTransactionReadResponse>();
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { id } = useParams();

  useEffect(() => {
    const getStoreTransaction = async () => {
      setStoreTransaction(undefined);
      const { response, data } = await apiClient.GET(
        "/store-transactions/{id}",
        {
          params: { path: { id: Number(id) } },
        },
      );
      setStoreTransaction(data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getStoreTransaction();
  }, [refreshCounter]);

  if (!storeTransaction) {
    return (
      <>
        <Skeleton variant="rounded" width={300} height={30} />
        <Box display="flex" gap={5} marginTop={5}>
          <Box display="flex" flexDirection="column" gap={2}>
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
          </Box>
          <Box display="flex" flexDirection="column" gap={2}>
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
          </Box>
        </Box>
      </>
    );
  }

  return (
    <>
      <h2>Detail skladové transakce {storeTransaction.id}</h2>
      <Box display="flex" gap={5}>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
          minWidth={300}
        >
          <Typography>
            Důvod: {transactionReasons[storeTransaction.reason]}
          </Typography>

          {storeTransaction.saleTransactionId && (
            <Typography>
              Součást{" "}
              <Link
                to={`/admin/sale-transactions/${storeTransaction.saleTransactionId}`}
              >
                prodejní transakce {storeTransaction.saleTransactionId}
              </Link>
            </Typography>
          )}

          <Box>
            <Typography>Poznámka:</Typography>
            <Typography>
              {storeTransaction.note || <i>bez poznámky</i>}
            </Typography>
          </Box>

          <Typography>
            Čas vytvoření:{" "}
            {new Date(storeTransaction.startedAt).toLocaleString("cs")}
          </Typography>

          <Typography>Vytvořil: {storeTransaction.startedBy.nick}</Typography>

          {storeTransaction.cancelledBy && storeTransaction.cancelledAt ? (
            <>
              <Typography>
                Čas zrušení:{" "}
                {new Date(storeTransaction.cancelledAt).toLocaleString("cs")}
              </Typography>

              <Typography>
                Zrušil: {storeTransaction.cancelledBy.nick}
              </Typography>
            </>
          ) : (
            <Button
              variant="outlined"
              color="error"
              onClick={async () => {
                const confirmed = confirm(
                  `Opravdu chcete transakci ${storeTransaction.id} smazat?`,
                );
                if (confirmed) {
                  const { response } = await apiClient.DELETE(
                    "/store-transactions/{id}",
                    {
                      params: { path: { id: storeTransaction.id } },
                    },
                  );
                  if (response.ok) {
                    setRefreshCounter((prev) => prev + 1);
                  } else {
                    handleApiError(response);
                  }
                }
              }}
            >
              Zrušit transakci
            </Button>
          )}

          <h3>Položky v transakci</h3>

          <StoreTransactionItemListView
            transactionItems={storeTransaction.storeTransactionItems}
            transactionReason={storeTransaction.reason}
          />
        </Box>
      </Box>
    </>
  );
};

export default StoreTransactionDetail;
