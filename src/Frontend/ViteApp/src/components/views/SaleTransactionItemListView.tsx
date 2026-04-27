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
import type { SaleTransactionItemModel } from "../../api/apiTypes";

const SaleTransactionItemListView = ({
  transactionItems,
}: {
  transactionItems: SaleTransactionItemModel[];
}) => {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Číslo položky</TableCell>

            <TableCell>Prodejní položka</TableCell>

            <TableCell>Prodané množství</TableCell>

            <TableCell>Celková cena položky</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {transactionItems.map((item) => (
            <TableRow key={`${item.lineNumber}`}>
              <TableCell>{item.lineNumber}</TableCell>

              <TableCell>
                <Link to={`/admin/sale-items/${item.saleItemId}`}>
                  {item.saleItemName}
                </Link>
              </TableCell>

              <TableCell>{item.amount} ks</TableCell>

              <TableCell>
                {Number(item.basePrice) * item.amount +
                  item.modifications.reduce(
                    (acc, m) =>
                      acc + m.amount * Number(m.priceChange) * item.amount,
                    0,
                  )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default SaleTransactionItemListView;
