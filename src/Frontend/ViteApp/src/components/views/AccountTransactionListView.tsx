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
                {transaction.type.endsWith("Money") && "czk"}
              </TableCell>

              <TableCell>
                {transaction.account.type === "UserAccount" && (
                  <>
                    Uživatel: {transaction.account.user.nick}
                    {transaction.type === "Prestige" && " (prestiž)"}
                  </>
                )}
                {transaction.account.type === "CashBoxAccount" && (
                  <>
                    Kasa:{" "}
                    <Link
                      to={`/admin/cashboxes/${transaction.account.cashBox.id}`}
                    >
                      {transaction.account.cashBox.name}
                    </Link>
                    {transaction.type === "SalesMoney" && " (prodej)"}
                    {transaction.type === "DonationMoney" && " (příspěvek)"}
                  </>
                )}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default AccountTransactionListView;
