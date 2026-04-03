import { zodResolver } from "@hookform/resolvers/zod";
import {
  Skeleton,
  Box,
  Paper,
  Typography,
  TextField,
  InputAdornment,
} from "@mui/material";
import { useState, useEffect } from "react";
import { useForm, type SubmitHandler } from "react-hook-form";
import z from "zod";
import { useShallow } from "zustand/react/shallow";
import { apiClient } from "../../api/apiClient";
import type {
  SaleTransactionCreateRequest,
  SaleTransactionItemModel,
} from "../../api/apiTypes";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import { useSnackbar } from "../../contexts/SnackbarContext";
import handleApiError from "../../errorHandling/apiResponseHandler";
import {
  type SaleTransactionItemDisplay,
  usePosStore,
} from "../../stores/posStore";
import { useAuth } from "../../auth/AuthContext";

const ValidationSchema = z.object({
  note: z
    .string()
    .max(
      validationConstants.maxNoteLength,
      "Poznámka přesahuje maximální délku",
    )
    .nullish(),
  storeId: z.number(),
  cashBoxId: z.number(),
  customerId: z.string(),
  paidAmount: z
    .string()
    .regex(
      validationConstants.numberRegex,
      "Zaplacená cena musí být platné číslo",
    )
    .refine(
      (val) => Number(val) >= 0,
      "Zaplacená cena musí být větší/rovna nule",
    ),
  saleTransactionItems: z
    .array(
      z.object({
        amount: z.number(),
        saleItemId: z.number(),
        modifications: z
          .array(
            z.object({
              amount: z.number(),
              modifierId: z.number(),
            }),
          )
          .optional(),
      }),
    )
    .optional(),
});

const OrderFinishForm = ({
  formId,
  transactionItems,
  afterSubmit,
}: {
  formId: string;
  transactionItems: SaleTransactionItemDisplay[];
  afterSubmit: () => void;
}) => {
  const { showSnackbar } = useSnackbar();
  const { store, cashBox } = usePosStore(
    useShallow((state) => ({
      store: state.currentStore,
      cashBox: state.currentCashBox,
    })),
  );
  const { userClaims } = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SaleTransactionCreateRequest>({
    defaultValues: {
      cashBoxId: cashBox?.id,
      storeId: store?.id,
      customerId:
        (userClaims!.find((val) => val.type === "sub")?.value as string) ?? "0",
      paidAmount: "0.00",
      saleTransactionItems: transactionItems.map((sti) => ({
        amount: sti.amount,
        saleItemId: sti.saleItemId,
        modifications: sti.modifications.map((m) => ({
          amount: m.amount,
          modifierId: m.modifierId,
        })),
      })),
    },
    resolver: zodResolver(ValidationSchema),
  });
  const { startLoading, stopLoading } = useLoading();
  const [prices, setPrices] = useState<SaleTransactionItemModel[] | null>(null);
  const clearTransactionItems = usePosStore(
    (state) => state.clearTransactionItems,
  );
  const setLayout = usePosStore((state) => state.setCurrentLayout);

  useEffect(() => {
    const getPrices = async () => {
      const { response, data, error } = await apiClient.POST(
        "/sale-transactions/check-price",
        {
          body: {
            saleTransactionItems: transactionItems.map((sti) => ({
              amount: sti.amount,
              saleItemId: sti.saleItemId,
              modifications: sti.modifications.map((m) => ({
                amount: m.amount,
                modifierId: m.modifierId,
              })),
            })),
          },
        },
      );

      if (data) {
        setPrices(data.saleTransactionItems);
      } else {
        handleApiError(response, error);
      }
    };

    getPrices();
  }, []);

  const createSaleTransaction: SubmitHandler<
    SaleTransactionCreateRequest
  > = async (requestBody) => {
    startLoading();
    const { response, data, error } = await apiClient.POST(
      "/sale-transactions",
      {
        body: requestBody,
      },
    );
    if (data) {
      clearTransactionItems();
      setLayout(undefined);
      showSnackbar(
        `Transakce byla úspěšně uložena pod ID ${data.id}!`,
        "success",
      );
      afterSubmit?.();
    } else {
      handleApiError(response, error);
    }
    stopLoading();
  };

  if (!prices) {
    return (
      <>
        <Skeleton variant="rounded" width={300} height={30} />
        <Box display="flex" flexDirection="column" gap={2}>
          <Skeleton variant="rounded" width={300} height={60} />
          <Skeleton variant="rounded" width={300} height={60} />
          <Skeleton variant="rounded" width={300} height={60} />
          <Skeleton variant="rounded" width={300} height={60} />
        </Box>
      </>
    );
  }

  return (
    <form onSubmit={handleSubmit(createSaleTransaction)} id={formId}>
      <Box display="flex" flexDirection="column" alignItems="stretch" gap={2}>
        <Box display="flex" flexDirection="column" gap={1}>
          {prices.map((sti, i) => (
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
                <Typography>
                  {(Number(sti.basePrice) * sti.amount).toFixed(2)},-
                </Typography>
              </Box>
              {sti.modifications?.map((mod, i) => (
                <Box
                  key={i}
                  display="flex"
                  padding={1}
                  paddingTop={0}
                  justifyContent="space-between"
                >
                  <Typography>
                    +{mod.amount} <b>{mod.modifierName}</b>
                  </Typography>
                  <Typography>
                    {(
                      Number(mod.priceChange) *
                      mod.amount *
                      sti.amount
                    ).toFixed(2)}
                    ,-
                  </Typography>
                </Box>
              ))}
            </Paper>
          ))}
          <Typography fontWeight="bold" fontSize={18}>
            Celková cena:{" "}
            {prices
              .reduce((acc, curr) => {
                return (
                  acc +
                  curr.amount *
                    (Number(curr.basePrice) +
                      curr.modifications.reduce(
                        (macc, mcurr) =>
                          macc + Number(mcurr.priceChange) * mcurr.amount,
                        0,
                      ))
                );
              }, 0)
              .toFixed(2)}
            czk
          </Typography>
        </Box>

        <TextField
          label="Zaplaceno"
          {...register("paidAmount")}
          error={!!errors.paidAmount}
          helperText={errors.paidAmount?.message}
          slotProps={{
            input: {
              endAdornment: <InputAdornment position="end">czk</InputAdornment>,
            },
          }}
        />

        <TextField
          label="Poznámka"
          {...register("note")}
          error={!!errors.note}
          helperText={errors.note?.message}
        />
      </Box>
    </form>
  );
};

export default OrderFinishForm;
