import z from "zod";
import { useAuth } from "../../../auth/AuthContext";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import {
  Box,
  Button,
  Checkbox,
  FormControlLabel,
  TextField,
  Typography,
} from "@mui/material";

const ValidationSchema = z.object({
  nickname: z.string().optional(),
  gamification: z.boolean(),
  newCardId: z.string().optional(),
});

type FormData = z.infer<typeof ValidationSchema>;

const UserProfile = () => {
  const { userClaims } = useAuth();
  const {
    register,
    control,
    formState: { errors, dirtyFields },
    handleSubmit,
  } = useForm<FormData>({
    defaultValues: {
      gamification:
        userClaims
          ?.find((claim) => claim.type === "gam")
          ?.value.toString()
          .toLowerCase() === "true",
      nickname: userClaims?.find((claim) => claim.type === "nick")
        ?.value as string,
    },
  });

  const updateUserData: SubmitHandler<FormData> = async () => {
    if (dirtyFields.gamification) {
      // TODO
      const resp = await fetch(
        "su-dev.fit.vutbr.cz/users/me/gamification_consent",
        { method: "POST" },
      );
    }
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
        <form onSubmit={handleSubmit(updateUserData)}>
          <Box
            display="flex"
            flexDirection="column"
            alignItems="flex-start"
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

            <TextField
              fullWidth
              label="Přezdívka"
              {...register("nickname")}
              error={!!errors.nickname}
              helperText={errors.nickname?.message}
            />

            <Controller
              control={control}
              name="gamification"
              render={({ field }) => (
                <FormControlLabel
                  label="Souhlas s gamifikací"
                  control={<Checkbox {...field} checked={field.value} />}
                />
              )}
            />

            <TextField
              fullWidth
              label="Kód karty ke spárování"
              {...register("newCardId")}
              error={!!errors.newCardId}
              helperText={errors.newCardId?.message}
            />

            <Button type="submit" variant="contained">
              Uložit změny
            </Button>
          </Box>
        </form>
      </Box>
    </>
  );
};

export default UserProfile;
