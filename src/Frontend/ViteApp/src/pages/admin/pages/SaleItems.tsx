import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import {
  CategoriesApi,
  PrintType,
  SaleItemsApi,
  type CategoryModel,
  type SaleItemListModel,
  type SaleItemsReadAllRequest,
} from "../../../api-generated";
import { useEffect, useState } from "react";
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
} from "@mui/material";
import { getGridStringOperators } from "@mui/x-data-grid";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";
import SaleItemCreateForm from "../../../components/SaleItemCreateForm";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { printTypes } from "../../../constants/printTypes";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";

const api = new SaleItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);

const SaleItems = () => {
  const [saleItems, setSaleItems] = useState<SaleItemListModel[] | null>(null);
  const [request, setRequest] = useState<SaleItemsReadAllRequest>({ page: 1 });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [rowCount, setRowCount] = useState<number>(0);
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const getSaleItemsDeferred = setTimeout(async () => {
      setLoading(true);
      const response = await handleApiCall(api.saleItemsReadAll(request));
      if (!response) {
        setSaleItems(null);
        setRowCount(0);
        setLoading(false);
        return;
      }
      setSaleItems(response.data);
      setRowCount(response.meta.total ?? 0);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getSaleItemsDeferred);
  }, [request]);
  useEffect(() => {
    const getCategories = async () => {
      const response = await handleApiCall(categoryApi.categoriesReadAll());
      if (!response) {
        setCategories(null);
        return;
      }
      setCategories(response.data);
    };
    getCategories();
  }, []);

  const columns: GridColDef<SaleItemListModel>[] = [
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
      field: "marginPercent",
      headerName: "Procentuální marže",
      type: "number",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      valueFormatter(value: number) {
        return `${value}%`;
      },
    },

    {
      field: "marginStatic",
      headerName: "Statická marže",
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
      field: "prestigeAmount",
      headerName: "Prestiž",
      type: "number",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
    },

    {
      field: "printType",
      headerName: "Tisknout?",
      type: "string",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      valueFormatter: (value: PrintType) => printTypes[value],
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
              await handleApiCall(
                api.saleItemsDelete({
                  id: params.row.id,
                }),
              );
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
  const refreshSaleItems = () => setRequest({ ...request });

  return (
    <>
      <h2>Prodejní položky</h2>

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
            <SaleItemCreateForm
              id="saleItemCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshSaleItems}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="saleItemCreateForm">
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
          rows={saleItems ?? []}
          rowCount={rowCount}
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

export default SaleItems;
