import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { PipesApi, type PipeListModel } from "../../../api-generated";
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
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import PipeCreateForm from "../../../components/PipeCreateForm";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";

const api = new PipesApi(defaultConfiguration);

const Pipes = () => {
  const [pipes, setPipes] = useState<PipeListModel[] | null>(null);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);

  useEffect(() => {
    setLoading(true);
    const getPipesDeferred = setTimeout(async () => {
      const response = await handleApiCall(api.pipesReadAll());
      if (!response) {
        setPipes(null);
        setLoading(false);
        return;
      }
      setPipes(response.data);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getPipesDeferred);
  }, [refreshCounter]);

  const columns: GridColDef<PipeListModel>[] = [
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
          color="error"
          variant="outlined"
          onClick={async () => {
            const confirmed = confirm(
              `Opravdu chcete ${params.row.name} smazat?`,
            );
            if (confirmed) {
              await handleApiCall(
                api.pipesDelete({
                  id: params.row.id,
                }),
              );
              refreshPipes();
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
  const refreshPipes = () => setRefreshCounter((val) => val + 1);

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
            <PipeCreateForm
              id="pipeCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshPipes}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="pipeCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <DataGrid
          rowSelection={false}
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={pipes ?? []}
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

export default Pipes;
