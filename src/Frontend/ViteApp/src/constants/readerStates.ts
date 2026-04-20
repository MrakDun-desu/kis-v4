import type { ReaderState } from "kis-reader-lib";

export const readerStates: Record<ReaderState, string> = {
  "0": "Disconnected",
  "1": "Reconnecting",
  "2": "Idle",
  "3": "Auto read",
  "4": "Single read",
  "5": "Unknown",
};

