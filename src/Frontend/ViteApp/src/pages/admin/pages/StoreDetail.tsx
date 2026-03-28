import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import {
  StoreItemAmountsApi,
  StoresApi,
  StoreTransactionsApi,
  TransactionReason,
  type StoreItemAmountModel,
  type StoreItemAmountsReadAllRequest,
  type StoreReadResponse,
  type StoreTransactionItemCreateRequest,
  type StoreUpdateRequest,
} from "../../../api-generated";
import { useEffect, useState } from "react";
import {
  Box,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  InputAdornment,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Skeleton,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
} from "@mui/material";
import { csCZ } from "@mui/x-data-grid/locales";
import { Link, useParams } from "react-router-dom";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import {
  Controller,
  useFieldArray,
  useForm,
  type SubmitHandler,
} from "react-hook-form";
import z from "zod";
import validationConstants from "../../../constants/validationConstants";
import { zodResolver } from "@hookform/resolvers/zod";
import { useLoading } from "../../../contexts/LoadingContext";
import { AddCircle } from "@mui/icons-material";
import { useSnackbar } from "../../../contexts/SnackbarContext";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import ContainerListView from "../../../components/views/ContainerListView";

const StoreValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
});

const TransactionValidationSchema = z.object({
  note: z
    .string()
    .max(
      validationConstants.maxNoteLength,
      "Poznámka přesahuje maximální délku",
    )
    .nullish(),
  storeTransactionItems: z
    .array(
      z.object({
        amount: z
          .string()
          .regex(
            validationConstants.numberRegex,
            "Množství skladové položky musí být číslo",
          ),
        cost: z
          .string()
          .regex(
            validationConstants.numberRegex,
            "Cena nákupu položky musí být číslo",
          )
          .refine(
            (x) => Number(x) >= 0,
            "Cena nákupu položky musí být větší/rovna nule",
          ),
        storeItemId: z.number(),
        storeItemName: z.string(),
        storeItemUnitName: z.string(),
      }),
    )
    .optional(),
  reason: z.custom<TransactionReason>().optional(),
  storeId: z.number(),
  sourceStoreId: z.number().optional(),
  updateCosts: z.boolean().optional(),
});

type StoreTransactionFormData = z.infer<typeof TransactionValidationSchema>;

const storesApi = new StoresApi(defaultConfiguration);
const storeItemAmountsApi = new StoreItemAmountsApi(defaultConfiguration);
const storeTransactionsApi = new StoreTransactionsApi(defaultConfiguration);

