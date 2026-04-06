import type { AccountTransactionType } from "../api/apiTypes";

export const accountTransactionTypes: Record<AccountTransactionType, string> = {
  SalesMoney: "prodej",
  DonationMoney: "příspěvek",
  Prestige: "prestiž",
  StockTaking: "inventura",
  Deposit: "vklad",
  Withdrawal: "výběr",
  Transfer: "přesun"
};

