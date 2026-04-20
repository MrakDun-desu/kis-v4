import { useAuth } from "../../../auth/AuthContext";
import { Box, Button, TextField, Typography } from "@mui/material";
import { useState } from "react";
import { CheckBox, CheckBoxOutlineBlank } from "@mui/icons-material";
import { useSnackbar } from "../../../contexts/SnackbarContext";
import { useLoading } from "../../../contexts/LoadingContext";
import z from "zod";
import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

const PinSchema = z.object({
  newPin: z.string().regex(/^\d{6,10}$/, "PIN musí být 6-10 čísel"),
});

type PinFormData = z.infer<typeof PinSchema>;

const UserProfile = () => {
  const { userClaims } = useAuth();
  const [newCardToken, setNewCardToken] = useState("");
  const { showSnackbar } = useSnackbar();
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    formState: { errors },
    handleSubmit,
  } = useForm<PinFormData>({ resolver: zodResolver(PinSchema) });

  const updateUserData = async (evt: React.SubmitEvent<HTMLFormElement>) => {
    evt.preventDefault();
    startLoading();
    try {
      const resp = await fetch(
        import.meta.env.BASE_URL + "/auth/users/me/rfid",
        {
          method: "PUT",
          credentials: "include",
          headers: {
            "X-CSRF": "1",
            "Content-Type": "application/json",
          },
          body: JSON.stringify(newCardToken),
        },
      );
      if (resp.ok) {
        showSnackbar("Karta úspěšně spárována!", "success");
        setNewCardToken("");
      } else {
        showSnackbar("Neplatný kód karty", "warning");
      }
    } catch (err) {
      console.error(err);
    }
    stopLoading();
  };

  const setCardPin: SubmitHandler<PinFormData> = async (data) => {
    startLoading();
    try {
      const resp = await fetch(
        import.meta.env.BASE_URL + "/auth/users/me/rfid/pin",
        {
          method: "PUT",
          credentials: "include",
          headers: {
            "X-CSRF": "1",
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data.newPin),
        },
      );

      if (resp.ok) {
        showSnackbar("Pin úspěšně nastaven!", "success");
        setNewCardToken("");
      } else {
        switch (resp.status) {
          case 400:
            showSnackbar("Špatný PIN", "warning");
            break;
          case 404:
            showSnackbar("Karta nenalezena", "warning");
            break;
          case 409:
            showSnackbar(
              "Konflikt: uživatel si nemůže nastavit PIN",
              "warning",
            );
            break;
        }
      }
    } catch (err) {
      console.error(err);
    }
    stopLoading();
  };

  return (
    <>
      <h2>Můj profil</h2>

      <Box
        display="flex"
        gap={2}
        paddingBottom={2}
        flexDirection="column"
        alignItems="flex-start"
      >
        <Box
          display="flex"
          flexDirection="column"
          alignItems="stretch"
          gap={2}
          minWidth={300}
        >
          <Typography>
            <b>Jméno:</b>{" "}
            {userClaims?.find((claim) => claim.type === "name")?.value}
          </Typography>

          <Typography>
            <b>E-mail:</b>{" "}
            {userClaims?.find((claim) => claim.type === "email")?.value}
          </Typography>

          <Typography>
            <b>Přezdívka:</b>{" "}
            {userClaims?.find((claim) => claim.type === "nick")?.value}
          </Typography>

          <Box display="flex" gap={1}>
            <b>Souhlas s gamifikací:</b>
            {userClaims
              ?.find((claim) => claim.type === "gam")
              ?.value.toString()
              .toLowerCase() === "true" ? (
              <CheckBox />
            ) : (
              <CheckBoxOutlineBlank />
            )}
          </Box>

          <form onSubmit={updateUserData}>
            <Box
              display="flex"
              flexDirection="column"
              alignItems="flex-start"
              gap={2}
            >
              <TextField
                fullWidth
                label="Kód karty ke spárování"
                value={newCardToken}
                onChange={(evt) => setNewCardToken(evt.target.value)}
              />

              <Button type="submit" variant="contained">
                Přidat novou kartu
              </Button>
            </Box>
          </form>

          <form onSubmit={handleSubmit(setCardPin)}>
            <Box
              display="flex"
              flexDirection="column"
              alignItems="flex-start"
              gap={2}
            >
              <TextField
                fullWidth
                label="PIN pro přihlášení přes kartu"
                {...register("newPin")}
                error={!!errors.newPin}
                helperText={errors.newPin?.message}
              />

              <Button type="submit" variant="contained">
                Nastavit PIN
              </Button>
            </Box>
          </form>
        </Box>
      </Box>
    </>
  );
};

export default UserProfile;
