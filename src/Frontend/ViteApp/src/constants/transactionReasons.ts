import type { TransactionReason } from "../api-generated";

export const transactionReasons: Record<TransactionReason, string> = {
  AddingToStore: "Přidání do skladu",
  ChangingStores: "Přesun mezi sklady",
  WriteOff: "Odpis",
  StockTaking: "Inventura",
  Sale: "Prodej"
}
