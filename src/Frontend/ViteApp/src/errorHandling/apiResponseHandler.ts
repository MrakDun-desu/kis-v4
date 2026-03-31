import { instanceOfHttpValidationProblemDetails, ResponseError } from "../api-generated";
import { authEvents } from "../auth/authEvents";
import { snackbarRef } from "../globalRefs/snackbarRef";

const handleApiCall = async <TRes>(call: Promise<TRes>, onNotFound: (() => void) | null = null): Promise<TRes | null> => {
  try {
    const res = await call;
    return res;
  } catch (err) {
    if (err instanceof ResponseError) {
      const resp = err.response;
      if (resp.status === 404) {
        if (onNotFound !== null) {
          onNotFound();
        } else {
          // TODO make this more resilient
          window.location.href = "/not-found";
        }
        return null;
      }

      if (resp.status === 401) {
        authEvents.emit("unauthorized");
      }

      try {
        const respBody = await resp.json();
        if (instanceOfHttpValidationProblemDetails(respBody)) {
          if (!respBody.errors) {
            snackbarRef.show?.(`Chyba ${resp.status}.`, "error");
            console.log(resp);
            return null;
          }
          let errorMessage = "Naskytli se validační chyby:";
          for (const error in respBody.errors) {
            errorMessage = errorMessage.concat(`\n${error}: ${respBody.errors[error]}`);
          }
          snackbarRef.show?.(errorMessage, "warning");
        }
      } catch {
        snackbarRef.show?.("Uh oh, něco se seriózně pokazilo. Detaily v konzoli", "error");
        console.log(err);
      }
    } else {
      snackbarRef.show?.("Uh oh, něco se seriózně pokazilo. Detaily v konzoli", "error");
      console.log(err);
    }
  }

  return null;
}

export default handleApiCall;
