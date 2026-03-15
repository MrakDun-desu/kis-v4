import { createContext, useContext, useState, type ReactNode } from 'react';
import Snackbar from '@mui/material/Snackbar'; // or your own component
import { Alert } from '@mui/material';

type Severity = "success" | "info" | "warning" | "error";
interface SnackbarState {
  open?: boolean,
  message?: string,
  icon?: ReactNode | null,
  severity?: Severity,
}

interface SnackbarContextType {
  showSnackbar: (message: string, icon: ReactNode | null, severity?: Severity) => void
}

const SnackbarContext = createContext<SnackbarContextType>(null!);

export function SnackbarProvider({ children }: { children: React.ReactNode }) {
  const [snackbarState, setSnackbarState] = useState<SnackbarState>({});

  const showSnackbar = (message: string, icon: ReactNode, severity?: Severity) =>
    setSnackbarState({ open: true, message, icon, severity });
  const handleClose = () => setSnackbarState((s) => ({ ...s, open: false }));

  return (
    <SnackbarContext.Provider value={{ showSnackbar }}>
      {children}
      <Snackbar
        open={snackbarState.open}
        onClose={handleClose}
        autoHideDuration={3000}
      >
        <Alert severity={snackbarState.severity} icon={snackbarState.icon}>
          {snackbarState.message}
        </Alert>
      </Snackbar>
    </SnackbarContext.Provider>
  );
}

export function useSnackbar() {
  const ctx = useContext(SnackbarContext);
  if (!ctx) throw new Error('useSnackbar must be used within SnackbarProvider');
  return ctx;
}
