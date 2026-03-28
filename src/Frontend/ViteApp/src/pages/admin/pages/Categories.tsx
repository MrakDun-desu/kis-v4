import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import { CategoriesApi, type CategoryModel } from "../../../api-generated";
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
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import CategoryCreateForm from "../../../components/forms/CategoryCreateForm";

const api = new CategoriesApi(defaultConfiguration);

const Categories = () => {
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const [isLoading, setLoading] = useState<boolean>(true);
  const [createDialogOpen, setCreateDialogOpen] = useState<boolean>(false);
  const [refreshCounter, setRefreshCounter] = useState(0);

  useEffect(() => {
    setLoading(true);
    const getCategoriesDeferred = setTimeout(async () => {
      const response = await handleApiCall(api.categoriesReadAll());
      if (!response) {
        setCategories(null);
        setLoading(false);
        return;
      }
      setCategories(response.data);
      setLoading(false);
    }, 500);
    return () => clearTimeout(getCategoriesDeferred);
  }, [refreshCounter]);

  const columns: GridColDef<CategoryModel>[] = [
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
                api.categoriesDelete({
                  id: params.row.id,
                }),
              );
              refreshCategories();
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
  const refreshCategories = () => setRefreshCounter((val) => val + 1);

  return (
    <>
      <h2>Kategorie</h2>

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
            <CategoryCreateForm
              id="categoryCreateForm"
              beforeSubmit={closeCreateDialog}
              afterSubmit={refreshCategories}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={closeCreateDialog}>Zrušit</Button>
            <Button type="submit" form="categoryCreateForm">
              Vytvořit
            </Button>
          </DialogActions>
        </Dialog>

        <DataGrid
          rowSelection={false}
          loading={isLoading}
          sx={{ width: "100%" }}
          rows={categories ?? []}
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

export default Categories;
