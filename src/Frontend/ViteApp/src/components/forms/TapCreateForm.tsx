import { zodResolver } from "@hookform/resolvers/zod";
import { Box, TextField } from "@mui/material";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import z from "zod";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import type { TapCreateRequest } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";
import StorePicker from "../pickers/StorePicker";

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  storeId: z.number("Vyberte sklad"),
});

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const TapCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<TapCreateRequest>({
    defaultValues: {
      name: "Nová pípa",
    },
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<TapCreateRequest> = async (requestBody) => {
    beforeSubmit?.();
    startLoading();
    const { response, error } = await apiClient.POST("/taps", {
      body: requestBody,
    });
    if (!response.ok) {
      handleApiError(response, error);
    }
    stopLoading();
    afterSubmit?.();
  };

  return (
    <form onSubmit={handleSubmit(submitForm)} id={id}>
      <Box
        display="flex"
        flexDirection="column"
        alignItems="flex-start"
        gap={2}
        marginTop={1}
      >
        <TextField
          label="Název"
          {...register("name")}
          error={!!errors.name}
          helperText={errors.name?.message}
        />

        <Controller
          control={control}
          name="storeId"
          render={({ field }) => (
            <StorePicker
              onChange={(val) => field.onChange(val?.id)}
              error={!!errors.storeId}
              helperText={errors.storeId?.message}
            />
          )}
        />
      </Box>
    </form>
  );
};

export default TapCreateForm;
