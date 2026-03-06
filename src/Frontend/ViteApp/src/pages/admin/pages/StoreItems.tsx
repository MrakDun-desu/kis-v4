import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid, GridActionsCellItem } from "@mui/x-data-grid";
import {
  CategoriesApi,
  StoreItemsApi,
  type CategoryModel,
  type StoreItemCreateRequest,
  type StoreItemListModel,
  type StoreItemsCreateRequest,
  type StoreItemsReadAllRequest,
} from "../../../api-generated";
import { useEffect, useState } from "react";
import { defaultConfiguration } from "../../../configuration";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Typography,
  TextField,
  Checkbox,
  FormControlLabel,
} from "@mui/material";
import { getGridStringOperators } from "@mui/x-data-grid";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { NumberField } from "@base-ui/react";

const api = new StoreItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);

export const StoreItems = () => {
  const [storeItems, setStoreItems] = useState<StoreItemListModel[] | null>(
    null,
  );
  const [request, setRequest] = useState<StoreItemsReadAllRequest>({ page: 1 });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [rowCount, setRowCount] = useState<number>(0);
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const { register, handleSubmit, control } = useForm<StoreItemCreateRequest>({
    values: {
      initialCost: "0.00",
      name: "",
      unitName: "ks",
      categoryIds: [],
      isContainerItem: false,
    },
  });
  const navigate = useNavigate();

  useEffect(() => {
    const handler = setTimeout(async () => {
      setLoading(true);
      const response = await api.storeItemsReadAll(request);
      setStoreItems(response.data);
      setRowCount(response.meta.total ?? 0);
      setLoading(false);
    }, 500);
    return () => clearTimeout(handler);
  }, [request]);
  useEffect(() => {
    const getCategories = async () => {
      const response = await categoryApi.categoriesReadAll();
      setCategories(response.data);
    };
    getCategories();
  }, []);

  const createStoreItem: SubmitHandler<StoreItemCreateRequest> = async (
    data,
  ) => {
    closeCreateDialog();
    await api.storeItemsCreate({
      storeItemCreateRequest: data,
    });
    setRequest({ ...request });
  };

  const columns: GridColDef<StoreItemListModel>[] = [
    {
      field: "id",
      headerName: "ID",
      type: "number",
      sortable: false,
      editable: false,
      filterable: false,
    },
    {
      field: "name",
      headerName: "Název",
      type: "string",
      sortable: false,
      filterable: true,
      filterOperators: getGridStringOperators().filter(
        (operator) => operator.value === "contains",
      ),
      editable: false,
      flex: 1,
    },
    {
      field: "unitName",
      headerName: "Jednotka",
      type: "string",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
    },
    {
      field: "currentCost",
      headerName: "Cena za jednotku",
      type: "number",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      valueFormatter(value: number) {
        return `${value} czk`;
      },
    },
    {
      field: "isContainerItem",
      headerName: "Kegová položka",
      type: "boolean",
      sortable: false,
      filterable: true,
      editable: false,
      flex: 1,
    },
    {
      field: "actions",
      headerName: "Akce",
      flex: 1,
      type: "actions",
      renderCell: (params) => [
        <Button
          sx={{
            marginRight: 1,
          }}
          variant="contained"
          onClick={() => navigate(`${params.row.id}`)}
        >
          Detail
        </Button>,
        <Button
          color="error"
          variant="outlined"
          onClick={async () => {
            const confirmed = confirm(
              `Opravdu chcete ${params.row.name} smazat?`,
            );
            if (confirmed) {
              await api.storeItemsDelete({
                id: params.row.id,
              });

              setRequest({ ...request });
            }
          }}
        >
          Smazat
        </Button>,
      ],
    },
  ];

  const openCreateDialog = () => setCreateDialogOpen(true);
  const closeCreateDialog = () => setCreateDialogOpen(false);

  return (
    <>
      <h2>Skladové položky</h2>

      <Box
        display="flex"
        gap={2}
        flexDirection="column"
        alignItems="flex-start"
      >
        <Button color="success" variant="contained" onClick={openCreateDialog}>
          Přidat novou
        </Button>

        <Dialog open={createDialogOpen} onClose={closeCreateDialog}>
          <DialogTitle>Vytvořit novou skladovou položku</DialogTitle>
          <DialogContent>
            <form
              onSubmit={handleSubmit(createStoreItem)}
              id="storeItemCreateForm"
            >
              <Box
                display="flex"
                flexDirection="column"
                alignItems="flex-start"
                gap={2}
                marginTop={1}
              >
                <TextField label="Název" {...register("name")} />
                <TextField label="Název jednotky" {...register("unitName")} />
                <FormControl>
                  <InputLabel id="categorySelect">Kategorie</InputLabel>
                  <Controller
                    name="categoryIds"
                    control={control}
                    render={({ field }) => (
                      <Select
                        sx={{
                          minWidth: "10em",
                        }}
                        labelId="categorySelect"
                        multiple
                        {...field}
                        label="Kategorie"
                      >
                        {categories?.map((cat) => (
                          <MenuItem key={cat.id} value={cat.id}>
                            {cat.name}
                          </MenuItem>
                        )) ?? null}
                      </Select>
                    )}
                  />
                </FormControl>
                <FormControlLabel
                  label="Kegová položka"
                  control={<Checkbox {...register("isContainerItem")} />}
                />
                <TextField
                  label="Počáteční cena"
                  {...register("initialCost")}
                />
              </Box>
            </form>
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="storeItemCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <FormControl fullWidth>
          <InputLabel id="categoryFilterLabel">
            Filtrování podle kategorie
          </InputLabel>
          <Select
            id="categoryFilter"
            label="Zobrazit pouze kategorii"
            labelId="categoryFilterLabel"
            value={request.categoryId ?? ""}
            onChange={(evt) => {
              const newValue =
                evt.target.value === 0 ? undefined : evt.target.value;
              setRequest((prev) => {
                return {
                  ...prev,
                  categoryId: newValue,
                };
              });
            }}
          >
            {categories
              ? [
                  <MenuItem key="empty" value={0}>
                    Zobrazit všechny
                  </MenuItem>,
                  ...categories.map((category) => (
                    <MenuItem key={category.id} value={category.id}>
                      {category.name}
                    </MenuItem>
                  )),
                ]
              : null}
          </Select>
        </FormControl>

        <DataGrid
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={storeItems ?? []}
          rowCount={rowCount}
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
                page: request.page ?? 1,
                pageSize: request.pageSize ?? 30,
              },
            },
          }}
          pageSizeOptions={[30, 100]}
          paginationMode="server"
          sortingMode="server"
          filterMode="server"
          onPaginationModelChange={(newModel) => {
            setRequest((prev) => {
              return {
                ...prev,
                page: newModel.page,
                pageSize: newModel.pageSize,
              };
            });
          }}
          onFilterModelChange={(newFilters) => {
            for (const filter of newFilters.items) {
              const newRequest: any = {
                page: request.page,
                pageSize: request.pageSize,
              };
              newRequest[filter.field] = filter.value;
              setRequest(newRequest);
            }
          }}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />
      </Box>
    </>
  );
};
