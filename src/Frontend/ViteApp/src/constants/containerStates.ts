import type { ContainerState } from "../api/apiTypes";

export const containerStates: Record<ContainerState, string> = {
  Bad: "Špatný",
  New: "Nový",
  Opened: "Naražený",
  WrittenOff: "Odepsaný"
}
