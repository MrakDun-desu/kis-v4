import { create } from "zustand";
import type { LayoutReadResponse } from "../api/apiTypes";

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

interface PosStoreType {
  transactionItems: SaleTransactionItemDisplay[];
  currentStore?: EntityWithName;
  currentCashBox?: EntityWithName;
  currentLayout?: LayoutReadResponse;
  currentLayoutId?: number;
  layoutHistory: number[];
  readerUri?: string;
  setCurrentLayout: (data: LayoutReadResponse | undefined) => void;
  setLayoutId: (val: number | undefined) => void;
  addTransactionItem: (item: SaleTransactionItemDisplay) => void;
  removeTransactionItem: (index: number) => void;
  updateTransactionItem: (index: number, update: Partial<SaleTransactionItemDisplay>) => void;
  clearTransactionItems: () => void;
  clearLayoutHistory: () => void;
  popLayoutHistory: () => void;
  updateMetadata: (
    cashBox: EntityWithName | undefined,
    store: EntityWithName | undefined,
    readerUri: string | undefined
  ) => void
}

export const usePosStore = create<PosStoreType>((set) => ({
  transactionItems: [],
  layoutHistory: [],
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
  setLayoutId: (val) => set(() => ({ currentLayoutId: val })),
  addTransactionItem: (item) => set(({ transactionItems: prev }) =>
    ({ transactionItems: [...prev, item] })
  ),
  removeTransactionItem: (index) => set(({ transactionItems: prev }) =>
    ({ transactionItems: prev.filter((_, i) => i !== index) })
  ),
  updateTransactionItem: (index, update) => set(({ transactionItems: prev }) =>
    ({ transactionItems: prev.map((val, i) => i === index ? { ...val, ...update } : val) })
  ),
  clearTransactionItems: () => set(() => ({ transactionItems: [] })),
  clearLayoutHistory: () => set(() => ({ layoutHistory: [] })),
  popLayoutHistory: () => set(({ layoutHistory: prev }) =>
    ({ layoutHistory: prev.slice(0, Math.max(prev.length - 1, 0)) })),
  updateMetadata: ((cashBox, store, readerUri) => set(() => ({
    currentCashBox: cashBox,
    currentStore: store,
    readerUri: readerUri
  })))
}))
