import z from "zod";
import { CashBoxesApi, type CashBoxCreateRequest } from "../api-generated";
import { defaultConfiguration } from "../configuration";
import validationConstants from "../constants/validationConstants";
import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, TextField } from "@mui/material";
import { useLoading } from "../contexts/LoadingContext";
import handleApiCall from "../errorHandling/apiResponseHandler";

const api = new CashBoxesApi(defaultConfiguration);

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
});

const defaultValue: CashBoxCreateRequest = {
  name: "Nová kasa",
};

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const CashBoxCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CashBoxCreateRequest>({
    defaultValues: defaultValue,
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<CashBoxCreateRequest> = async (data) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(api.cashBoxesCreate({ cashBoxCreateRequest: data }));
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
      </Box>
    </form>
  );
};

export default CashBoxCreateForm;
