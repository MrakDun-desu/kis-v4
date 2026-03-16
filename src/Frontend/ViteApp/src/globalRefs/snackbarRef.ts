type Severity = "success" | "info" | "warning" | "error";

type ShowSnackBarFunc = (message: string, severity: Severity) => void

export const snackbarRef: {
  show?: ShowSnackBarFunc
} = {};
