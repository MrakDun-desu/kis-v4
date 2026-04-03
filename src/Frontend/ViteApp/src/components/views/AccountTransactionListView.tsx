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
                {transaction.account.kind === "CashBoxAccount" && "czk"}
              </TableCell>

              <TableCell>
                {transaction.account.kind === "UserAccount" && (
                  <>
                    Uživatel: {transaction.account.user.nick}
                    {transaction.account.type === "Prestige" && " (prestiž)"}
                  </>
                )}
                {transaction.account.kind === "CashBoxAccount" && (
                  <>
                    Kasa:{" "}
                    <Link
                      to={`/admin/cash-boxes/${transaction.account.cashBox.id}`}
                    >
                      {transaction.account.cashBox.name}
                    </Link>
                    {transaction.account.type === "SalesMoney" && " (prodej)"}
                    {transaction.account.type === "DonationMoney" &&
                      " (příspěvek)"}
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
