import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Box, Skeleton, Typography } from "@mui/material";
import type { CashBoxReadResponse } from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";
import AccountTransactionPagedView from "../../../components/views/AccountTransactionPagedView";

const CashBoxDetail = () => {
  const [cashBox, setCashBox] = useState<CashBoxReadResponse>();
  const { id } = useParams();

  useEffect(() => {
    const getCashBox = async () => {
      const { data, response } = await apiClient.GET("/cashboxes/{id}", {
        params: { path: { id: Number(id) } },
      });
      setCashBox(data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getCashBox();
  }, []);

  if (!cashBox) {
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
      <h2>Detail kasy {cashBox.name}</h2>
      <Typography marginBottom={3}>
        Dohromady v kase: {cashBox.accountTransactions.total}
        czk
      </Typography>
      <Box display="flex" gap={5}>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
        >
          <Typography
            variant="h6"
            component="span"
            display="inline"
            sx={{ marginBottom: 1 }}
          >
            Změny v kase
          </Typography>

          <AccountTransactionPagedView
            initialTransactions={cashBox.accountTransactions}
          />
        </Box>
      </Box>
    </>
  );
};

export default CashBoxDetail;
