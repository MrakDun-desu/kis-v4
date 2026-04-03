import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { useEffect, useState } from "react";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { getGridStringOperators } from "@mui/x-data-grid";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";
import CategoryFilter from "../../../components/filters/CategoryFilter";
import StoreItemCreateForm from "../../../components/forms/StoreItemCreateForm";

import handleApiError from "../../../errorHandling/apiResponseHandler";
import type { operations } from "../../../api/apiSchema.ts";
import type { StoreItemListModel } from "../../../api/apiTypes.ts";
import { apiClient } from "../../../api/apiClient.ts";

type Query = operations["StoreItemsReadAll"]["parameters"]["query"];

const StoreItems = () => {
  const [storeItems, setStoreItems] = useState<StoreItemListModel[] | null>(
    null,
  );
  const [query, setQuery] = useState<Query>({ Page: 1 });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [rowCount, setRowCount] = useState<number>(0);
  const navigate = useNavigate();

  useEffect(() => {
    const getStoreItemsDeferred = setTimeout(async () => {
      setLoading(true);
      const resp = await apiClient.GET("/store-items", { params: { query } });
      if (!resp.data) {
        setStoreItems(null);
        setRowCount(0);
        handleApiError(resp.response, resp.error);
      } else {
        setStoreItems(resp.data.data);
        setRowCount(resp.data.meta.total ?? 0);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getStoreItemsDeferred);
  }, [query]);

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
              const { response } = await apiClient.DELETE("/store-items/{id}", {
                params: { path: { id: params.row.id } },
              });
              if (response.ok) {
                setQuery({ ...query });
              }
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
  const refreshStoreItems = () => setQuery({ ...query });

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

        <CategoryFilter
          onChange={(categoryId) =>
            setQuery((prev) => ({ ...prev, CategoryId: categoryId }))
          }
        />

        <DataGrid
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={storeItems ?? []}
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
            setQuery((prev) => ({
              ...prev,
              Page: newModel.page + 1,
              PageSize: newModel.pageSize,
            }));
          }}
          onFilterModelChange={(newFilters) => {
            setQuery((prev) => ({
              ...prev,
              Name: newFilters.items.find((f) => f.field === "name")?.value,
              IsContainerItem: newFilters.items.find(
                (f) => f.field === "isContainerItem",
              )?.value,
            }));
          }}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />

        <Dialog open={createDialogOpen} onClose={closeCreateDialog}>
          <DialogTitle>Vytvořit novou skladovou položku</DialogTitle>
          <DialogContent>
            <StoreItemCreateForm
              id="storeItemCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshStoreItems}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="storeItemCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>
      </Box>
    </>
  );
};

export default StoreItems;
