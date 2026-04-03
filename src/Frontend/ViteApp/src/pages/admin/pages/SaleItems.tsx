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
import { getGridStringOperators } from "@mui/x-data-grid";
import { csCZ } from "@mui/x-data-grid/locales";
import { useNavigate } from "react-router-dom";
import { printTypes } from "../../../constants/printTypes";
import CategoryFilter from "../../../components/filters/CategoryFilter";
import SaleItemCreateForm from "../../../components/forms/SaleItemCreateForm";
import type { SaleItemListModel, PrintType } from "../../../api/apiTypes";
import type { operations } from "../../../api/apiSchema";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

type Query = operations["SaleItemsReadAll"]["parameters"]["query"];

const SaleItems = () => {
  const [saleItems, setSaleItems] = useState<SaleItemListModel[]>();
  const [query, setQuery] = useState<Query>({ Page: 1 });
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [rowCount, setRowCount] = useState<number>(0);
  const navigate = useNavigate();

  useEffect(() => {
    const getSaleItemsDeferred = setTimeout(async () => {
      setLoading(true);
      const { response, data } = await apiClient.GET("/sale-items");
      setSaleItems(data?.data);
      if (!data) {
        setRowCount(0);
        handleApiError(response);
      } else {
        setRowCount(data.meta.total ?? 0);
      }
      setLoading(false);
    }, 500);
    return () => clearTimeout(getSaleItemsDeferred);
  }, [query]);

  const columns: GridColDef<SaleItemListModel>[] = [
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
      filterable: true,
      filterOperators: getGridStringOperators().filter(
        (operator) => operator.value === "contains",
      ),
      editable: false,
      flex: 1,
    },

    {
      field: "marginPercent",
      headerName: "Procentuální marže",
      type: "number",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      valueFormatter(value: number) {
        return `${value}%`;
      },
    },

    {
      field: "marginStatic",
      headerName: "Statická marže",
      type: "number",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      valueFormatter(value: number) {
        return `${value} czk`;
      },
    },

    {
      field: "prestigeAmount",
      headerName: "Prestiž",
      type: "number",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
    },

    {
      field: "printType",
      headerName: "Tisknout?",
      type: "string",
      sortable: false,
      filterable: false,
      editable: false,
      flex: 1,
      valueFormatter: (value: PrintType) => printTypes[value],
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
              const { response } = await apiClient.DELETE("/sale-items/{id}", {
                params: { path: { id: params.row.id } },
              });
              if (!response.ok) {
                handleApiError(response);
              } else {
                setQuery({ ...query });
              }
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
  const refreshSaleItems = () => setQuery({ ...query });

  return (
    <>
      <h2>Prodejní položky</h2>

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
          <DialogTitle>Vytvořit novou skladovou položku</DialogTitle>
          <DialogContent>
            <SaleItemCreateForm
              id="saleItemCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshSaleItems}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="saleItemCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <CategoryFilter
          onChange={(categoryId) =>
            setQuery((prev) => ({ ...prev, CategoryId: categoryId }))
          }
        />

        <DataGrid
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={saleItems ?? []}
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
          onFilterModelChange={(newFilters) => {
            setQuery((prev) => ({
              ...prev,
              Name: newFilters.items.find((f) => f.field === "name")?.value,
            }));
          }}
          localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        />
      </Box>
    </>
  );
};

export default SaleItems;
