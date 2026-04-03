import type { TransactionReason } from "../api/apiTypes";

export const transactionReasons: Record<TransactionReason, string> = {
  AddingToStore: "Přidání do skladu",
  ChangingStores: "Přesun mezi sklady",
  WriteOff: "Odpis",
  StockTaking: "Inventura",
  Sale: "Prodej"
}
