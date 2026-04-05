import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Box, Button, Skeleton, TextField, Typography } from "@mui/material";
import type {
  CashBoxReadResponse,
  CashBoxUpdateRequestModel,
} from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";
import AccountTransactionPagedView from "../../../components/views/AccountTransactionPagedView";
import z from "zod";
import validationConstants from "../../../constants/validationConstants";
import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useLoading } from "../../../contexts/LoadingContext";

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
});

const CashBoxDetail = () => {
  const [cashBox, setCashBox] = useState<CashBoxReadResponse>();
  const cashBoxForm = useForm<CashBoxUpdateRequestModel>({
    values: !cashBox ? { name: "Kasa" } : { name: cashBox.name },
    resolver: zodResolver(ValidationSchema),
  });
  const { id } = useParams();
  const { startLoading, stopLoading } = useLoading();

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

  const updateStore: SubmitHandler<CashBoxUpdateRequestModel> = async (
    requestBody,
  ) => {
    startLoading();
    const { data, response, error } = await apiClient.PUT("/cashboxes/{id}", {
      params: { path: { id: Number(id) } },
      body: requestBody,
    });
    if (data) {
      setCashBox((prev) =>
        prev
          ? {
              ...prev,
              name: data.name,
            }
          : undefined,
      );
    } else {
      handleApiError(response, error);
    }

    stopLoading();
  };

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
      <h2>Detail kasy</h2>

      <Typography marginBottom={3}>
        Aktuální zůstatek: {cashBox.accountTransactions.total}
        czk
      </Typography>

      <form onSubmit={cashBoxForm.handleSubmit(updateStore)}>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
        >
          <TextField
            fullWidth
            label="Název"
            {...cashBoxForm.register("name")}
            error={!!cashBoxForm.formState.errors.name}
            helperText={cashBoxForm.formState.errors?.name?.message}
          />

          <Button type="submit" variant="contained">
            Upravit název
          </Button>
        </Box>
      </form>

      <Box
        display="flex"
        flexDirection="column"
        alignItems="flex-start"
        marginTop={2}
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
    </>
  );
};

export default CashBoxDetail;
