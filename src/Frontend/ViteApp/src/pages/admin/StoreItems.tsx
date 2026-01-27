import { Paper } from "@mui/material";
import { csCZ } from "@mui/x-data-grid/locales";
import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid, getGridNumericOperators, getGridStringOperators } from "@mui/x-data-grid";

interface StoreItem {
  id: number,
  name: string,
  unitName: string,
  isContainerItem: boolean,
  currentCost: number
}

const testData: StoreItem[] = [
  {
    id: 1,
    name: "Chleba",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 40
  },
  {
    id: 2,
    name: "Šunka",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 200
  },
  {
    id: 3,
    name: "Kofola",
    unitName: "l",
    isContainerItem: true,
    currentCost: 20
  },
]

const columns: GridColDef<StoreItem>[] = [
  {
    field: 'id',
    headerName: "ID",
    sortable: false,
    type: "number",
    width: 50,
  },
  {
    field: 'name',
    headerName: "Název",
    sortable: false,
    type: "string",
    width: 120,
    filterOperators: getGridStringOperators().filter(val => val.value === "contains")
  },
  {
    field: 'unitName',
    headerName: "Jednotka",
    sortable: false,
    filterable: false,
    type: "number",
    width: 80
  },
  {
    field: 'isContainerItem',
    headerName: "Kegová položka",
    sortable: false,
    filterable: false,
    type: "boolean",
    width: 120,
  },
  {
    field: 'currentCost',
    headerName: "Aktuální cena",
    sortable: false,
    filterable: false,
    type: "number",
    width: 110,
  },
]

export const StoreItems = () => {
  return <>
    <h2>Skladové položky</h2>
    <Paper>
      <DataGrid
        localeText={csCZ.components.MuiDataGrid.defaultProps.localeText}
        rows={testData}
        columns={columns}
        initialState={{
          pagination: { paginationModel: { page: 0, pageSize: 30 } }
        }}
        onRowClick={(params) => {
          var row: StoreItem = params.row;
          console.log(row.id);
        }}
      />
    </Paper>
  </>;
};
