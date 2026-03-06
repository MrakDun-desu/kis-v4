import { Configuration } from "../api-generated";

export const defaultConfiguration = new Configuration({
  basePath: "/api",
  credentials: "include",
  headers: {
    "X-CSRF": "1",
  },
});
