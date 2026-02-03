import { Paper } from "@mui/material";
import type { GridColDef } from "@mui/x-data-grid";
import { DataGrid } from "@mui/x-data-grid";

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
    name: "Toustový chleba",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 40.00
  },
  {
    id: 2,
    name: "Šunka",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 200.00
  },
  {
    id: 3,
    name: "Kofola",
    unitName: "l",
    isContainerItem: true,
    currentCost: 20.00
  },
  {
    id: 4,
    name: "Tatranka",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 25.12
  },
  {
    id: 5,
    name: "Toffifee",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 79.90
  },
  {
    id: 6,
    name: "Vafle",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 41
  },
  {
    id: 7,
    name: "Mléko",
    unitName: "l",
    isContainerItem: false,
    currentCost: 10.90
  },
  {
    id: 8,
    name: "Káva",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 200
  },
  {
    id: 9,
    name: "Nachos",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 21
  },
  {
    id: 10,
    name: "Oříšky kešu",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 300
  },
  {
    id: 11,
    name: "Toustový chleba",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 40.00
  },
  {
    id: 12,
    name: "Šunka",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 200.00
  },
  {
    id: 13,
    name: "Kofola",
    unitName: "l",
    isContainerItem: true,
    currentCost: 20.00
  },
  {
    id: 14,
    name: "Tatranka",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 25.12
  },
  {
    id: 15,
    name: "Toffifee",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 79.90
  },
  {
    id: 16,
    name: "Vafle",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 41
  },
  {
    id: 17,
    name: "Mléko",
    unitName: "l",
    isContainerItem: false,
    currentCost: 10.90
  },
  {
    id: 18,
    name: "Káva",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 200
  },
  {
    id: 19,
    name: "Nachos",
    unitName: "ks",
    isContainerItem: false,
    currentCost: 21
  },
  {
    id: 20,
    name: "Oříšky kešu",
    unitName: "kg",
    isContainerItem: false,
    currentCost: 300
  },
]

const columns: GridColDef<StoreItem>[] = [
  {
    field: 'id',
    headerName: "ID",
    sortable: false,
    type: "number",
    editable: false
  },
  {
    field: 'name',
    headerName: "Název",
    sortable: false,
    filterable: false,
    type: "string",
    editable: false,
    flex: 1
  },
  {
    field: 'unitName',
    headerName: "Jednotka",
    sortable: false,
    filterable: false,
    type: "string",
    editable: false,
    flex: 1
  },
  {
    field: 'currentCost',
    headerName: "Cena za jednotku",
    sortable: false,
    filterable: false,
    type: "number",
    editable: false,
    flex: 1,
    valueFormatter(value: number) {
      return `${value} czk`;
    }
  },
  {
    field: 'isContainerItem',
    headerName: "Kegová položka",
    sortable: false,
    filterable: false,
    type: "boolean",
    editable: false,
    flex: 1
  },
]

export const StoreItems = () => {
  return <>
    <h2>Skladové položky</h2>
    <Paper>
      <DataGrid
        rows={testData}
        columns={columns}
        initialState={{
          pagination: { paginationModel: { page: 0, pageSize: 10 } }
        }}
        pageSizeOptions={[10, 20, 50, 100]}
      />
    </Paper>
  </>;
};
