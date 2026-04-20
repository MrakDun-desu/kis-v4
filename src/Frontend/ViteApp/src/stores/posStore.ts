import { create } from "zustand";
import type { LayoutReadResponse } from "../api/apiTypes";
import type { UserDetails } from "../auth/AuthContext";

interface ModificationDisplay {
  amount: number;
  modifierId: number;
  modifierName: string;
}

export interface SaleTransactionItemDisplay {
  saleItemId: number;
  saleItemName: string;
  amount: number;
  modifications: ModificationDisplay[]
}

export interface EntityWithName {
  id: number;
  name: string;
}

type PosStoreType = {
  transactionItems: SaleTransactionItemDisplay[];
  layoutHistory: number[];
  currentStore?: EntityWithName;
  currentCashBox?: EntityWithName;
  currentLayout?: LayoutReadResponse;
  currentLayoutId?: number;
  customerData?: UserDetails;
  sellForFree: boolean;
  setCustomerData: (data?: UserDetails) => void;
  setCurrentLayout: (data?: LayoutReadResponse) => void;
  setLayoutId: (val?: number) => void;
  addTransactionItem: (item: SaleTransactionItemDisplay) => void;
  removeTransactionItem: (index: number) => void;
  updateTransactionItem: (index: number, update: Partial<SaleTransactionItemDisplay>) => void;
  clearTransactionItems: () => void;
  clearLayoutHistory: () => void;
  popLayoutHistory: () => void;
  updateMetadata: (
    cashBox?: EntityWithName,
    store?: EntityWithName,
  ) => void,
  setSellForFree: (val: boolean) => void;
}

export const usePosStore = create<PosStoreType>((set) => ({
  transactionItems: [],
  layoutHistory: [],
  sellForFree: false,
  setCurrentLayout: (data) => set(({ layoutHistory }) => {
    if (data) {
      return {
        currentLayout: data,
        layoutHistory: layoutHistory[layoutHistory.length - 1] === data.id
          ? layoutHistory
          : [...layoutHistory, data.id],
        currentLayoutId: data.id
      }
    } else {
      return {
        currentLayout: data
      }
    }
  }),
  setCustomerData: (val) => set({ customerData: val }),
  setLayoutId: (val) => set({ currentLayoutId: val }),
  addTransactionItem: (item) => set(({ transactionItems: prev }) =>
    ({ transactionItems: [...prev, item] })
  ),
  removeTransactionItem: (index) => set(({ transactionItems: prev }) =>
    ({ transactionItems: prev.filter((_, i) => i !== index) })
  ),
  updateTransactionItem: (index, update) => set(({ transactionItems: prev }) =>
    ({ transactionItems: prev.map((val, i) => i === index ? { ...val, ...update } : val) })
  ),
  clearTransactionItems: () => set({ transactionItems: [] }),
  clearLayoutHistory: () => set({ layoutHistory: [] }),
  popLayoutHistory: () => set(({ layoutHistory: prev }) =>
    ({ layoutHistory: prev.slice(0, Math.max(prev.length - 1, 0)) })),
  updateMetadata: ((cashBox, store) => set({
    currentCashBox: cashBox,
    currentStore: store,
  })),
  setSellForFree: (val) => set({ sellForFree: val })
}))
