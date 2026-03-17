import type { PrintType } from "../api-generated";

export const printTypes: Record<PrintType, string> = {
  DontPrint: "Netisknout",
  PrintForCustomer: "Tisknout pro zákazníka",
  PrintForEmployee: "Tisknout pro barmana",
  PrintForBoth: "Tisknout pro oba",
};

