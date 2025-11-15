import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  server: {
    proxy: {
      "/bff": "https::/localhost:5001",
      "/signin-oidc": "https::/localhost:5001",
      "/signout": "https::/localhost:5001",
      "/api": "https::/localhost:5001",
    }
  },
  build: {
    outDir: "../wwwroot"
  },
  plugins: [
    react({
      babel: {
        plugins: [['babel-plugin-react-compiler']],
      },
    }),
  ],
})
