import { createContext, useContext, useEffect, useState, type ReactNode } from 'react';
import Snackbar from '@mui/material/Snackbar'; // or your own component
import { Alert } from '@mui/material';
import { snackbarRef } from '../globalRefs/snackbarRef';
import { CheckCircleOutline, InfoOutline, ReportGmailerrorred, WarningOutlined } from '@mui/icons-material';

type Severity = "success" | "info" | "warning" | "error";
interface SnackbarState {
  open?: boolean,
  message?: string,
  severity: Severity,
}

export interface SnackbarContextType {
  showSnackbar: (message: string, severity?: Severity) => void
}

const SnackbarContext = createContext<SnackbarContextType>(null!);

const severityIconMap: Record<Severity, ReactNode> = {
  error: <ReportGmailerrorred />,
  info: <InfoOutline />,
  success: <CheckCircleOutline />,
  warning: <WarningOutlined />
}

export function SnackbarProvider({ children }: { children: React.ReactNode }) {
  const [snackbarState, setSnackbarState] = useState<SnackbarState>({ severity: "info" });

  const showSnackbar = (message: string, severity: Severity = "info") =>
    setSnackbarState({ open: true, message, severity });
  const handleClose = () => setSnackbarState((s) => ({ ...s, open: false }));

  useEffect(() => {
    snackbarRef.show = showSnackbar;
    return () => { snackbarRef.show = null; }
  }, []);

  return (
    <SnackbarContext.Provider value={{ showSnackbar }}>
      {children}
      <Snackbar
        open={snackbarState.open}
        onClose={handleClose}
        autoHideDuration={3000}
      >
        <Alert severity={snackbarState.severity} icon={severityIconMap[snackbarState.severity]}>
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
