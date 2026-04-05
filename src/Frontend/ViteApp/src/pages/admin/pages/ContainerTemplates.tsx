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
import { Link } from "react-router-dom";
import ContainerTemplateCreateForm from "../../../components/forms/ContainerTemplateCreateForm";
import type { ContainerTemplateModel } from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";
import { useLoading } from "../../../contexts/LoadingContext";

const ContainerTemplates = () => {
  const [containerTemplates, setContainerTemplates] =
    useState<ContainerTemplateModel[]>();
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { startLoading, stopLoading } = useLoading();

  useEffect(() => {
    setLoading(true);
    const getContainerTemplatesDeferred = setTimeout(async () => {
      const { response, data } = await apiClient.GET("/container-templates");
      setContainerTemplates(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getContainerTemplatesDeferred);
  }, [refreshCounter]);

  const columns: GridColDef<ContainerTemplateModel>[] = [
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
      editable: true,
      flex: 1,
    },

    {
      field: "amount",
      headerName: "Objem",
      type: "number",
      sortable: true,
      filterable: true,
      editable: false,
      flex: 1,
      valueFormatter: (val, row) => `${val} ${row.storeItem.unitName}`,
    },

    {
      field: "storeItem",
      headerName: "Skladová položka",
      type: "string",
      sortable: false,
      filterable: true,
      editable: false,
      flex: 1,
      renderCell: (params) => (
        <Link to={`/admin/store-items/${params.row.storeItem.id}`}>
          {params.row.storeItem.name}
        </Link>
      ),
    },

    {
      field: "actions",
      headerName: "Akce",
      flex: 1,
      type: "actions",
      renderCell: (params) => (
        <>
          {params.row.name !==
            containerTemplates?.find((c) => c.id === params.row.id)?.name && (
            <Button
              variant="outlined"
              sx={{ marginRight: 1 }}
              onClick={async () => {
                if (
                  params.row.name ===
                  containerTemplates?.find((c) => c.id === params.row.id)?.name
                ) {
                  alert("Název je nezměněn");
                }
                startLoading();
                const { data, response, error } = await apiClient.PUT(
                  "/container-templates/{id}",
                  {
                    params: {
                      path: { id: params.row.id },
                    },
                    body: {
                      name: params.row.name,
                    },
                  },
                );
                stopLoading();
                if (data) {
                  setContainerTemplates((prev) =>
                    prev?.map((c) =>
                      c.id === data.id ? { ...c, name: data.name } : c,
                    ),
                  );
                } else {
                  handleApiError(response, error);
                }
              }}
            >
              Uložit změny
            </Button>
          )}
          <Button
            color="error"
            variant="outlined"
            onClick={async () => {
              const confirmed = confirm(
                `Opravdu chcete ${params.row.name} smazat?`,
              );
              if (confirmed) {
                const { response } = await apiClient.DELETE(
                  "/container-templates/{id}",
                  { params: { path: { id: params.row.id } } },
                );
                if (response.ok) {
                  refreshContainerTemplates();
                }
              }
            }}
          >
            Smazat
          </Button>
        </>
      ),
    },
  ];

  const openCreateDialog = () => setCreateDialogOpen(true);
  const closeCreateDialog = () => setCreateDialogOpen(false);
  const refreshContainerTemplates = () => setRefreshCounter((val) => val + 1);

  return (
    <>
      <h2>Typy kegů</h2>

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
            <ContainerTemplateCreateForm
              id="containerTemplateCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshContainerTemplates}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="containerTemplateCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <DataGrid
          rowSelection={false}
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={containerTemplates ?? []}
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

export default ContainerTemplates;
