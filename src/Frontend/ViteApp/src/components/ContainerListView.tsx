import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import {
  ContainerChangesApi,
  ContainersApi,
  ContainerState,
  type ContainerListModel,
  type ContainersReadAllRequest,
} from "../api-generated";
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
import { Link, useNavigate } from "react-router-dom";
import { defaultConfiguration } from "../configuration/apiConfiguration";
import handleApiCall from "../errorHandling/apiResponseHandler";
import ContainerCreateForm from "./ContainerCreateForm";

const api = new ContainersApi(defaultConfiguration);
const containerChangesApi = new ContainerChangesApi(defaultConfiguration);

const ContainerListView = ({ storeId }: { storeId: number }) => {
  const [containers, setContainers] = useState<ContainerListModel[] | null>(
    null,
  );
  const [request, setRequest] = useState<ContainersReadAllRequest>({
    page: 1,
    storeId,
    includeUnusable: false,
  });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [rowCount, setRowCount] = useState<number>(0);
  const navigate = useNavigate();

  useEffect(() => {
    const getContainersDeferred = setTimeout(async () => {
      setLoading(true);
      const response = await handleApiCall(api.containersReadAll(request));
      if (!response) {
        setContainers(null);
        setRowCount(0);
        setLoading(false);
      } else {
        setContainers(response.data);
        setRowCount(response.meta.total);
        setLoading(false);
      }
    }, 500);
    return () => clearTimeout(getContainersDeferred);
  }, [request]);

  const columns: GridColDef<ContainerListModel>[] = [
    {
      field: "id",
      headerName: "ID",
      type: "number",
      sortable: false,
      editable: false,
      filterable: false,
    },

    {
      field: "template",
      headerName: "Typ",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      renderCell: ({ row }) => (
        <Link to={`/admin/container-templates/${row.template.id}`}>
          {row.template.name}
        </Link>
      ),
    },

    {
      field: "amount",
      headerName: "Aktuální množství",
      type: "number",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
    },

    {
      field: "state",
      headerName: "Aktuální stav",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      valueFormatter: (val: ContainerState) => {
        switch (val) {
          case "New":
            return "Nový";
          case "Opened":
            return "Otevřený";
          case "WrittenOff":
            return "Odepsaný";
          case "Bad":
            return "Špatný";
          default:
            return "Neznámý";
        }
      },
    },

    {
      field: "pipe",
      headerName: "Aktuální pípa",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      renderCell: ({ row }) =>
        !row.pipe ? (
          "Žádná"
        ) : (
          <Link to={`/admin/container-templates/${row.pipe.id}`}>
            {row.template.name}
          </Link>
        ),
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
            const confirmed = confirm(`Opravdu chcete tento keg odepsat?`);
            if (confirmed) {
              await handleApiCall(
                containerChangesApi.containerChangesCreate({
                  containerChangeCreateRequest: {
                    containerId: params.row.id,
                    newAmount: params.row.amount,
                    newState: "WrittenOff",
                  },
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
  const refreshContainers = () => setRequest({ ...request });

  return (
    <>
      <Button color="success" variant="contained" onClick={openCreateDialog}>
        Naskladnit kegy
      </Button>

      <Dialog open={createDialogOpen} onClose={closeCreateDialog}>
        <DialogTitle>Naskladnit kegy</DialogTitle>
        <DialogContent>
          <ContainerCreateForm
            storeId={storeId}
            id="containerCreateForm"
            beforeSubmit={closeCreateDialog}
            afterSubmit={refreshContainers}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={closeCreateDialog}>Zrušit</Button>
          <Button type="submit" form="containerCreateForm">
            Naskladnit
          </Button>
        </DialogActions>
      </Dialog>

      <DataGrid
        loading={isLoading}
        sx={{ width: "100%" }}
        rows={containers ?? []}
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
    </>
  );
};

export default ContainerListView;
