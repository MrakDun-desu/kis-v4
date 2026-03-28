import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { CashBoxesApi, type CashBoxListModel } from "../../../api-generated";
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
import { useNavigate } from "react-router-dom";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import CashBoxCreateForm from "../../../components/forms/CashBoxCreateForm";

const api = new CashBoxesApi(defaultConfiguration);

const CashBoxes = () => {
  const [cashBoxes, setCashBoxes] = useState<CashBoxListModel[] | null>(null);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    const getCashBoxesDeferred = setTimeout(async () => {
      const response = await handleApiCall(api.cashBoxesReadAll());
      if (!response) {
        setCashBoxes(null);
        setLoading(false);
        return;
      }
      setCashBoxes(response.data);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getCashBoxesDeferred);
  }, [refreshCounter]);

  const columns: GridColDef<CashBoxListModel>[] = [
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
                api.cashBoxesDelete({
                  id: params.row.id,
                }),
              );
              refreshCashBoxes();
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
  const refreshCashBoxes = () => setRefreshCounter((val) => val + 1);

  return (
    <>
      <h2>Kasy</h2>

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
            <CashBoxCreateForm
              id="cashBoxCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshCashBoxes}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="cashBoxCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <DataGrid
          rowSelection={false}
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={cashBoxes ?? []}
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

export default CashBoxes;
