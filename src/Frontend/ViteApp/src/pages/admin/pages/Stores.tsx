import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { StoresApi, type StoreListModel } from "../../../api-generated";
import { useEffect, useState } from "react";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import StoreCreateForm from "../../../components/forms/StoreCreateForm";

const api = new StoresApi(defaultConfiguration);

const Stores = () => {
  const [stores, setStores] = useState<StoreListModel[] | null>(null);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    const getStoresDeferred = setTimeout(async () => {
      const response = await handleApiCall(api.storesReadAll());
      if (!response) {
        setStores(null);
        setLoading(false);
        return;
      }
      setStores(response.data);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getStoresDeferred);
  }, [refreshCounter]);

  const columns: GridColDef<StoreListModel>[] = [
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
      filterable: false,
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
                api.storesDelete({
                  id: params.row.id,
                }),
              );
              refreshStores();
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
  const refreshStores = () => setRefreshCounter((val) => val + 1);

  return (
    <>
      <h2>Sklady</h2>

      <Box
        display="flex"
        gap={2}
        flexDirection="column"
        alignItems="flex-start"
      >
        <Button color="success" variant="contained" onClick={openCreateDialog}>
          Přidat nový
        </Button>

        <Dialog open={createDialogOpen} onClose={closeCreateDialog}>
          <DialogTitle>Vytvořit nový sklad</DialogTitle>
          <DialogContent>
            <StoreCreateForm
              id="storeCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshStores}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="storeCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <DataGrid
          rowSelection={false}
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={stores ?? []}
          columns={columns}
          slotProps={{
            loadingOverlay: {
              variant: "skeleton",
              noRowsVariant: "skeleton",
            },
          }}
          pageSizeOptions={[]}
          pagination={undefined}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />
      </Box>
    </>
  );
};

export default Stores;
