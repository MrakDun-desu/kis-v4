import z from "zod";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Box,
  Checkbox,
  FormControl,
  FormControlLabel,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import { useEffect, useState } from "react";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import type { SaleItemCreateRequest, CategoryModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  image: z.string().nullish(),
  marginPercent: z
    .string()
    .regex(validationConstants.numberRegex, "Procentuální marže musí být číslo")
    .refine(
      (val) => Number(val) >= 0,
      "Procentuální marže musí být větší/rovna nule",
    ),
  marginStatic: z
    .string()
    .regex(validationConstants.numberRegex, "Statická marže musí být číslo")
    .refine(
      (val) => Number(val) >= 0,
      "Statická marže musí být větší/rovna nule",
    ),
  prestigeAmount: z
    .string()
    .regex(validationConstants.numberRegex, "Prestiž musí být číslo")
    .refine((val) => Number(val) >= 0, "Prestiž musí být větší/rovna nule"),
  modifierIds: z.array(z.number()).optional(),
  categoryIds: z.array(z.number()).optional(),
  triggerTablePicker: z.boolean(),
  sendToFood: z.boolean(),
});

const defaultValue: SaleItemCreateRequest = {
  name: "Nová prodejní položka",
  image: "",
  marginPercent: "0.00",
  marginStatic: "0.00",
  prestigeAmount: "0.00",
  triggerTablePicker: false,
  sendToFood: false,
  modifierIds: [],
  categoryIds: [],
};

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const SaleItemCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const [categories, setCategories] = useState<CategoryModel[]>();
  // TODO add modifiers here
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<SaleItemCreateRequest>({
    defaultValues: defaultValue,
    resolver: zodResolver(ValidationSchema),
  });
  useEffect(() => {
    const getCategories = async () => {
      const { response, data } = await apiClient.GET("/categories");
      setCategories(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getCategories();
  }, []);

  const submitForm: SubmitHandler<SaleItemCreateRequest> = async (
    requestBody,
  ) => {
    beforeSubmit?.();
    startLoading();
    const { response, error } = await apiClient.POST("/sale-items", {
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
          fullWidth
          label="Název"
          {...register("name")}
          error={!!errors.name}
          helperText={errors.name?.message}
        />

        <TextField
          fullWidth
          label="Procentuální marže"
          {...register("marginPercent")}
          error={!!errors.marginPercent}
          helperText={errors.marginPercent?.message}
        />

        <TextField
          fullWidth
          label="Statická marže"
          {...register("marginStatic")}
          error={!!errors.marginStatic}
          helperText={errors.marginStatic?.message}
        />

        <TextField
          fullWidth
          label="Prestiž"
          {...register("prestigeAmount")}
          error={!!errors.prestigeAmount}
          helperText={errors.prestigeAmount?.message}
        />

        <FormControlLabel
          label="Zobrazit při prodeji výběr stolu"
          control={<Checkbox {...register("triggerTablePicker")} />}
        />

        <FormControlLabel
          label="Dlouhá příprava"
          control={<Checkbox {...register("sendToFood")} />}
        />

        {/* TODO add image and modifier pickers */}
        <FormControl fullWidth>
          <InputLabel id="categorySelect">Kategorie</InputLabel>
          <Controller
            name="categoryIds"
            control={control}
            render={({ field }) => (
              <Select
                sx={{
                  minWidth: "10em",
                }}
                labelId="categorySelect"
                multiple
                {...field}
                label="Kategorie"
              >
                {categories?.map((cat) => (
                  <MenuItem key={cat.id} value={cat.id}>
                    {cat.name}
                  </MenuItem>
                )) ?? null}
              </Select>
            )}
          />
        </FormControl>
      </Box>
    </form>
  );
};

export default SaleItemCreateForm;
