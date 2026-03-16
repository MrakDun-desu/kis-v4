import { create } from "zustand";
import type { SaleTransactionItemCreateRequest } from "../api-generated";

interface ModificationDisplay {
  amount: number;
  modifierId: number;
  modifierName: string;
}

interface SaleTransactionItemDisplay {
  saleItemId: number;
  saleItemName: string;
  amount: number;
  modifications?: ModificationDisplay[]
}

interface PosStoreType {
  transactionItems: SaleTransactionItemDisplay[];
  currentStoreId?: number;
  currentCashBoxId?: number;
  currentLayoutId?: number;
  setCashBox: (id: number) => void;
  setStoreId: (id: number) => void;
  setCurrentLayout: (id: number) => void;
  addTransactionItem: (item: SaleTransactionItemDisplay) => void;
  removeTransactionItem: (index: number) => void;
  updateTransactionItem: (index: number, update: Partial<SaleTransactionItemDisplay>) => void;
  clearTransactionItems: () => void;
}

const usePosStore = create<PosStoreType>((set) => ({
  transactionItems: [{
    amount: 42,
    saleItemId: 1,
    saleItemName: "Testovací prodejní položka",
    modifications: [
      {
        amount: 1,
        modifierId: 1,
        modifierName: "Testovací modifikátor"
      }
    ]
  }],
  setCashBox: (id) => set(() => ({ currentCashBoxId: id })),
  setStoreId: (id) => set(() => ({ currentStoreId: id })),
  setCurrentLayout: (id) => set(() => ({ currentLayoutId: id })),
  addTransactionItem: (item) => set(({ transactionItems: prev }) =>
    ({ transactionItems: [...prev, item] })
  ),
  removeTransactionItem: (index) => set(({ transactionItems: prev }) =>
    ({ transactionItems: prev.filter((_, i) => i !== index) })
  ),
  updateTransactionItem: (index, update) => set(({ transactionItems: prev }) =>
    ({ transactionItems: prev.map((val, i) => i === index ? { ...val, ...update } : val) })
  ),
  clearTransactionItems: () => set(() => ({ transactionItems: [] }))
}))

export default usePosStore;
