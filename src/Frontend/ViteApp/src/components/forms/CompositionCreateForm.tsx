import z from "zod";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, TextField } from "@mui/material";
import {
  CompositionsApi,
  type CompositionPutRequest,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import handleApiCall from "../../errorHandling/apiResponseHandler";
import StoreItemPicker from "../pickers/StoreItemPicker";

const api = new CompositionsApi(defaultConfiguration);

const ValidationSchema = z.object({
  compositeId: z.number(),
  storeItemId: z.number("Skladová položka je povinná"),
  amount: z
    .string()
    .regex(
      validationConstants.numberRegex,
      "Množství skladové položky musí být číslo",
    )
    .optional(),
});

type Props = {
  formId?: string;
  compositeId: number;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const CompositionCreateForm = ({
  formId,
  compositeId,
  beforeSubmit,
  afterSubmit,
}: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<CompositionPutRequest>({
    defaultValues: {
      amount: "1",
      compositeId: compositeId,
    },
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<CompositionPutRequest> = async (data) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(api.compositionsPut({ compositionPutRequest: data }));
    stopLoading();
    afterSubmit?.();
  };

  return (
    <form
      onSubmit={handleSubmit(submitForm)}
      id={formId}
      style={{ width: "100%" }}
    >
      <Box
        display="flex"
        flexDirection="column"
        alignItems="flex-start"
        gap={2}
      >
        <Controller
          name="storeItemId"
          control={control}
          render={({ field }) => (
            <StoreItemPicker
              onChange={field.onChange}
              error={!!errors.storeItemId}
              helperText={errors?.storeItemId?.message}
            />
          )}
        />
        <TextField
          fullWidth
          label="Množství"
          {...register("amount")}
          error={!!errors.amount}
          helperText={errors?.amount?.message}
        />

        <Button type="submit" variant="contained">
          Upravit zložení
        </Button>
      </Box>
    </form>
  );
};

export default CompositionCreateForm;
