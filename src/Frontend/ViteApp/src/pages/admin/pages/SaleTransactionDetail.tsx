import { Link, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Box, Button, Skeleton, Typography } from "@mui/material";
import SaleTransactionItemListView from "../../../components/views/SaleTransactionItemListView";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";
import AccountTransactionListView from "../../../components/views/AccountTransactionListView";
import type { SaleTransactionReadResponse } from "../../../api/apiTypes";

const SaleTransactionDetail = () => {
  const [saleTransaction, setSaleTransaction] =
    useState<SaleTransactionReadResponse>();
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { id } = useParams();

  useEffect(() => {
    const getSaleTransaction = async () => {
      setSaleTransaction(undefined);
      const { data, response } = await apiClient.GET(
        "/sale-transactions/{id}",
        { params: { path: { id: Number(id) } } },
      );
      setSaleTransaction(data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getSaleTransaction();
  }, [refreshCounter]);

  if (!saleTransaction) {
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
      <h2>Detail prodejní transakce {saleTransaction.id}</h2>
      <Box display="flex" gap={5}>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
          minWidth={300}
        >
          <Box>
            <Typography>Poznámka:</Typography>
            <Typography>
              {saleTransaction.note || <i>bez poznámky</i>}
            </Typography>
          </Box>

          <Typography>
            Čas vytvoření:{" "}
            {new Date(saleTransaction.startedAt).toLocaleString("cs")}
          </Typography>

          <Typography>
            Součástí jsou skladové transakce:{" "}
            {saleTransaction.storeTransactions.map(
              (storeTransaction, i, arr) => (
                <>
                  <Link
                    key={storeTransaction.id}
                    to={`/admin/store-transactions/${storeTransaction.id}`}
                  >
                    {storeTransaction.id}
                  </Link>
                  {i !== arr.length - 1 && ", "}
                </>
              ),
            )}
          </Typography>

          <Typography>Vytvořil: {saleTransaction.startedBy.nick}</Typography>

          {saleTransaction.openedBy && (
            <Typography>
              Transakce je otevřená pro {saleTransaction.openedBy.nick}
            </Typography>
          )}

          {saleTransaction.cancelledBy && saleTransaction.cancelledAt ? (
            <>
              <Typography>
                Čas zrušení:{" "}
                {new Date(saleTransaction.cancelledAt).toLocaleString("cs")}
              </Typography>

              <Typography>
                Zrušil: {saleTransaction.cancelledBy.nick}
              </Typography>
            </>
          ) : (
            <Button
              variant="outlined"
              color="error"
              onClick={async () => {
                const confirmed = confirm(
                  `Opravdu chcete transakci ${saleTransaction.id} smazat?`,
                );
                if (confirmed) {
                  const { response } = await apiClient.DELETE(
                    "/sale-transactions/{id}",
                    {
                      params: {
                        path: { id: saleTransaction.id },
                      },
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

          <SaleTransactionItemListView
            transactionItems={saleTransaction.saleTransactionItems}
          />

          <h3>Změny na účtech</h3>

          <AccountTransactionListView
            accountTransactions={saleTransaction.accountTransactions}
          />
        </Box>
      </Box>
    </>
  );
};

export default SaleTransactionDetail;
