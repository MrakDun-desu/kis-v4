import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import {
  StoreItemsApi,
  type StoreItemListModel,
  type StoreItemsReadAllRequest,
} from "../../../api-generated";
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
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import CategoryFilter from "../../../components/filters/CategoryFilter";
import StoreItemCreateForm from "../../../components/forms/StoreItemCreateForm";

const api = new StoreItemsApi(defaultConfiguration);

const StoreItems = () => {
  const [storeItems, setStoreItems] = useState<StoreItemListModel[] | null>(
    null,
  );
  const [request, setRequest] = useState<StoreItemsReadAllRequest>({ page: 1 });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [rowCount, setRowCount] = useState<number>(0);
  const navigate = useNavigate();

  useEffect(() => {
    const getStoreItemsDeferred = setTimeout(async () => {
      setLoading(true);
      const response = await handleApiCall(api.storeItemsReadAll(request));
      if (!response) {
        setStoreItems(null);
        setRowCount(0);
        setLoading(false);
        return;
      }
      setStoreItems(response.data);
      setRowCount(response.meta.total ?? 0);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getStoreItemsDeferred);
  }, [request]);

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
              await handleApiCall(
                api.storeItemsDelete({
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
  const refreshStoreItems = () => setRequest({ ...request });

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
            setRequest((prev) => ({ ...prev, categoryId }))
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
                page: request.page ?? 1,
                pageSize: request.pageSize ?? 30,
              },
            },
          }}
          pageSizeOptions={[30, 100]}
          paginationMode="server"
          sortingMode="server"
          filterMode="server"
          onPaginationModelChange={(newModel, details) => {
            if (details.reason === "stateRestorePreProcessing") {
              return;
            }
            setRequest((prev) => {
              return {
                ...prev,
                page: Math.max(1, newModel.page),
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