const StoreDetail = () => {
  const { id } = useParams();
  const [store, setStore] = useState<StoreReadResponse | null>(null);
  const [storeItems, setStoreItems] = useState<StoreItemAmountModel[] | null>(
    null,
  );
  const [request, setRequest] = useState<StoreItemAmountsReadAllRequest | null>(
    null,
  );
  const [isLoading, setLoading] = useState<boolean>(true);
  const [rowCount, setRowCount] = useState<number>(0);
  const { startLoading, stopLoading } = useLoading();
  const { showSnackbar } = useSnackbar();
  const storeForm = useForm<StoreUpdateRequest>({
    values: !store
      ? {
          name: "Sklad",
        }
      : {
          name: store.name,
        },
    resolver: zodResolver(StoreValidationSchema),
  });
  const transactionForm = useForm<StoreTransactionFormData>({
    defaultValues: {
      storeId: Number(id),
      note: null,
      updateCosts: false,
      reason: "AddingToStore",
      storeTransactionItems: [],
    },
    resolver: zodResolver(TransactionValidationSchema),
  });

  const transactionReason = transactionForm.watch("reason");

  const transactionItems = useFieldArray({
    control: transactionForm.control,
    name: "storeTransactionItems",
  });

  useEffect(() => {
    const getStore = async () => {
      setLoading(true);
      const response = await handleApiCall(
        storesApi.storesRead({
          id: Number(id),
        }),
      );
      if (response) {
        setStore(response);
        setStoreItems(response.storeItemAmounts.data);
        setRowCount(response.storeItemAmounts.meta.total);
      } else {
        setStore(null);
        setStoreItems(null);
        setRowCount(0);
      }
      setLoading(false);
    };
    getStore();
  }, []);

  useEffect(() => {
    if (request === null) {
      return;
    }
    const getStoreItemAmountsDeferred = setTimeout(async () => {
      setLoading(true);
      const response = await handleApiCall(
        storeItemAmountsApi.storeItemAmountsReadAll(request),
      );
      if (!response) {
        setStoreItems(null);
        setRowCount(0);
      } else {
        setStoreItems(response.data);
        setRowCount(response.meta.total ?? 0);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getStoreItemAmountsDeferred);
  }, [request]);

  const updateStore: SubmitHandler<StoreUpdateRequest> = async (data) => {
    startLoading();
    const response = await handleApiCall(
      storesApi.storesUpdate({ id: Number(id), storeUpdateRequest: data }),
    );
    if (response) {
      setStore((prev) =>
        !prev
          ? null
          : {
              ...prev,
              name: response.name,
            },
      );
    }
    stopLoading();
  };

  const createTransaction: SubmitHandler<StoreTransactionFormData> = async (
    data,
  ) => {
    startLoading();
    const response = await handleApiCall(
      storeTransactionsApi.storeTransactionsCreate({
        storeTransactionCreateRequest: {
          ...data,
          storeTransactionItems: data.storeTransactionItems?.map((sti) => ({
            amount: sti.amount,
            cost: sti.cost,
            storeItemId: sti.storeItemId,
          })),
        },
      }),
    );
    if (response) {
      showSnackbar(
        `Transakce úspěšně uložena pod ID ${response.id}`,
        "success",
      );
      setRequest((prev) =>
        prev ? { ...prev } : { page: 1, pageSize: 30, storeId: Number(id) },
      );
    }
    transactionForm.reset();
    stopLoading();
  };

  const columns: GridColDef<StoreItemAmountModel>[] = [
    {
      field: "storeItem",
      headerName: "Skladová položka",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      renderCell: (params) => (
        <Link to={`/admin/store-items/${params.row.storeItem.id}`}>
          {params.row.storeItem.name}
        </Link>
      ),
    },

    {
      field: "amount",
      headerName: "Množství ve skladu",
      type: "number",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      valueFormatter: (_, row) => `${row.amount} ${row.storeItem.unitName}`,
    },

    {
      field: "actions",
      headerName: "Akce",
      flex: 1,
      type: "actions",
      renderCell: ({ row: itemAmount }) =>
        // don't let people add stuff into transactions if it's container items
        !itemAmount.storeItem.isContainerItem &&
        // also don't display the button for items that already are in the transaction
        transactionItems.fields.findIndex(
          (val) => val.storeItemId === itemAmount.storeItem.id,
        ) === -1 ? (
          <Button
            sx={{
              marginRight: 1,
            }}
            variant="contained"
            onClick={() => {
              const existingItem = transactionItems.fields.find(
                (sti: StoreTransactionItemCreateRequest) =>
                  sti.storeItemId === itemAmount.storeItem.id,
              );
              if (!existingItem) {
                transactionItems.append({
                  amount: "0",
                  cost: "0",
                  storeItemId: itemAmount.storeItem.id,
                  storeItemName: itemAmount.storeItem.name,
                  storeItemUnitName: itemAmount.storeItem.unitName,
                });
              }
            }}
            startIcon={<AddCircle />}
          >
            Přidat do transakce
          </Button>
        ) : null,
    },
  ];

  if (!store) {
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
      <h2>Detail skladu</h2>

      <Box
        display="flex"
        gap={2}
        paddingBottom={30}
        flexDirection="column"
        alignItems="flex-start"
      >
        <form onSubmit={storeForm.handleSubmit(updateStore)}>
          <Box
            display="flex"
            flexDirection="column"
            alignItems="flex-start"
            gap={2}
          >
            <TextField
              fullWidth
              label="Název"
              {...storeForm.register("name")}
              error={!!storeForm.formState.errors.name}
              helperText={storeForm.formState.errors?.name?.message}
            />

            <Button type="submit" variant="contained">
              Upravit název
            </Button>
          </Box>
        </form>

        <h3>Položky ve skladu</h3>
        <DataGrid
          loading={isLoading}
          rows={storeItems ?? []}
          rowCount={rowCount}
          sx={{
            width: "100%",
          }}
          getRowId={(row) => row.storeItem.id}
          rowSelection={false}
          columns={columns}
          slotProps={{
            loadingOverlay: {
              variant: "skeleton",
              noRowsVariant: "skeleton",
            },
          }}
          initialState={{
            pagination: {
              paginationModel: {
                page: request?.page ?? 1,
                pageSize: request?.pageSize ?? 30,
              },
            },
          }}
          pageSizeOptions={[30, 100]}
          paginationMode="server"
          sortingMode="server"
          filterMode="server"
          onPaginationModelChange={(newModel, details) => {
            if (!details.reason) {
              return;
            }
            setRequest({
              storeId: Number(id),
              page: Math.max(newModel.page, 1),
              pageSize: newModel.pageSize,
            });
          }}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />

        {transactionItems.fields.length > 0 && (
          <>
            <h3>Skladová transakce</h3>
            <form onSubmit={transactionForm.handleSubmit(createTransaction)}>
              <Box
                display="flex"
                gap={2}
                marginTop={1}
                justifyContent="stretch"
              >
                <Box
                  display="flex"
                  flexDirection="column"
                  alignItems="flex-start"
                  gap={2}
                >
                  <FormControl fullWidth>
                    <InputLabel id="reasonSelect">Důvod transakce</InputLabel>
                    <Controller
                      name="reason"
                      control={transactionForm.control}
                      render={({ field }) => (
                        <Select
                          labelId="reasonSelect"
                          {...field}
                          label="Důvod transakce"
                        >
                          <MenuItem value="AddingToStore">
                            Přidání do skladu
                          </MenuItem>
                          <MenuItem value="WriteOff">Odpis</MenuItem>
                          {/* TODO implement stock taking and changing stores */}
                        </Select>
                      )}
                    />
                  </FormControl>
                  <TextField
                    fullWidth
                    multiline
                    label="Poznámka"
                    {...transactionForm.register("note")}
                    error={!!transactionForm.formState.errors.note}
                    helperText={transactionForm.formState.errors?.note?.message}
                  />
                  {transactionReason === "AddingToStore" && (
                    <FormControlLabel
                      label="Automaticky přepočíst ceny"
                      control={
                        <Checkbox
                          {...transactionForm.register("updateCosts")}
                        />
                      }
                    />
                  )}

                  <Button variant="contained" type="submit">
                    Dokončit transakci
                  </Button>
                </Box>

                <TableContainer component={Paper}>
                  <Table>
                    <TableHead>
                      <TableRow>
                        <TableCell>Skladová položka</TableCell>
                        <TableCell>Množství</TableCell>
                        {transactionReason === "AddingToStore" && (
                          <TableCell>Nákupní cena</TableCell>
                        )}
                        <TableCell>Akce</TableCell>
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {transactionItems.fields.map((item, index) => (
                        <TableRow key={item.storeItemId}>
                          <TableCell>{item.storeItemName}</TableCell>
                          <TableCell>
                            <TextField
                              fullWidth
                              slotProps={{
                                input: {
                                  endAdornment: (
                                    <InputAdornment position="end">
                                      {item.storeItemUnitName}
                                    </InputAdornment>
                                  ),
                                },
                              }}
                              {...transactionForm.register(
                                `storeTransactionItems.${index}.amount`,
                              )}
                              error={
                                !!transactionForm.formState.errors
                                  .storeTransactionItems?.[index]?.amount
                              }
                              helperText={
                                transactionForm.formState.errors
                                  ?.storeTransactionItems?.[index]?.amount
                                  ?.message
                              }
                            />
                          </TableCell>

                          {transactionReason === "AddingToStore" && (
                            <TableCell>
                              <TextField
                                fullWidth
                                slotProps={{
                                  input: {
                                    endAdornment: (
                                      <InputAdornment position="end">
                                        czk
                                      </InputAdornment>
                                    ),
                                  },
                                }}
                                {...transactionForm.register(
                                  `storeTransactionItems.${index}.cost`,
                                )}
                                error={
                                  !!transactionForm.formState.errors
                                    .storeTransactionItems?.[index]?.cost
                                }
                                helperText={
                                  transactionForm.formState.errors
                                    ?.storeTransactionItems?.[index]?.cost
                                    ?.message
                                }
                              />
                            </TableCell>
                          )}

                          <TableCell>
                            <Button
                              color="error"
                              variant="outlined"
                              type="button"
                              onClick={() => transactionItems.remove(index)}
                            >
                              Odebrat
                            </Button>
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                </TableContainer>
              </Box>
            </form>
          </>
        )}

        <h3>Kegy</h3>

        <ContainerListView
          storeId={store.id}
          initialContainers={store.containers}
          showPipeFilter
          showTemplateFilter
        />
      </Box>
    </>
  );
};

export default StoreDetail;
