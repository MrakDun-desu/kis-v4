import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import { Link } from "react-router-dom";
import type {
  StoreTransactionItemModel,
  TransactionReason,
} from "../../api/apiTypes";

const StoreTransactionItemListView = ({
  transactionReason,
  transactionItems,
}: {
  transactionItems: StoreTransactionItemModel[];
  transactionReason: TransactionReason;
}) => {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Skladová položka</TableCell>

            <TableCell>Sklad</TableCell>

            <TableCell>Změna v množství</TableCell>

            {transactionReason === "AddingToStore" && (
              <TableCell>Nákupní cena</TableCell>
            )}
          </TableRow>
        </TableHead>

        <TableBody>
          {transactionItems.map((item) => (
            <TableRow key={`${item.store.id},${item.storeItem.id}`}>
              <TableCell>
                <Link to={`/admin/store-items/${item.storeItem.id}`}>
                  {item.storeItem.name}
                </Link>
              </TableCell>

              <TableCell>
                <Link to={`/admin/stores/${item.store.id}`}>
                  {item.store.name}
                </Link>
              </TableCell>

              <TableCell>
                {Number(item.itemAmount) > 0 && "+"}
                {item.itemAmount} {item.storeItem.unitName}
              </TableCell>

              {transactionReason === "AddingToStore" && (
                <TableCell>{item.cost}</TableCell>
              )}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default StoreTransactionItemListView;
