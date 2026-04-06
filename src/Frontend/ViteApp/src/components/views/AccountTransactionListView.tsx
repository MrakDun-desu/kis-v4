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
import type { AccountTransactionModel } from "../../api/apiTypes";
import { accountTransactionTypes } from "../../constants/accountTransactionTypes";

const AccountTransactionListView = ({
  accountTransactions,
}: {
  accountTransactions: AccountTransactionModel[];
}) => {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Změna na účtu</TableCell>

            <TableCell>Účet</TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          {accountTransactions.map((transaction) => (
            <TableRow
              key={`${transaction.saleTransactionId}${transaction.account.id}`}
            >
              <TableCell>
                {Number(transaction.amount) > 0 && "+"}
                {transaction.amount}
                {transaction.account.type === "CashBoxAccount" && "czk"}
              </TableCell>

              <TableCell>
                {transaction.account.type === "UserAccount" && (
                  <>Uživatel: {transaction.account.user.nick}</>
                )}
                {transaction.account.type === "CashBoxAccount" && (
                  <>
                    Kasa:{" "}
                    <Link
                      to={`/admin/cashboxes/${transaction.account.cashBox.id}`}
                    >
                      {transaction.account.cashBox.name}
                    </Link>
                  </>
                )}
                {` - ${accountTransactionTypes[transaction.type]}`}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default AccountTransactionListView;
