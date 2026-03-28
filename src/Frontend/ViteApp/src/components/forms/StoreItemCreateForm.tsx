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
import {
  StoreItemsApi,
  CategoriesApi,
  type StoreItemCreateRequest,
  type CategoryModel,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import validationConstants from "../../constants/validationConstants";
import { useLoading } from "../../contexts/LoadingContext";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new StoreItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  unitName: z
    .string()
    .min(1, "Jednotka nesmí být prázdná")
    .max(
      validationConstants.maxUnitNameLength,
      "Jednotka přesahuje maximální délku",
    ),
  initialCost: z
    .string()
    .regex(validationConstants.numberRegex, "Počáteční cena musí být číslo")
    .refine((val) => Number(val) >= 0, "Cena musí být větší/rovna nule"),
  isContainerItem: z.boolean().optional(),
  categoryIds: z.array(z.number()).optional(),
});

const defaultValue: StoreItemCreateRequest = {
  initialCost: "0.00",
  name: "Nová skladová položka",
  unitName: "ks",
  categoryIds: [],
  isContainerItem: false,
};

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const StoreItemCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const { startLoading, stopLoading } = useLoading();
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<StoreItemCreateRequest>({
    defaultValues: defaultValue,
    resolver: zodResolver(ValidationSchema),
  });
  useEffect(() => {
    const getCategories = async () => {
      const response = await handleApiCall(categoryApi.categoriesReadAll());
      if (response) {
        setCategories(response.data);
      } else {
        setCategories(null);
      }
    };
    getCategories();
  }, []);

  const submitForm: SubmitHandler<StoreItemCreateRequest> = async (data) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(api.storeItemsCreate({ storeItemCreateRequest: data }));
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
          label="Název jednotky"
          {...register("unitName")}
          error={!!errors.unitName}
          helperText={errors.unitName?.message}
        />
        <FormControl>
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
        <FormControlLabel
          label="Kegová položka"
          control={<Checkbox {...register("isContainerItem")} />}
        />
        <TextField
          label="Počáteční cena"
          {...register("initialCost")}
          error={!!errors.initialCost}
          helperText={errors.initialCost?.message}
        />
      </Box>
    </form>
  );
};

export default StoreItemCreateForm;
