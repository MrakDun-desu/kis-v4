import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  server: {
    port: 7003,
    proxy: {
      "/bff": {
        target: "https://localhost:7002",
        changeOrigin: true,
        secure: false,
      },
      "/signin-oidc": {
        target: "https://localhost:7002",
        changeOrigin: true,
        secure: false,
      },
      "/signout": {
        target: "https://localhost:7002",
        changeOrigin: true,
        secure: false,
      },
      "/api": {
        target: "https://localhost:7002",
        changeOrigin: true,
        secure: false,
      },
    },
  },
  build: {
    outDir: "../wwwroot",
  },
  plugins: [
    react({
      babel: {
        plugins: [["babel-plugin-react-compiler"]],
      },
    }),
  ],
});
