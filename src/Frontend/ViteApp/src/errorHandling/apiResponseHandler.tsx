import { authEvents } from "../auth/authEvents";
import { snackbarRef } from "../globalRefs/snackbarRef";
import type { HttpValidationProblemDetails } from "../api/apiTypes";
import { Box } from "@mui/material";
import { redirect } from "react-router-dom";

const handleApiError = (
  response: Response,
  problem?: HttpValidationProblemDetails,
  onNotFound?: () => void,
) => {
  switch (response.status) {
    case 400: {
      if (problem) {
        if (!problem.errors) {
          snackbarRef.show?.(`Chyba požadavku, detaily v konzoli.`, "error");
          console.log(response);
        }
        if (problem.errors !== undefined) {
          snackbarRef.show?.(
            Object.keys(problem.errors).map((error) => (
              <Box>{problem.errors?.[error]}</Box>
            )),
            "warning",
            "Naskytli se validační chyby",
          );
        }

        return;
      }

      snackbarRef.show?.(`Chyba požadavku, detaily v konzoli.`, "error");
      console.log(response);
      break;
    }
    case 401: {
      authEvents.emit("unauthorized");
      break;
    }
    case 403: {
      snackbarRef.show?.("Na tuhle akci nemáte práva", "error");
      break;
    }
    case 404: {
      if (onNotFound) {
        onNotFound();
      } else {
        // TODO make this work better
        redirect("/not-found");
      }
      break;
    }
    default: {
      snackbarRef.show?.(`Chyba ${response.status}.`, "error");
      break;
    }
  }
};

export default handleApiError;
