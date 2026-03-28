import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Button, TextField } from "@mui/material";
import { useForm, type SubmitHandler } from "react-hook-form";
import z from "zod";
import {
  CostsApi,
  type CostCreateResponse,
  type CostCreateRequest,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new CostsApi(defaultConfiguration);

const ValidationSchema = z.object({
  storeItemId: z.number(),
  amount: z
    .string()
    .regex(validationConstants.numberRegex, "Cena musí být číslo")
    .refine((x) => Number(x) >= 0, "Cena musí být větší/rovna nule"),
  description: z
    .string()
    .min(1, "Popis nesmí být prázdný")
    .max(
      validationConstants.maxDescriptionLength,
      "Popis přesahuje maximální délku",
    ),
});

type Props = {
  id: string;
  storeItemId: number;
  beforeSubmit?: () => void;
  afterSubmit?: (output: CostCreateResponse) => void;
};

const CostCreateForm = ({
  id,
  storeItemId,
  beforeSubmit,
  afterSubmit,
}: Props) => {
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CostCreateRequest>({
    defaultValues: {
      storeItemId: storeItemId,
      amount: "0.00",
      description: "Popis nové ceny",
    },
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<CostCreateRequest> = async (data) => {
    beforeSubmit?.();
    startLoading();
    const output = await handleApiCall(
      api.costsCreate({ costCreateRequest: data }),
    );
    stopLoading();
    if (output) {
      afterSubmit?.(output);
    }
  };

  return (
    <form onSubmit={handleSubmit(submitForm)} id={id}>
      <Box
        display="flex"
        flexDirection="column"
        alignItems="flex-start"
        gap={2}
      >
        <TextField
          label="Nová cena"
          {...register("amount")}
          error={!!errors.amount}
          helperText={errors.amount?.message}
        />
        <TextField
          label="Popis nové ceny"
          {...register("description")}
          error={!!errors.description}
          helperText={errors.description?.message}
        />
        <Button type="submit" variant="contained">
          Nastavit novou cenu
        </Button>
      </Box>
    </form>
  );
};

export default CostCreateForm;
