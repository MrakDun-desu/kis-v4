import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
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
import ContainerListView from "../../../components/views/ContainerListView";
import StorePicker from "../../../components/pickers/StorePicker";
import type {
  StoreItemAmountModel,
  StoreReadResponse,
  StoreTransactionItemCreateRequest,
  StoreUpdateRequest,
  TransactionReason,
} from "../../../api/apiTypes";
import type { operations } from "../../../api/apiSchema";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const StoreValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
});

const TransactionValidationSchema = z
  .object({
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
    reason: z.custom<TransactionReason>(),
    storeId: z.number(),
    sourceStoreId: z.number().optional(),
    updateCosts: z.boolean(),
  })
  .superRefine((val, ctx) => {
    if (val.reason === "ChangingStores" && val.sourceStoreId === undefined) {
      ctx.addIssue({
        code: "custom",
        message: "Při přesunu musíte nastavit ID zdrojového skladu",
        input: val,
        path: ["sourceStoreId"],
      });
    }
  });

type StoreTransactionFormData = z.infer<typeof TransactionValidationSchema>;
type Query = operations["StoreItemAmountsReadAll"]["parameters"]["query"];

const StoreDetail = () => {
  const { id } = useParams();
  const [store, setStore] = useState<StoreReadResponse>();
  const [storeItems, setStoreItems] = useState<StoreItemAmountModel[]>();
  const [query, setQuery] = useState<Query>();
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
      const { response, data } = await apiClient.GET("/stores/{id}", {
        params: { path: { id: Number(id) } },
      });

      setStore(data);
      setStoreItems(data?.storeItemAmounts.data);
      setRowCount(data?.storeItemAmounts.meta.total ?? 0);

      if (!response.ok) {
        handleApiError(response);
      }
      setLoading(false);
    };
    getStore();
  }, []);

  useEffect(() => {
    if (!query) {
      return;
    }
    const getStoreItemAmountsDeferred = setTimeout(async () => {
      setLoading(true);
      const { response, data } = await apiClient.GET("/store-item-amounts", {
        params: { query },
      });
      setStoreItems(data?.data);
      setRowCount(data?.meta.total ?? 0);
      if (!response.ok) {
        handleApiError(response);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getStoreItemAmountsDeferred);
  }, [query]);

  const updateStore: SubmitHandler<StoreUpdateRequest> = async (
    requestBody,
  ) => {
    startLoading();
    const { data, response, error } = await apiClient.PUT("/stores/{id}", {
      params: { path: { id: Number(id) } },
      body: requestBody,
    });
    if (data) {
      setStore((prev) =>
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

  const createTransaction: SubmitHandler<StoreTransactionFormData> = async (
    responseBody,
  ) => {
    startLoading();
    const { response, data, error } = await apiClient.POST(
      "/store-transactions",
      {
        body: {
          ...responseBody,
          storeTransactionItems: responseBody.storeTransactionItems?.map(
            (sti: StoreTransactionItemCreateRequest) => ({
              amount: sti.amount,
              cost: sti.cost,
              storeItemId: sti.storeItemId,
            }),
          ),
        },
      },
    );
    if (data) {
      showSnackbar(`Transakce úspěšně uložena pod ID ${data.id}`, "success");
      setStoreItems((prev) =>
        prev?.map((si) => {
          const transactionItem = data.storeTransactionItems.find(
            (sti) => sti.storeItem.id === si.storeItem.id,
          );
          return {
            ...si,
            amount:
              transactionItem !== undefined
                ? si.amount + transactionItem.itemAmount
                : si.amount,
          };
        }),
      );
    } else {
      handleApiError(response, error);
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
        alignItems="stretch"
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
                page: query?.Page ?? 0,
                pageSize: query?.PageSize ?? 30,
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
            setQuery({
              StoreId: Number(id),
              Page: newModel.page + 1,
              PageSize: newModel.pageSize,
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
                  minWidth="300px"
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
                          <MenuItem value="ChangingStores">
                            Přesun mezi sklady
                          </MenuItem>
                          <MenuItem value="StockTaking">Inventura</MenuItem>
                        </Select>
                      )}
                    />
                  </FormControl>

                  {transactionReason === "ChangingStores" && (
                    <Controller
                      control={transactionForm.control}
                      name="sourceStoreId"
                      render={({ field }) => (
                        <StorePicker
                          onChange={(val) => field.onChange(val?.id)}
                          error={
                            !!transactionForm.formState.errors.sourceStoreId
                          }
                          helperText={
                            transactionForm.formState.errors?.sourceStoreId
                              ?.message
                          }
                          labelText="Zdrojový sklad"
                        />
                      )}
                    />
                  )}

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

                <TableContainer
                  component={Paper}
                  sx={{
                    flex: "1",
                  }}
                >
                  <Table>
                    <TableHead>
                      <TableRow>
                        <TableCell>Skladová položka</TableCell>
                        <TableCell>
                          {transactionReason === "AddingToStore" &&
                            "Nakoupené množství"}
                          {transactionReason === "ChangingStores" &&
                            "Přesunuté množství"}
                          {transactionReason === "WriteOff" &&
                            "Množství k odepsání"}
                          {transactionReason === "StockTaking" &&
                            "Nové množství"}
                        </TableCell>
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
          afterCreate={(resp) => {
            const storeItemId = resp.data[0]?.template.storeItem.id;
            if (!storeItemId) {
              return;
            }

            const changedAmount =
              Number(resp.data[0].template.amount) * resp.data.length;
            setStoreItems((prev) =>
              prev?.map((sia) =>
                sia.storeItem.id === storeItemId
                  ? {
                      ...sia,
                      amount: String(Number(sia.amount) + changedAmount),
                    }
                  : sia,
              ),
            );
          }}
        />
      </Box>
    </>
  );
};

export default StoreDetail;
