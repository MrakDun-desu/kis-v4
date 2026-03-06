import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import {
  CategoriesApi,
  StoreItemsApi,
  type CategoryModel,
  type StoreItemListModel,
  type StoreItemsReadAllRequest,
} from "../../../api-generated";
import { useEffect, useState } from "react";
import { defaultConfiguration } from "../../../configuration";
import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import { getGridStringOperators } from "@mui/x-data-grid";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";

const api = new StoreItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);

export const StoreItems = () => {
  const [storeItems, setStoreItems] = useState<StoreItemListModel[] | null>(
    null,
  );
  const [request, setRequest] = useState<StoreItemsReadAllRequest>({ page: 1 });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [rowCount, setRowCount] = useState<number>(0);
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
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
      type: "actions",
      renderCell: (params) => [
        <Button
          key=""
          variant="contained"
          size="small"
          onClick={() => navigate(`${params.row.id}`)}
        >
          Detail
        </Button>,
      ],
    },
  ];

  return (
    <>
      <h2>Skladové položky</h2>

      <Box display={"flex"} gap={2} flexDirection={"column"}>
        <FormControl>
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
