import { zodResolver } from "@hookform/resolvers/zod";
import { Box, TextField, FormControlLabel, Checkbox } from "@mui/material";
import { useForm, type SubmitHandler } from "react-hook-form";
import z from "zod";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import type {
  LayoutItemType,
  LayoutCreateRequestModel,
} from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  image: z.string().nullish(),
  topLevel: z.boolean(),
  layoutItems: z.array(
    z.object({
      x: z
        .number()
        .refine((x) => x >= 1 && x <= 4, "Pozice položky je mimo rozsah"),
      y: z
        .number()
        .refine((x) => x >= 1 && x <= 4, "Pozice položky je mimo rozsah"),
      targetId: z.number(),
      type: z.custom<LayoutItemType>(),
    }),
  ),
});

const defaultValue: LayoutCreateRequestModel = {
  name: "Nové rozložení",
  layoutItems: [],
  topLevel: false,
};

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const LayoutCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LayoutCreateRequestModel>({
    defaultValues: defaultValue,
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<LayoutCreateRequestModel> = async (
    requestBody,
  ) => {
    beforeSubmit?.();
    startLoading();
    const { response, error } = await apiClient.POST("/layouts", {
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
        <FormControlLabel
          label="Výchozí rozložení"
          control={<Checkbox {...register("topLevel")} />}
        />
      </Box>
    </form>
  );
};

export default LayoutCreateForm;
