import z from "zod";
import {
  LayoutItemType,
  LayoutsApi,
  type LayoutCreateRequestModel,
} from "../api-generated";
import { defaultConfiguration } from "../configuration";
import validationConstants from "../constants/validationConstants";
import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Checkbox, FormControlLabel, TextField } from "@mui/material";
import { useLoading } from "../contexts/LoadingContext";
import handleApiCall from "../errorHandling/apiResponseHandler";

const api = new LayoutsApi(defaultConfiguration);

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  image: z.string().nullish(),
  topLevel: z.boolean().optional(),
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

  const submitForm: SubmitHandler<LayoutCreateRequestModel> = async (data) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(api.layoutsCreate({ layoutCreateRequestModel: data }));
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
          label="Výchozí layout"
          control={<Checkbox {...register("topLevel")} />}
        />
      </Box>
    </form>
  );
};

export default LayoutCreateForm;
