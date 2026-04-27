import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  base: "/kisv4fe",
  server: {
    port: 7003,
    host: "127.0.0.1",
    allowedHosts: ["localhost", "su-dev.fit.vutbr.cz"],
    hmr: {
      "protocol": "wss",
      "host": "su-dev.fit.vutbr.cz",
      "clientPort": 443,
    },
    proxy: {
      "/kisv4fe/bff": {
        target: "http://127.0.0.1:7002",
        changeOrigin: true,
        rewrite: (path) => path.replace("/kisv4fe", ""),
        configure: (proxy) => {
          proxy.on("proxyReq", (proxyReq) => {
            proxyReq.setHeader("X-Forwarded-Host", "su-dev.fit.vutbr.cz");
            proxyReq.setHeader("X-Forwarded-Proto", "https");
            proxyReq.setHeader("X-Forwarded-Prefix", "/kisv4fe");
          })
        }
      },
      "/kisv4fe/api": {
        target: "http://127.0.0.1:7002",
        changeOrigin: true,
        rewrite: (path) => path.replace("/kisv4fe", ""),
      },
      "/kisv4fe/auth": {
        target: "http://127.0.0.1:7002",
        changeOrigin: true,
        rewrite: (path) => path.replace("/kisv4fe", ""),
      },
      "/kisv4fe/signin-oidc": {
        target: "http://127.0.0.1:7002",
        changeOrigin: true,
        rewrite: (path) => path.replace("/kisv4fe", ""),
      },
      "/kisv4fe/signout-callback-oidc": {
        target: "http://127.0.0.1:7002",
        changeOrigin: true,
        rewrite: (path) => path.replace("/kisv4fe", ""),
      },
    },
  },
  build: {
    outDir: "../wwwroot",
    emptyOutDir: true
  },
  plugins: [
    react({
      babel: {
        plugins: [["babel-plugin-react-compiler"]],
      },
    })
  ],
});
