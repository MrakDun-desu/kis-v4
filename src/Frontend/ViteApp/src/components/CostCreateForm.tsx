import z from "zod";
import { StoreItemsApi, CategoriesApi, type StoreItemCreateRequest, type CategoryModel, type CostCreateRequest } from "../api-generated";
import { defaultConfiguration } from "../configuration";
import validationConstants from "../constants/validationConstants";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Checkbox, FormControl, FormControlLabel, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { useEffect, useState } from "react";

const api = new StoreItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);

const ValidationSchema = z.object({
  storeItemId: z.number(),
  amount: z.string()
    .regex(validationConstants.numberRegex)
    .refine(x => Number(x) >= 0),
  description: z.string()
    .min
})

const defaultValue: CostCreateRequest = {
  storeItemId: 0,
  amount: "0",
  description: "Popis nové ceny"
}

type Props = {
  id: string,
  beforeSubmit?: () => void,
  afterSubmit?: () => void,
}

const CostCreateForm = ({ id, beforeSubmit, afterSubmit }: Props) => {
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const {
    register,
    handleSubmit,
    control,
    formState: { errors }
  } = useForm<CostCreateRequest>({
    values: defaultValue,
    resolver: zodResolver(ValidationSchema)
  });
  useEffect(() => {
    const getCategories = async () => {
      const response = await categoryApi.categoriesReadAll();
      setCategories(response.data);
    };
    getCategories();
  }, []);

  const createStoreItem: SubmitHandler<StoreItemCreateRequest> = async (
    data,
  ) => {
    beforeSubmit?.();
    await api.storeItemsCreate({
      storeItemCreateRequest: data,
    });
    afterSubmit?.();
  };


  return <form
    onSubmit={handleSubmit(createStoreItem)}
    id={id}
  >
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
}

export default StoreItemCreateForm;
