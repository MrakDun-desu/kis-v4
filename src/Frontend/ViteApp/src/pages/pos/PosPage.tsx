import {
  ArrowBack,
  ArrowUpward,
  Cancel,
  Done,
  GridView,
  PointOfSale,
  Store,
} from "@mui/icons-material";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  InputAdornment,
  Paper,
  Skeleton,
  TextField,
  Typography,
} from "@mui/material";
import { useEffect, useState, type ReactNode } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import {
  usePosStore,
  type SaleTransactionItemDisplay,
} from "../../stores/posStore";
import { useShallow } from "zustand/react/shallow";
import { useForm, type SubmitHandler } from "react-hook-form";
import {
  SaleTransactionsApi,
  type SaleTransactionCheckPriceResponse,
  type SaleTransactionCreateRequest,
  type SaleTransactionItemModel,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import { useLoading } from "../../contexts/LoadingContext";
import handleApiCall from "../../errorHandling/apiResponseHandler";
import z from "zod";
import validationConstants from "../../constants/validationConstants";
import { zodResolver } from "@hookform/resolvers/zod";
import { useSnackbar } from "../../contexts/SnackbarContext";

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
  // {
  //   label: "Nedávné transakce",
  //   url: "recent-transactions",
  // },
  {
    label: "Kegy",
    url: "containers",
  },
  // {
  //   label: "Párování karty",
  //   url: "card-pairing",
  // },
  {
    label: "Nastavení",
    url: "settings",
  },
];

const PosPage = () => {
  const navigate = useNavigate();
  const {
    transactionItems,
    currentStore,
    currentCashBox,
    currentLayout,
    layoutHistory,
    clearTransactionItems,
    popLayoutHistory,
    setLayoutId,
    removeTransactionItem,
    updateTransactionItem,
  } = usePosStore(
    useShallow((state) => ({
      transactionItems: state.transactionItems,
      currentStore: state.currentStore,
      currentCashBox: state.currentCashBox,
      currentLayout: state.currentLayout,
      layoutHistory: state.layoutHistory,
      clearTransactionItems: state.clearTransactionItems,
      popLayoutHistory: state.popLayoutHistory,
      setLayoutId: state.setLayoutId,
      removeTransactionItem: state.removeTransactionItem,
      updateTransactionItem: state.updateTransactionItem,
    })),
  );
  const [finishingOrder, setFinishingOrder] = useState(false);

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
        {layoutHistory.length > 1 && (
          <Button
            variant="outlined"
            size="large"
            sx={{
              height: "5rem",
            }}
            startIcon={<ArrowUpward />}
            onClick={() => {
              console.log(layoutHistory);
              const prevLayoutId = layoutHistory[layoutHistory.length - 2];
              popLayoutHistory();
              setLayoutId(prevLayoutId);
            }}
          >
            Předchozí rozložení
          </Button>
        )}
        {links.map((l) => (
          <Button
            key={l.url}
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
          onClick={() => navigate("/admin/store-items")}
        >
          Administrace
        </Button>
      </Paper>

      <Paper
        sx={{
          flexGrow: 1,
          display: "flex",
          flexDirection: "column",
          overflow: "hidden",
        }}
      >
        <Box display="flex" gap={2} padding={1} flexWrap="wrap" flexShrink={0}>
          <Paper
            elevation={4}
            sx={{
              padding: "0.5em 1em",
              display: "flex",
              alignItems: "center",
              gap: "0.5em",
            }}
          >
            <Store /> Aktivní sklad: {currentStore?.name ?? "Nenastaven"}
          </Paper>

          <Paper
            elevation={4}
            sx={{
              padding: "0.5em 1em",
              display: "flex",
              alignItems: "center",
              gap: "0.5em",
            }}
          >
            <PointOfSale /> Aktivní kasa:{" "}
            {currentCashBox?.name ?? "Nenastavena"}
          </Paper>

          <Paper
            elevation={4}
            sx={{
              padding: "0.5em 1em",
              display: "flex",
              alignItems: "center",
              gap: "0.5em",
            }}
          >
            <GridView />{" "}
            <div>Rozložení: {currentLayout?.name ?? "Nenastaveno"}</div>
          </Paper>
        </Box>
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
          flex="1"
          minHeight="0"
        >
          <Box
            display="flex"
            flexDirection="column"
            gap={1}
            flex="1"
            minHeight="0"
            overflow="auto"
            marginBottom="1em"
          >
            {transactionItems.map((sti, i) => (
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

                  <Button
                    color="error"
                    variant="contained"
                    sx={{
                      minWidth: 0,
                    }}
                    onClick={() => {
                      if (sti.amount > 1) {
                        updateTransactionItem(i, { amount: sti.amount - 1 });
                      } else {
                        removeTransactionItem(i);
                      }
                    }}
                  >
                    -1
                  </Button>
                </Box>
                {sti.modifications?.map((mod, i) => (
                  <Box
                    key={i}
                    display="flex"
                    padding={1}
                    paddingTop={0}
                    gap={1}
                  >
                    <Typography>
                      +{mod.amount} <b>{mod.modifierName}</b>
                    </Typography>
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
              disabled={transactionItems.length === 0}
              onClick={() => {
                if (!currentStore || !currentCashBox) {
                  alert(
                    "Pro dokončení objednávky musíte nastavit kasu a sklad!",
                  );
                } else {
                  setFinishingOrder(true);
                }
              }}
            >
              Dokončit objednávku
            </Button>

            <Button
              variant="contained"
              color="error"
              size="large"
              startIcon={<Cancel />}
              sx={{ height: "100px", fontSize: 20 }}
              disabled={transactionItems.length === 0}
              onClick={() => {
                clearTransactionItems();
              }}
            >
              Zrušit objednávku
            </Button>
          </Box>
        </Box>
      </Paper>

      <Dialog open={finishingOrder}>
        <DialogTitle>Dokončení objednávky</DialogTitle>
        <DialogContent>
          <OrderFinish
            formId="orderFinishForm"
            transactionItems={transactionItems}
            afterSubmit={() => setFinishingOrder(false)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setFinishingOrder(false)}>Zrušit</Button>
          <Button type="submit" form="orderFinishForm">
            Dokončit
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

const saleTransactionsApi = new SaleTransactionsApi(defaultConfiguration);

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

const OrderFinish = ({
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
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SaleTransactionCreateRequest>({
    defaultValues: {
      cashBoxId: cashBox?.id,
      storeId: store?.id,
      customerId: "0",
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
      const resp = await handleApiCall(
        saleTransactionsApi.saleTransactionsCheckPrice({
          saleTransactionCheckPriceRequest: {
            saleTransactionItems: transactionItems.map((sti) => ({
              amount: sti.amount,
              saleItemId: sti.saleItemId,
              modifications: sti.modifications.map((m) => ({
                amount: m.amount,
                modifierId: m.modifierId,
              })),
            })),
          },
        }),
      );

      if (resp) {
        setPrices(resp.saleTransactionItems);
      }
    };

    getPrices();
  }, []);

  const createSaleTransaction: SubmitHandler<
    SaleTransactionCreateRequest
  > = async (data) => {
    startLoading();
    const resp = await handleApiCall(
      saleTransactionsApi.saleTransactionsCreate({
        saleTransactionCreateRequest: data,
      }),
    );
    if (resp) {
      clearTransactionItems();
      setLayout(undefined);
      showSnackbar(
        `Transakce byla úspěšně uložena pod ID ${resp.id}!`,
        "success",
      );
      afterSubmit?.();
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

export default PosPage;
