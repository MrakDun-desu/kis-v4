import {
  createTheme,
  CssBaseline,
  ThemeProvider,
  useMediaQuery,
  type Theme,
} from "@mui/material";
import { useMemo } from "react";
import { BrowserRouter, Outlet, Route, Routes } from "react-router-dom";
import { RequireAuth } from "./auth/RequireAuth";
import { AuthProvider } from "./auth/AuthContext";
import { LoadingProvider } from "./contexts/LoadingContext";
import { SnackbarProvider } from "./contexts/SnackbarContext";
import StoreItemDetail from "./pages/admin/pages/StoreItemDetail";
import Orders from "./pages/pos/pages/Orders";
import SaleItems from "./pages/admin/pages/SaleItems";
import SaleItemDetail from "./pages/admin/pages/SaleItemDetail";
import Stores from "./pages/admin/pages/Stores";
import CashBoxes from "./pages/admin/pages/CashBoxes";
import StoreItems from "./pages/admin/pages/StoreItems";
import StoreDetail from "./pages/admin/pages/StoreDetail";
import Layouts from "./pages/admin/pages/Layouts";
import LayoutDetail from "./pages/admin/pages/LayoutDetail";
import { HomePage } from "./pages/HomePage";
import { AdminPage } from "./pages/admin/AdminPage";
import { PosMain } from "./pages/pos/PosMain";
import { NotFoundPage } from "./pages/NotFoundPage";
import PosSettings from "./pages/pos/pages/PosSettings";
import Categories from "./pages/admin/pages/Categories";
import Pipes from "./pages/admin/pages/Pipes";
import ContainerTemplates from "./pages/admin/pages/ContainerTemplates";

function App() {
  const prefersLightMode = useMediaQuery("(prefers-color-scheme: light)");

  const theme = useMemo<Theme>(
    () =>
      createTheme({
        palette: {
          mode: prefersLightMode ? "light" : "dark",
        },
      }),
    [prefersLightMode],
  );

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <BrowserRouter>
        <Routes>
          <Route
            element={
              <AuthProvider>
                <LoadingProvider>
                  <SnackbarProvider>
                    <Outlet />
                  </SnackbarProvider>
                </LoadingProvider>
              </AuthProvider>
            }
          >
            <Route
              index
              element={
                <RequireAuth>
                  <HomePage />
                </RequireAuth>
              }
            />

            <Route
              path="admin"
              element={
                <RequireAuth>
                  <AdminPage />
                </RequireAuth>
              }
            >
              <Route path="store-items" element={<StoreItems />} />
              <Route path="store-items/:id" element={<StoreItemDetail />} />
              <Route path="stores" element={<Stores />} />
              <Route path="stores/:id" element={<StoreDetail />} />
              <Route path="store-transactions" element={null} />
              <Route path="sale-items" element={<SaleItems />} />
              <Route path="sale-items/:id" element={<SaleItemDetail />} />
              <Route path="modifiers" element={null} />
              <Route path="sale-transactions" element={null} />
              <Route path="cashboxes" element={<CashBoxes />} />
              <Route path="categories" element={<Categories />} />
              <Route
                path="container-templates"
                element={<ContainerTemplates />}
              />
              <Route path="containers" element={null} />
              <Route path="taps" element={<Pipes />} />
              <Route path="users" element={null} />
              <Route path="layouts" element={<Layouts />} />
              <Route path="layouts/:id" element={<LayoutDetail />} />
              <Route path="discounts" element={null} />
            </Route>

            <Route
              path="pos"
              element={
                <RequireAuth>
                  <PosMain />
                </RequireAuth>
              }
            >
              <Route path="orders" element={<Orders />} />
              <Route path="recent-transactions" element={null} />
              <Route path="containers" element={null} />
              <Route path="card-pairing" element={null} />
              <Route path="settings" element={<PosSettings />} />
              <Route path="pipes/:id" element={null} />
              <Route path="containers/:id" element={null} />
            </Route>

            <Route path="*" element={<NotFoundPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;
