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
import { csCZ } from "@mui/x-data-grid/locales";
import TapCreateForm from "../../../components/forms/TapCreateForm";
import type { TapListModel } from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";
import { Link } from "react-router-dom";

const Taps = () => {
  const [taps, setTaps] = useState<TapListModel[] | null>(null);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);

  useEffect(() => {
    setLoading(true);
    const getTapsDeferred = setTimeout(async () => {
      const { data, response } = await apiClient.GET("/taps");
      if (!data) {
        setTaps(null);
        handleApiError(response);
      } else {
        setTaps(data.data);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getTapsDeferred);
  }, [refreshCounter]);

  const columns: GridColDef<TapListModel>[] = [
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
      field: "store",
      headerName: "Sklad",
      type: "string",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      renderCell: ({ row }) => (
        <Link to={`/admin/stores/${row.store.id}`}>{row.store.name}</Link>
      ),
    },

    {
      field: "container",
      headerName: "Keg",
      type: "string",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      renderCell: ({ row }) => (
        <Link to={`/admin/stores/${row.containerId}`}>{row.containerId}</Link>
      ),
    },

    {
      field: "actions",
      headerName: "Akce",
      flex: 1,
      type: "actions",
      renderCell: (params) => [
        <Button
          color="error"
          variant="outlined"
          onClick={async () => {
            const confirmed = confirm(
              `Opravdu chcete ${params.row.name} smazat?`,
            );
            if (confirmed) {
              const { response } = await apiClient.DELETE("/taps/{id}", {
                params: { path: { id: params.row.id } },
              });
              if (!response.ok) {
                handleApiError(response);
              } else {
                refreshTaps();
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
  const refreshTaps = () => setRefreshCounter((val) => val + 1);

  return (
    <>
      <h2>Pípy</h2>

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
          <DialogTitle>Vytvořit nový sklad</DialogTitle>
          <DialogContent>
            <TapCreateForm
              id="tapCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshTaps}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="tapCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <DataGrid
          rowSelection={false}
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={taps ?? []}
          columns={columns}
          slotProps={{
            loadingOverlay: {
              variant: "skeleton",
              noRowsVariant: "skeleton",
            },
          }}
          pageSizeOptions={[]}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />
      </Box>
    </>
  );
};

export default Taps;
