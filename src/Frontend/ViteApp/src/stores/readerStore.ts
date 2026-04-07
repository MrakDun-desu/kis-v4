import { ReaderState } from "kis-reader-lib";
import { create } from "zustand";

const readerUriLsKey = "readerUri";
const readerUri = localStorage.getItem(readerUriLsKey) ?? undefined;

interface ReaderStoreType {
  readerUri?: string;
  currentCard?: string;
  readerState: ReaderState;
  setReaderState: (val: ReaderState) => void
  setReaderUri: (val: string) => void
  setCurrentCard: (val?: string) => void
}

export const useReaderStore = create<ReaderStoreType>((set) => ({
  readerUri: readerUri,
  readerState: ReaderState.ST_UNKNOWN,
  setReaderState: (val) => set({ readerState: val }),
  setReaderUri: (val) => set(() => {
    localStorage.setItem(readerUriLsKey, val);
    return {
      readerUri: val
    }
  }),
  setCurrentCard: (val) => set({ currentCard: val })
}))
