import z from "zod";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, TextField } from "@mui/material";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import StoreItemPicker from "../pickers/StoreItemPicker";
import type { ContainerTemplateCreateRequest } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  amount: z
    .string()
    .regex(validationConstants.numberRegex, "Objem kegu musí být validní číslo")
    .refine((x) => Number(x) > 0, "Objem kegu musí být větší než 0"),
  storeItemId: z.number(),
});

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const ContainerTemplateCreateForm = ({
  id,
  beforeSubmit,
  afterSubmit,
}: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<ContainerTemplateCreateRequest>({
    defaultValues: {
      name: "Nový typ kegu",
      amount: "0.00",
    },
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<ContainerTemplateCreateRequest> = async (
    requestBody,
  ) => {
    beforeSubmit?.();
    startLoading();
    const { response, error } = await apiClient.POST("/container-templates", {
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
        <TextField
          label="Objem kegu"
          {...register("amount")}
          error={!!errors.amount}
          helperText={errors.amount?.message}
        />
        <Controller
          name="storeItemId"
          control={control}
          render={({ field }) => (
            <StoreItemPicker
              onChange={field.onChange}
              error={!!errors.storeItemId}
              helperText={errors?.storeItemId?.message}
              containerItemsOnly
            />
          )}
        />
      </Box>
    </form>
  );
};

export default ContainerTemplateCreateForm;
