import { authEvents } from "../auth/authEvents";
import { snackbarRef } from "../globalRefs/snackbarRef";
import type { HttpValidationProblemDetails } from "../api/apiTypes";


const handleApiError = (
  response: Response,
  problem?: HttpValidationProblemDetails,
  onNotFound?: () => void
) => {
  if (problem) {
    if (!problem.errors) {
      snackbarRef.show?.(`Chyba požadavku, detaily v konzoli.`, "error");
      console.log(response);
    }
    let errorMessage = "Naskytli se validační chyby:";
    for (const error in problem.errors) {
      errorMessage = errorMessage.concat(`\n${error}: ${problem.errors[error]}`);
    }
    snackbarRef.show?.(errorMessage, "warning");

    return;
  }

  switch (response.status) {
    case 400: {
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
        window.location.href = "/not-found";
      }
      break;
    }
    default: {
      snackbarRef.show?.(`Chyba ${response.status}.`, "error");
      break;
    }
  }
}

export default handleApiError;
