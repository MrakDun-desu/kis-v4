import { Paper } from "@mui/material";
import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";
import type { StoreItemListModel } from "../../../api-generated";

const columns: GridColDef<StoreItemListModel>[] = [
  {
    field: "id",
    headerName: "ID",
    sortable: false,
    type: "number",
    editable: false,
  },
  {
    field: "name",
    headerName: "Název",
    sortable: false,
    filterable: false,
    type: "string",
    editable: false,
    flex: 1,
  },
  {
    field: "unitName",
    headerName: "Jednotka",
    sortable: false,
    filterable: false,
    type: "string",
    editable: false,
    flex: 1,
  },
  {
    field: "currentCost",
    headerName: "Cena za jednotku",
    sortable: false,
    filterable: false,
    type: "number",
    editable: false,
    flex: 1,
    valueFormatter(value: number) {
      return `${value} czk`;
    },
  },
  {
    field: "isContainerItem",
    headerName: "Kegová položka",
    sortable: false,
    filterable: false,
    type: "boolean",
    editable: false,
    flex: 1,
  },
];

export const StoreItems = () => {
  return (
    <>
      <h2>Skladové položky</h2>
      <Paper>
        <DataGrid
          rows={[]}
          columns={columns}
          initialState={{
            pagination: { paginationModel: { page: 0, pageSize: 10 } },
          }}
          pageSizeOptions={[10, 20, 50, 100]}
        />
      </Paper>
    </>
  );
};
