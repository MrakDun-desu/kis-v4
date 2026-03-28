import { zodResolver } from "@hookform/resolvers/zod";
import { Box, TextField } from "@mui/material";
import { useForm, type SubmitHandler } from "react-hook-form";
import z from "zod";
import { PipesApi, type PipeCreateRequest } from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new PipesApi(defaultConfiguration);

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
});

const defaultValue: PipeCreateRequest = {
  name: "Nová pípa",
};

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const PipeCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<PipeCreateRequest>({
    defaultValues: defaultValue,
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<PipeCreateRequest> = async (data) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(api.pipesCreate({ pipeCreateRequest: data }));
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

export default PipeCreateForm;
