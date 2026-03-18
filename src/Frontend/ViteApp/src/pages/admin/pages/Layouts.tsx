import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { LayoutsApi, type LayoutListModel } from "../../../api-generated";
import { useEffect, useState } from "react";
import { defaultConfiguration } from "../../../configuration";
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
import LayoutCreateForm from "../../../components/LayoutCreateForm";

const api = new LayoutsApi(defaultConfiguration);

const Layouts = () => {
  const [layouts, setLayouts] = useState<LayoutListModel[] | null>(null);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    const getLayoutsDeferred = setTimeout(async () => {
      const response = await handleApiCall(api.layoutsReadAll());
      if (!response) {
        setLayouts(null);
        setLoading(false);
        return;
      }
      setLayouts(response.data);
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
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
    },

    {
      field: "topLevel",
      headerName: "Výchozí layout",
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
                api.layoutsDelete({
                  id: params.row.id,
                }),
              );
              refreshLayouts();
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
          pageSizeOptions={[]}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />
      </Box>
    </>
  );
};

export default Layouts;
