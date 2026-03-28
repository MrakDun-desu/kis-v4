import z from "zod";
import { ContainersApi, type ContainerCreateRequest } from "../api-generated";
import validationConstants from "../constants/validationConstants";
import { useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, TextField } from "@mui/material";
import { useLoading } from "../contexts/LoadingContext";
import handleApiCall from "../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../configuration/apiConfiguration";

const api = new ContainersApi(defaultConfiguration);

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
  updateCosts: z.boolean().optional(),
});

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
  storeId: number;
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
    formState: { errors },
  } = useForm<ContainerCreateRequest>({
    defaultValues: {
      amount: 1,
      cost: "0.00",
      storeId,
    },
    resolver: zodResolver(ValidationSchema),
  });

  const submitForm: SubmitHandler<ContainerCreateRequest> = async (data) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(api.containersCreate({ containerCreateRequest: data }));
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
      ></Box>
    </form>
  );
};

export default ContainerCreateForm;
