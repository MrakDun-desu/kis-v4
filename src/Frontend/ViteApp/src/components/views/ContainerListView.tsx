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
import ContainerCreateForm from "../forms/ContainerCreateForm";
import StoreFilter from "../filters/StoreFilter";
import ContainerTemplateFilter from "../filters/ContainerTemplateFilter";
import { containerStates } from "../../constants/containerStates";
import type {
  ContainerCreateResponse,
  ContainerListModel,
  ContainerReadAllResponse,
  ContainerState,
} from "../../api/apiTypes";
import type { operations } from "../../api/apiSchema";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

type Query = operations["ContainersReadAll"]["parameters"]["query"];

const ContainerListView = ({
  storeId,
  initialContainers,
  showUnusableFilter,
  showTemplateFilter,
  afterCreate,
}: {
  storeId?: number;
  initialContainers?: ContainerReadAllResponse;
  showUnusableFilter?: boolean;
  showTemplateFilter?: boolean;
  afterCreate?: (resp: ContainerCreateResponse) => void;
}) => {
  const [containers, setContainers] = useState<ContainerListModel[]>();
  const [query, setQuery] = useState<Query>({
    Page: 1,
    StoreId: storeId,
    IncludeUnusable: false,
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
      const { response, data, error } = await apiClient.GET("/containers", {
        params: { query },
      });
      if (!response.ok) {
        handleApiError(response, error);
      }
      setContainers(data?.data);
      setRowCount(data?.meta.total ?? 0);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getContainersDeferred);
  }, [query]);

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
      field: "tap",
      headerName: "Aktuální pípa",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      renderCell: ({ row }) => (!row.tap ? "Žádná" : row.tap.name),
      //<Link to={`/admin/taps/${row.tap.id}`}>{row.tap.name}</Link>
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
  const refreshContainers = () => setQuery({ ...query });

  return (
    <>
      <div>
        <Button color="success" variant="contained" onClick={openCreateDialog}>
          Naskladnit kegy
        </Button>
      </div>

      {showTemplateFilter && (
        <ContainerTemplateFilter
          onChange={(templateId) =>
            setQuery((prev) => ({ ...prev, TemplateId: templateId }))
          }
        />
      )}

      {storeId === undefined && (
        <StoreFilter
          onChange={(storeId) =>
            setQuery((prev) => ({ ...prev, StoreId: storeId }))
          }
        />
      )}

      {showUnusableFilter && (
        <FormControlLabel
          label="Zobrazit staré kegy"
          control={
            <Checkbox
              value={query?.IncludeUnusable}
              onChange={(evt) => {
                setQuery((prev) => ({
                  ...prev,
                  IncludeUnusable: evt.target.checked,
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
        localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
      />
    </>
  );
};

export default ContainerListView;
