import { Link, useParams } from "react-router-dom";
import {
  StoreTransactionsApi,
  type StoreTransactionReadResponse,
} from "../../../api-generated";
import { useEffect, useState } from "react";
import { Box, Button, Skeleton, Typography } from "@mui/material";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import { transactionReasons } from "../../../constants/transactionReasons";
import StoreTransactionItemListView from "../../../components/views/StoreTransactionItemListView";

const api = new StoreTransactionsApi(defaultConfiguration);

const StoreTransactionDetail = () => {
  const [storeTransaction, setStoreTransaction] =
    useState<StoreTransactionReadResponse | null>(null);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { id } = useParams();

  useEffect(() => {
    const getStoreTransaction = async () => {
      setStoreTransaction(null);
      const response = await handleApiCall(
        api.storeTransactionsRead({
          id: Number(id),
        }),
      );
      setStoreTransaction(response);
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
            Čas vytvoření: {storeTransaction.startedAt.toLocaleString("cs")}
          </Typography>

          <Typography>Vytvořil: {storeTransaction.startedBy.nick}</Typography>

          {storeTransaction.cancelledBy ? (
            <>
              <Typography>
                Čas zrušení:{" "}
                {storeTransaction.cancelledAt?.toLocaleString("cs")}
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
                  const resp = await handleApiCall(
                    api.storeTransactionsDelete({
                      id: storeTransaction.id,
                    }),
                  );
                  if (resp !== null) {
                    setRefreshCounter((prev) => prev + 1);
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
