import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { useEffect, useState } from "react";
import { Box, Button } from "@mui/material";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";
import type {
  SaleTransactionListModel,
  UserListModel,
} from "../../../api/apiTypes";
import type { operations } from "../../../api/apiSchema";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

type Query = operations["SaleTransactionsReadAll"]["parameters"]["query"];

const SaleTransactions = () => {
  const [saleTransactions, setSaleTransactions] =
    useState<SaleTransactionListModel[]>();
  const [query, setQuery] = useState<Query>({
    Page: 1,
  });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [rowCount, setRowCount] = useState<number>(0);
  const navigate = useNavigate();

  useEffect(() => {
    const getSaleTransactionsDeferred = setTimeout(async () => {
      setLoading(true);
      const { data, response } = await apiClient.GET("/sale-transactions", {
        params: { query },
      });
      setSaleTransactions(data?.data);
      if (!data) {
        setRowCount(0);
        handleApiError(response);
      } else {
        setRowCount(data.meta.total);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getSaleTransactionsDeferred);
  }, [query]);

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
      valueFormatter: (val: string) => new Date(val).toLocaleString("cs"),
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
      valueFormatter: (val: string | null) =>
        val ? new Date(val).toLocaleString("cs") : "Nezrušena",
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
              const { response } = await apiClient.DELETE(
                "/sale-transactions/{id}",
                { params: { path: { id: params.row.id } } },
              );
              if (response.ok) {
                setQuery({ ...query });
              } else {
                handleApiError(response);
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
            setQuery((prev) => {
              return {
                ...prev,
                Page: newModel.page + 1,
                PageSize: newModel.pageSize,
              };
            });
          }}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />
      </Box>
    </>
  );
};

export default SaleTransactions;
