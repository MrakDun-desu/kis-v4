import type { ReactNode } from "react";

type Severity = "success" | "info" | "warning" | "error";

type ShowSnackBarFunc = (message: string | ReactNode, severity: Severity, title?: string) => void

export const snackbarRef: {
  show?: ShowSnackBarFunc
} = {};
