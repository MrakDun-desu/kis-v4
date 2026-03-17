import z from "zod";
import {
  SaleItemsApi,
  CategoriesApi,
  type SaleItemCreateRequest,
  type CategoryModel,
  type PrintType,
} from "../api-generated";
import { defaultConfiguration } from "../configuration";
import validationConstants from "../constants/validationConstants";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Box,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import { useEffect, useState } from "react";
import { useLoading } from "../contexts/LoadingContext";
import handleApiCall from "../errorHandling/apiResponseHandler";
import { printTypes } from "../constants/printTypes";

const api = new SaleItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);
// const imageApi = new ImagesApi(defaultConfiguration);

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
    )
    .optional(),
  marginStatic: z
    .string()
    .regex(validationConstants.numberRegex, "Statická marže musí být číslo")
    .refine(
      (val) => Number(val) >= 0,
      "Statická marže musí být větší/rovna nule",
    )
    .optional(),
  prestigeAmount: z
    .string()
    .regex(validationConstants.numberRegex, "Prestiž musí být číslo")
    .refine((val) => Number(val) >= 0, "Prestiž musí být větší/rovna nule")
    .optional(),
  printType: z.custom<PrintType>().optional(),
  modifierIds: z.array(z.number()).optional(),
  categoryIds: z.array(z.number()).optional(),
});

const defaultValue: SaleItemCreateRequest = {
  name: "Nová prodejní položka",
  image: "",
  marginPercent: "0.00",
  marginStatic: "0.00",
  prestigeAmount: "0.00",
  printType: "DontPrint",
  modifierIds: [],
  categoryIds: [],
};

type Props = {
  id: string;
  beforeSubmit?: () => void;
  afterSubmit?: () => void;
};

const SaleItemCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
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
      const response = await handleApiCall(categoryApi.categoriesReadAll());
      if (response) {
        setCategories(response.data);
      } else {
        setCategories(null);
      }
    };
    getCategories();
  }, []);

  const submitForm: SubmitHandler<SaleItemCreateRequest> = async (data) => {
    beforeSubmit?.();
    startLoading();
    await handleApiCall(api.saleItemsCreate({ saleItemCreateRequest: data }));
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
        <FormControl fullWidth>
          <InputLabel id="printType">Tisknout?</InputLabel>
          <Select
            label="Tisknout?"
            labelId="printType"
            defaultValue="DontPrint"
            {...register("printType")}
          >
            {Object.keys(printTypes).map((x) => (
              <MenuItem value={x} key={x}>
                {printTypes[x as PrintType]}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
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
