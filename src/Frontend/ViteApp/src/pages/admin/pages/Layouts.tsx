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
import { useNavigate } from "react-router-dom";
import { usePosStore } from "../../../stores/posStore";
import LayoutCreateForm from "../../../components/forms/LayoutCreateForm";
import type { LayoutListModel } from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const Layouts = () => {
  const [layouts, setLayouts] = useState<LayoutListModel[] | null>(null);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const setCurrentLayout = usePosStore((state) => state.setCurrentLayout);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    const getLayoutsDeferred = setTimeout(async () => {
      const { response, data } = await apiClient.GET("/layouts");
      if (!data) {
        setLayouts(null);
        handleApiError(response);
      } else {
        setLayouts(data.data);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getLayoutsDeferred);
  }, [refreshCounter]);

  const columns: GridColDef<LayoutListModel>[] = [
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
      sortable: true,
      filterable: true,
      editable: false,
      flex: 1,
    },

    {
      field: "topLevel",
      headerName: "Výchozí rozložení",
      type: "boolean",
      sortable: true,
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
              const { response } = await apiClient.DELETE("/layouts/{id}", {
                params: { path: { id: params.row.id } },
              });
              if (!response.ok) {
                handleApiError(response);
              } else {
                refreshLayouts();
              }
              setCurrentLayout(undefined);
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
  const refreshLayouts = () => setRefreshCounter((val) => val + 1);

  return (
    <>
      <h2>Rozložení</h2>

      <Box
        display="flex"
        gap={2}
        flexDirection="column"
        alignItems="flex-start"
      >
        <Button color="success" variant="contained" onClick={openCreateDialog}>
          Přidat nové
        </Button>

        <Dialog open={createDialogOpen} onClose={closeCreateDialog}>
          <DialogTitle>Vytvořit nový sklad</DialogTitle>
          <DialogContent>
            <LayoutCreateForm
              id="layoutCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshLayouts}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="layoutCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <DataGrid
          rowSelection={false}
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={layouts ?? []}
          columns={columns}
          slotProps={{
            loadingOverlay: {
              variant: "skeleton",
              noRowsVariant: "skeleton",
            },
          }}
          pageSizeOptions={[30, 100]}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />
      </Box>
    </>
  );
};

export default Layouts;
