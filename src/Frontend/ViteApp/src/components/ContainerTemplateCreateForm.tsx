import z from "zod";
import {
  ContainerTemplatesApi,
  type ContainerTemplateCreateRequest,
} from "../api-generated";
import validationConstants from "../constants/validationConstants";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, TextField } from "@mui/material";
import { useLoading } from "../contexts/LoadingContext";
import handleApiCall from "../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../configuration/apiConfiguration";
import StoreItemPicker from "./StoreItemPicker";

const api = new ContainerTemplatesApi(defaultConfiguration);

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
    data,
  ) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(
      api.containerTemplatesCreate({ containerTemplateCreateRequest: data }),
    );
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
