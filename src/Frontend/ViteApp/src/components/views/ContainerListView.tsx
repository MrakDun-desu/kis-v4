import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { useEffect, useRef, useState } from "react";
import {
  Button,
  Checkbox,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControlLabel,
} from "@mui/material";
import { csCZ } from "@mui/x-data-grid/locales";
import { Link, useNavigate } from "react-router-dom";
import {
  ContainersApi,
  type ContainerReadAllResponse,
  type ContainerListModel,
  type ContainersReadAllRequest,
  ContainerState,
  type ContainerCreateResponse,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";
import ContainerCreateForm from "../forms/ContainerCreateForm";
import PipeFilter from "../filters/PipeFilter";
import StoreFilter from "../filters/StoreFilter";
import ContainerTemplateFilter from "../filters/ContainerTemplateFilter";
import { containerStates } from "../../constants/containerStates";

const api = new ContainersApi(defaultConfiguration);

const ContainerListView = ({
  storeId,
  initialContainers,
  showPipeFilter,
  showUnusableFilter,
  showTemplateFilter,
  afterCreate,
}: {
  storeId?: number;
  initialContainers?: ContainerReadAllResponse;
  showPipeFilter?: boolean;
  showUnusableFilter?: boolean;
  showTemplateFilter?: boolean;
  afterCreate?: (resp: ContainerCreateResponse) => void;
}) => {
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
  const firstRender = useRef(true);
  const navigate = useNavigate();

  useEffect(() => {
    // In production, this will work and prevent useless fetching.
    // In development, hooks run twice, so the containers are still fetched once for no reason.
    if (firstRender.current && initialContainers !== undefined) {
      setContainers(initialContainers.data);
      setRowCount(initialContainers.meta.total);
      firstRender.current = false;
      return;
    }
    firstRender.current = false;
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
      renderCell: ({ row }) => row.template.name,
    },

    {
      field: "amount",
      headerName: "Aktuální množství",
      type: "number",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      renderCell: ({ row }) =>
        `${row.amount} ${row.template.storeItem.unitName}`,
    },

    {
      field: "state",
      headerName: "Aktuální stav",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      valueFormatter: (val: ContainerState) => containerStates[val],
    },

    {
      field: "pipe",
      headerName: "Aktuální pípa",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      renderCell: ({ row }) => (!row.pipe ? "Žádná" : row.pipe.name),
      //<Link to={`/admin/taps/${row.pipe.id}`}>{row.pipe.name}</Link>
    },

    {
      field: "actions",
      headerName: "Akce",
      flex: 1,
      type: "actions",
      renderCell: (params) => [
        <Button
          variant="contained"
          onClick={() => navigate(`/admin/containers/${params.row.id}`)}
        >
          Detail
        </Button>,
      ],
    },
  ];

  if (storeId === undefined) {
    columns.splice(2, 0, {
      field: "store",
      headerName: "Sklad",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      renderCell: ({ row }) => (
        <Link to={`/admin/stores/${row.store.id}`}>{row.store.name}</Link>
      ),
    });
  }

  const openCreateDialog = () => setCreateDialogOpen(true);
  const closeCreateDialog = () => setCreateDialogOpen(false);
  const refreshContainers = () => setRequest({ ...request });

  return (
    <>
      <div>
        <Button color="success" variant="contained" onClick={openCreateDialog}>
          Naskladnit kegy
        </Button>
      </div>

      {showPipeFilter && (
        <PipeFilter
          onChange={(pipeId) => setRequest((prev) => ({ ...prev, pipeId }))}
        />
      )}

      {showTemplateFilter && (
        <ContainerTemplateFilter
          onChange={(templateId) =>
            setRequest((prev) => ({ ...prev, templateId }))
          }
        />
      )}

      {storeId === undefined && (
        <StoreFilter
          onChange={(storeId) => setRequest((prev) => ({ ...prev, storeId }))}
        />
      )}

      {showUnusableFilter && (
        <FormControlLabel
          label="Zobrazit staré kegy"
          control={
            <Checkbox
              value={request.includeUnusable}
              onChange={(evt) => {
                setRequest((prev) => ({
                  ...prev,
                  includeUnusable: evt.target.checked,
                }));
              }}
            />
          }
        />
      )}

      <Dialog open={createDialogOpen} onClose={closeCreateDialog}>
        <DialogTitle>Naskladnit kegy</DialogTitle>
        <DialogContent>
          <ContainerCreateForm
            storeId={storeId}
            id="containerCreateForm"
            beforeSubmit={closeCreateDialog}
            afterSubmit={(resp) => {
              refreshContainers();
              afterCreate?.(resp);
            }}
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
              page: request.page ?? 0,
              pageSize: request.pageSize ?? 30,
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
          setRequest((prev) => {
            return {
              ...prev,
              page: newModel.page + 1,
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
