import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import {
  Box,
  Button,
  capitalize,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Skeleton,
  TextField,
  Typography,
} from "@mui/material";
import type {
  AccountTransactionCreateRequest,
  AccountTransactionType,
  CashBoxReadResponse,
  CashBoxUpdateRequestModel,
} from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";
import AccountTransactionPagedView from "../../../components/views/AccountTransactionPagedView";
import z from "zod";
import validationConstants from "../../../constants/validationConstants";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useLoading } from "../../../contexts/LoadingContext";
import { accountTransactionTypes } from "../../../constants/accountTransactionTypes";
import CashBoxPicker from "../../../components/pickers/CashBoxPicker";

const CashBoxValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
});

const TransactionValidationSchema = z
  .object({
    accountId: z.number(),
    amount: z
      .string()
      .regex(validationConstants.numberRegex, "Částka musí být platné číslo"),
    type: z.custom<AccountTransactionType>(),
    targetAccountId: z.number().optional(),
  })
  .superRefine((val, ctx) => {
    if (val.type === "Transfer" && val.targetAccountId === undefined) {
      ctx.addIssue({
        code: "custom",
        message: "Při přesunu musíte nastavit ID cílové kasy",
        input: val,
        path: ["targetAccountId"],
      });
    }
  });

const allowedTransactionTypes: AccountTransactionType[] = [
  "StockTaking",
  "Deposit",
  "Withdrawal",
  "Transfer",
];

const CashBoxDetail = () => {
  const { id } = useParams();
  const [cashBox, setCashBox] = useState<CashBoxReadResponse>();
  const [transactionRefreshCounter, setTransactionRefreshCounter] = useState(0);
  const cashBoxForm = useForm<CashBoxUpdateRequestModel>({
    values: !cashBox ? { name: "Kasa" } : { name: cashBox.name },
    resolver: zodResolver(CashBoxValidationSchema),
  });
  const transactionForm = useForm<AccountTransactionCreateRequest>({
    defaultValues: {
      accountId: 0,
      amount: "0.00",
      type: "StockTaking",
    },
    resolver: zodResolver(TransactionValidationSchema),
  });
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

  const updateCashBox: SubmitHandler<CashBoxUpdateRequestModel> = async (
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

  const createAccountTransaction: SubmitHandler<
    AccountTransactionCreateRequest
  > = async (requestBody) => {
    if (!cashBox) {
      return;
    }
    startLoading();
    const { data, response, error } = await apiClient.POST(
      "/account-transactions",
      {
        body: {
          ...requestBody,
          accountId: cashBox.accountTransactions.accountId,
        },
      },
    );

    if (data) {
      setTransactionRefreshCounter((prev) => prev + 1);
    }

    if (!response.ok) {
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

  const transactionType = transactionForm.watch("type");

  return (
    <>
      <h2>Detail kasy</h2>

      <Box display="flex" flexDirection="column" gap={2}>
        <Box display="flex" gap={2}>
          <form onSubmit={cashBoxForm.handleSubmit(updateCashBox)}>
            <Box
              display="flex"
              flexDirection="column"
              alignItems="flex-start"
              gap={2}
              minWidth={300}
            >
              <Typography>
                Aktuální zůstatek: {cashBox.accountTransactions.total}
                czk
              </Typography>

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

          <form
            onSubmit={transactionForm.handleSubmit(createAccountTransaction)}
          >
            <Box
              display="flex"
              flexDirection="column"
              alignItems="flex-start"
              gap={2}
              minWidth={300}
            >
              <Typography variant="h6" component="h3">
                Zápis pohybu na kase
              </Typography>

              <FormControl fullWidth>
                <InputLabel id="typeSelect">Typ pohybu</InputLabel>
                <Controller
                  control={transactionForm.control}
                  name="type"
                  render={({ field }) => (
                    <Select
                      fullWidth
                      labelId="typeSelect"
                      {...field}
                      label="Typ pohybu"
                    >
                      {allowedTransactionTypes.map((x) => (
                        <MenuItem value={x}>
                          {capitalize(accountTransactionTypes[x])}
                        </MenuItem>
                      ))}
                    </Select>
                  )}
                />
              </FormControl>

              {transactionType === "Transfer" && (
                <Controller
                  control={transactionForm.control}
                  name="targetAccountId"
                  render={({ field }) => (
                    <CashBoxPicker
                      onChange={(val) => field.onChange(val?.accountId)}
                      label="Cílová kasa"
                      excludeId={cashBox.id}
                      error={!!transactionForm.formState.errors.targetAccountId}
                      helperText={
                        transactionForm.formState.errors.targetAccountId
                          ?.message
                      }
                    />
                  )}
                />
              )}

              <TextField
                fullWidth
                label={
                  transactionType === "StockTaking"
                    ? "Nový zůstatek"
                    : transactionType === "Deposit"
                      ? "Přidaná částka"
                      : transactionType === "Withdrawal"
                        ? "Vybraná částka"
                        : transactionType === "Transfer"
                          ? "Přesunutá částka"
                          : null
                }
                {...transactionForm.register("amount")}
                error={!!transactionForm.formState.errors.amount}
                helperText={transactionForm.formState.errors.amount?.message}
              />

              <Button variant="contained" type="submit">
                Zapsat pohyb
              </Button>
            </Box>
          </form>
        </Box>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
        >
          <Typography
            variant="h6"
            component="h3"
            display="inline"
            sx={{ marginBottom: 1 }}
          >
            Pohyby na kase
          </Typography>

          <AccountTransactionPagedView
            initialTransactions={cashBox.accountTransactions}
            refreshCounter={transactionRefreshCounter}
            onRefresh={(data) =>
              setCashBox((prev) =>
                prev
                  ? {
                      ...prev,
                      accountTransactions: data,
                    }
                  : undefined,
              )
            }
          />
        </Box>
      </Box>
    </>
  );
};

export default CashBoxDetail;
