import { createContext, useContext, useState } from 'react';
import { Backdrop, CircularProgress } from '@mui/material';

interface LoadingContextType {
  startLoading: () => void
  stopLoading: () => void
}

const LoadingContext = createContext<LoadingContextType>(null!);

export function LoadingProvider({ children }: { children: React.ReactNode }) {
  const [loading, setLoading] = useState(false);

  const startLoading = () => setLoading(true);
  const stopLoading = () => setLoading(false);

  return (
    <LoadingContext.Provider value={{ startLoading, stopLoading }}>
      {children}
      <Backdrop open={loading}>
        <CircularProgress />
      </Backdrop>
    </LoadingContext.Provider>
  );
}

export function useLoading() {
  const ctx = useContext(LoadingContext);
  if (!ctx) throw new Error('useLoading must be used within LoadingProvider');
  return ctx;
}
