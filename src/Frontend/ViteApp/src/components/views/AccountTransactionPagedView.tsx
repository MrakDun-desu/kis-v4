import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { useEffect, useRef, useState } from "react";
import { csCZ } from "@mui/x-data-grid/locales";
import type { operations } from "../../api/apiSchema";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";
import type {
  AccountTransactionModel,
  AccountTransactionReadAllResponse,
} from "../../api/apiTypes";
import { Link } from "react-router-dom";
import { Typography } from "@mui/material";

type Query = operations["AccountTransactionsReadAll"]["parameters"]["query"];

const AccountTransactionPagedView = ({
  initialTransactions,
}: {
  initialTransactions: AccountTransactionReadAllResponse;
}) => {
  const [transactions, setAccountTransactions] =
    useState<AccountTransactionReadAllResponse>();
  const [query, setQuery] = useState<Query>({
    Page: 1,
    AccountId: initialTransactions.accountId,
  });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [rowCount, setRowCount] = useState<number>(0);
  const firstRender = useRef(true);

  useEffect(() => {
    // In production, this will work and prevent useless fetching.
    // In development, hooks run twice, so the containers are still fetched once for no reason.
    if (firstRender.current && initialTransactions !== undefined) {
      setAccountTransactions(initialTransactions);
      setRowCount(initialTransactions.meta.total);
      firstRender.current = false;
      return;
    }
    firstRender.current = false;
    const getAccountTransactionsDeferred = setTimeout(async () => {
      setLoading(true);
      const { response, data, error } = await apiClient.GET(
        "/account-transactions",
        {
          params: { query },
        },
      );
      if (!response.ok) {
        handleApiError(response, error);
      }
      setAccountTransactions(data);
      setRowCount(data?.meta.total ?? 0);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getAccountTransactionsDeferred);
  }, [query]);

  const columns: GridColDef<AccountTransactionModel>[] = [
    {
      field: "amount",
      headerName: "Změna",
      type: "number",
      sortable: false,
      editable: false,
      filterable: false,
      width: 100,
      valueFormatter: (val: string) => `${Number(val) > 0 && "+"}${val}czk`,
    },
    {
      field: "timestamp",
      headerName: "Čas změny",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      flex: 1,
      valueFormatter: (val: string) => new Date(val).toLocaleString("cs"),
    },

    {
      field: "saleTransactionId",
      headerName: "ID transakce",
      type: "string",
      sortable: false,
      editable: false,
      filterable: false,
      width: 110,
      renderCell: ({ row }) => (
        <Link to={`/admin/sale-transactions/${row.saleTransactionId}`}>
          {row.saleTransactionId}
        </Link>
      ),
    },
  ];

  return (
    <>
      <Typography>Aktuálně v kase: {transactions?.total}czk</Typography>
      <DataGrid
        loading={isLoading}
        sx={{ width: "100%" }}
        rows={transactions?.data ?? []}
        rowCount={rowCount}
        getRowId={(row) => `${row.saleTransactionId},${row.account}`}
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

export default AccountTransactionPagedView;
