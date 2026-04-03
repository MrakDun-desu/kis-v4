import z from "zod";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Checkbox, FormControlLabel, TextField } from "@mui/material";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import ContainerTemplatePicker from "../pickers/ContainerTemplatePicker";
import StorePicker from "../pickers/StorePicker";
import type {
  ContainerCreateResponse,
  ContainerCreateRequest,
} from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const ValidationSchema = z.object({
  templateId: z.int("Vyberte typ kegu"),
  storeId: z.int("Vyberte sklad"),
  amount: z.int(),
  cost: z
    .string()
    .regex(validationConstants.numberRegex)
    .refine(
      (val) => Number(val) >= 0,
      "Nákupná cena musí být větší/rovna nule",
    ),
  updateCosts: z.boolean(),
});

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: (resp: ContainerCreateResponse) => void;
  storeId?: number;
};

const ContainerCreateForm = ({
  id,
  beforeSubmit,
  afterSubmit,
  storeId,
}: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<ContainerCreateRequest>({
    defaultValues: {
      amount: 1,
      cost: "0.00",
      storeId,
    },
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<ContainerCreateRequest> = async (
    requestBody,
  ) => {
    beforeSubmit?.();
    startLoading();
    const { data, response, error } = await apiClient.POST("/containers", {
      body: requestBody,
    });
    if (!response.ok) {
      handleApiError(response, error);
    }
    stopLoading();
    if (data) {
      afterSubmit?.(data);
    }
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
        <Controller
          control={control}
          name="templateId"
          render={({ field }) => (
            <ContainerTemplatePicker
              onChange={(val) => field.onChange(val?.id)}
              error={!!errors.templateId}
              helperText={errors.templateId?.message}
            />
          )}
        />

        {storeId === undefined && (
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
        )}

        <Controller
          control={control}
          name="amount"
          render={({ field }) => (
            <TextField
              fullWidth
              label="Množství k naskladnění"
              onChange={(val) => field.onChange(Number(val.target.value))}
              error={!!errors.amount}
              helperText={errors.amount?.message}
            />
          )}
        />

        <TextField
          fullWidth
          label="Nákupní cena"
          {...register("cost")}
          error={!!errors.cost}
          helperText={errors.cost?.message}
        />

        <FormControlLabel
          label="Automaticky přepočíst cenu"
          control={<Checkbox {...register("updateCosts")} />}
        />
      </Box>
    </form>
  );
};

export default ContainerCreateForm;
