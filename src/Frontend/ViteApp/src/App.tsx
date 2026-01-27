import {
  createTheme,
  CssBaseline,
  ThemeProvider,
  useMediaQuery,
  type Theme,
} from "@mui/material";
import { useMemo } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import {
  AdminPage,
  HomePage,
  NotFoundPage,
  PosMain,
  StoreItems,
} from "./pages";

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

          <Route index element={<HomePage />} />

          <Route path="admin" element={<AdminPage />}>
            <Route path="store-items" element={<StoreItems />} />
            <Route path="stores" element={<StoreItems />} />
            <Route path="store-transactions" element={<StoreItems />} />
            <Route path="sale-items" element={<StoreItems />} />
            <Route path="modifiers" element={<StoreItems />} />
            <Route path="sale-transactions" element={<StoreItems />} />
            <Route path="cashboxes" element={<StoreItems />} />
            <Route path="container-templates" element={<StoreItems />} />
            <Route path="containers" element={<StoreItems />} />
            <Route path="taps" element={<StoreItems />} />
            <Route path="users" element={<StoreItems />} />
            <Route path="layouts" element={<StoreItems />} />
            <Route path="discounts" element={<StoreItems />} />
          </Route>

          <Route path="pos" element={<PosMain />} />

          <Route path="*" element={<NotFoundPage />} />

        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;
