import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import {
  SaleTransactionsApi,
  type SaleTransactionListModel,
  type SaleTransactionsReadAllRequest,
  type UserListModel,
} from "../../../api-generated";
import { useEffect, useState } from "react";
import { Box, Button } from "@mui/material";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";

const api = new SaleTransactionsApi(defaultConfiguration);

const SaleTransactions = () => {
  const [saleTransactions, setSaleTransactions] = useState<
    SaleTransactionListModel[] | null
  >(null);
  const [request, setRequest] = useState<SaleTransactionsReadAllRequest>({
    page: 1,
  });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [rowCount, setRowCount] = useState<number>(0);
  const navigate = useNavigate();

  useEffect(() => {
    const getSaleTransactionsDeferred = setTimeout(async () => {
      setLoading(true);
      const response = await handleApiCall(
        api.saleTransactionsReadAll(request),
      );
      if (!response) {
        setSaleTransactions(null);
        setRowCount(0);
        setLoading(false);
        return;
      }
      setSaleTransactions(response.data);
      setRowCount(response.meta.total ?? 0);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getSaleTransactionsDeferred);
  }, [request]);

  const columns: GridColDef<SaleTransactionListModel>[] = [
    {
      field: "id",
      headerName: "ID",
      type: "number",
      sortable: false,
      editable: false,
      filterable: false,
    },

    {
      field: "note",
      headerName: "Poznámka",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
    },

    {
      field: "startedAt",
      headerName: "Čas vytvoření",
      type: "dateTime",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      valueFormatter: (val: Date) => val.toLocaleString("cs"),
    },

    {
      field: "startedBy",
      headerName: "Vytvořil",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      valueGetter: (val: UserListModel) => val.nick,
    },

    {
      field: "cancelledAt",
      headerName: "Čas zrušení",
      type: "dateTime",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      valueFormatter: (val: Date | null) =>
        val?.toLocaleString("cs") ?? "Nezrušena",
    },

    {
      field: "cancelledBy",
      headerName: "Zrušil",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      valueGetter: (val: UserListModel) => val?.nick ?? "Nezrušena",
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
              `Opravdu chcete transakci ${params.row.id} zrušit?`,
            );
            if (confirmed) {
              const resp = await handleApiCall(
                api.saleTransactionsDelete({
                  id: params.row.id,
                }),
              );
              if (resp !== null) {
                setRequest({ ...request });
              }
            }
          }}
        >
          Zrušit
        </Button>,
      ],
    },
  ];

  return (
    <>
      <h2>Prodejní transakce</h2>

      <Box
        display="flex"
        gap={2}
        flexDirection="column"
        alignItems="flex-start"
      >
        <DataGrid
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={saleTransactions ?? []}
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
      </Box>
    </>
  );
};

export default SaleTransactions;
