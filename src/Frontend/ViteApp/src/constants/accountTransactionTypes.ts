import type { AccountTransactionType } from "../api/apiTypes";

export const accountTransactionTypes: Record<AccountTransactionType, string> = {
  DonationMoney: "Příspěvek",
  Prestige: "Prestiž",
  SalesMoney: "Prodej",
  StockTaking: "Inventura"
};

