import createClient from "openapi-fetch";
import type { paths } from "./apiSchema";

export const apiClient = createClient<paths>({
  baseUrl: "/api",
  credentials: "include",
  headers: {
    "X-CSRF": "1",
  },
});
