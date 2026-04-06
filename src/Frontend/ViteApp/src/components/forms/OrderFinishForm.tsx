import {
  Skeleton,
  Box,
  Paper,
  Typography,
  TableContainer,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
  IconButton,
  Button,
  Divider,
} from "@mui/material";
import { useState, useEffect, type SubmitEventHandler } from "react";
import { useShallow } from "zustand/react/shallow";
import { apiClient } from "../../api/apiClient";
import type { SaleTransactionItemModel } from "../../api/apiTypes";
import { useLoading } from "../../contexts/LoadingContext";
import { useSnackbar } from "../../contexts/SnackbarContext";
import handleApiError from "../../errorHandling/apiResponseHandler";
import { usePosStore } from "../../stores/posStore";
import { useAuth } from "../../auth/AuthContext";
import { Add, Backspace, Remove } from "@mui/icons-material";

const OrderFinishForm = ({
  formId,
  afterSubmit,
}: {
  formId: string;
  afterSubmit: () => void;
}) => {
  const {
    transactionItems,
    clearTransactionItems,
    removeTransactionItem,
    updateTransactionItem,
  } = usePosStore(
    useShallow((state) => ({
      transactionItems: state.transactionItems,
      clearTransactionItems: state.clearTransactionItems,
      removeTransactionItem: state.removeTransactionItem,
      updateTransactionItem: state.updateTransactionItem,
    })),
  );
  const { showSnackbar } = useSnackbar();
  const { store, cashBox } = usePosStore(
    useShallow((state) => ({
      store: state.currentStore,
      cashBox: state.currentCashBox,
    })),
  );
  const { userClaims } = useAuth();
  const { startLoading, stopLoading } = useLoading();
  const [prices, setPrices] = useState<SaleTransactionItemModel[]>();
  const [paidAmount, setPaidAmount] = useState("0");
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

      setPrices(data?.saleTransactionItems);
      if (!response.ok) {
        handleApiError(response, error);
      }
    };

    getPrices();
  }, []);

  const createSaleTransaction: SubmitEventHandler<HTMLFormElement> = async (
    evt,
  ) => {
    evt?.preventDefault();
    if (!cashBox || !store) {
      return;
    }
    startLoading();
    const { response, data, error } = await apiClient.POST(
      "/sale-transactions",
      {
        body: {
          cashBoxId: cashBox.id,
          storeId: store.id,
          customerId:
            (userClaims!.find((val) => val.type === "sub")?.value as string) ??
            "0",
          paidAmount,
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
    <form
      onSubmit={createSaleTransaction}
      id={formId}
      style={{ height: "100%" }}
    >
      <Box display="flex" gap={5} paddingTop={1} height="100%">
        <Box display="flex" flexDirection="column" gap={2} flex="1.5">
          <Typography fontSize="1.2rem">Prodejní položky:</Typography>
          <Box
            display="flex"
            flexDirection="column"
            gap={1}
            flexGrow="1"
            overflow="auto"
            flexShrink="1"
            minHeight="0"
          >
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Počet</TableCell>
                    <TableCell sx={{ flex: 1 }}>Položka</TableCell>
                    <TableCell>Za kus</TableCell>
                    <TableCell>Celkem</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {prices.map((p) => (
                    <TableRow key={p.lineNumber}>
                      <TableCell>
                        <IconButton
                          onClick={() => {
                            if (p.amount > 1) {
                              updateTransactionItem(p.lineNumber - 1, {
                                amount: p.amount - 1,
                              });
                              setPrices((prev) =>
                                prev?.map((prevP) =>
                                  prevP.lineNumber === p.lineNumber
                                    ? {
                                        ...p,
                                        amount: p.amount - 1,
                                      }
                                    : prevP,
                                ),
                              );
                            } else {
                              removeTransactionItem(p.lineNumber - 1);
                              setPrices((prev) =>
                                prev?.filter(
                                  (prevP) => prevP.lineNumber !== p.lineNumber,
                                ),
                              );
                            }
                          }}
                        >
                          <Remove />
                        </IconButton>
                        <Typography
                          fontWeight="bold"
                          fontSize="1.2em"
                          component="span"
                        >
                          {p.amount}
                        </Typography>
                        <IconButton
                          onClick={() => {
                            updateTransactionItem(p.lineNumber - 1, {
                              amount: p.amount + 1,
                            });
                            setPrices((prev) =>
                              prev?.map((prevP) =>
                                prevP.lineNumber === p.lineNumber
                                  ? {
                                      ...p,
                                      amount: p.amount + 1,
                                    }
                                  : prevP,
                              ),
                            );
                          }}
                        >
                          <Add />
                        </IconButton>
                      </TableCell>

                      <TableCell>{p.saleItemName}</TableCell>

                      <TableCell>
                        {p.basePrice +
                          p.modifications.reduce(
                            (acc, curr) =>
                              acc + curr.amount * Number(curr.priceChange),
                            0,
                          )}{" "}
                        Kč
                      </TableCell>

                      <TableCell sx={{ fontWeight: "bold" }}>
                        {(Number(p.basePrice) +
                          p.modifications.reduce(
                            (acc, curr) =>
                              acc + curr.amount * Number(curr.priceChange),
                            0,
                          )) *
                          p.amount}{" "}
                        Kč
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          </Box>
          <Typography variant="h4">
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

        <Divider variant="fullWidth" orientation="vertical" />

        <Box
          flex="0.7"
          display="flex"
          flexDirection="column"
          gap={2}
          alignSelf="end"
        >
          <Typography component="div" variant="h4" textAlign="right">
            {paidAmount} Kč
          </Typography>

          <Box display="grid" gridTemplateColumns="repeat(3, 1fr)" gap={2}>
            {Array.apply(null, Array(9)).map((_, x) => (
              <Button
                key={x}
                variant="contained"
                sx={{ aspectRatio: 1, fontSize: "1.5em" }}
                onClick={() => {
                  if (Number(paidAmount) === 0) {
                    setPaidAmount(String(x + 1));
                  } else {
                    setPaidAmount((prev) => `${prev}${x + 1}`);
                  }
                }}
              >
                {x + 1}
              </Button>
            ))}

            <Button
              variant="contained"
              color="warning"
              sx={{ aspectRatio: 1, fontSize: "1.5em" }}
              onClick={() => {
                setPaidAmount("0");
              }}
            >
              C
            </Button>

            <Button
              variant="contained"
              sx={{ aspectRatio: 1, fontSize: "1.5em" }}
              onClick={() => {
                if (Number(paidAmount) !== 0) {
                  setPaidAmount((prev) => `${prev}0`);
                }
              }}
            >
              0
            </Button>

            <Button
              variant="contained"
              sx={{ aspectRatio: 1, fontSize: "1.5em" }}
              onClick={() => {
                if (paidAmount.length === 1) {
                  setPaidAmount("0");
                } else {
                  setPaidAmount((prev) => prev.slice(0, prev.length - 1));
                }
              }}
            >
              <Backspace />
            </Button>
          </Box>
        </Box>
      </Box>
    </form>
  );
};

export default OrderFinishForm;
