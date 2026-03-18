import { useParams } from "react-router-dom";
import {
  CategoriesApi,
  StoreItemsApi,
  type CategoryModel,
  type StoreItemReadResponse,
  type StoreItemUpdateModel,
} from "../../../api-generated";
import { useEffect, useState } from "react";
import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Skeleton,
  TextField,
  Typography,
} from "@mui/material";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { CheckBox, CheckBoxOutlineBlank } from "@mui/icons-material";
import CostCreateForm from "../../../components/CostCreateForm";
import z from "zod";
import validationConstants from "../../../constants/validationConstants";
import { zodResolver } from "@hookform/resolvers/zod";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";

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
  categoryIds: z.array(z.number()).optional(),
});

const StoreItemDetail = () => {
  const { id } = useParams();
  const [storeItem, setStoreItem] = useState<StoreItemReadResponse | null>(
    null,
  );
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<StoreItemUpdateModel>({
    values:
      storeItem === null
        ? {
            name: "",
            unitName: "",
            categoryIds: [],
          }
        : {
            name: storeItem.name,
            unitName: storeItem.unitName,
            categoryIds: storeItem.categories.map((cat) => cat.id),
          },
    resolver: zodResolver(ValidationSchema),
  });

  useEffect(() => {
    const getStoreItem = async () => {
      const response = await handleApiCall(
        api.storeItemsRead({
          id: Number(id),
        }),
      );
      setStoreItem(response);
    };
    getStoreItem();
  }, []);
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

  const saveStoreItem: SubmitHandler<StoreItemUpdateModel> = async (data) => {
    if (!storeItem) {
      return;
    }
    setStoreItem(null);
    const response = await handleApiCall(
      api.storeItemsUpdate({
        id: id as unknown as number,
        storeItemUpdateModel: data,
      }),
    );
    setStoreItem(response);
  };

  if (!storeItem) {
    return (
      <>
        <Skeleton variant="rounded" width={300} height={30} />
        <Box display="flex" gap={5}>
          <Box display="flex" flexDirection="column" gap={2}>
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
          </Box>
          <Box display="flex" flexDirection="column" gap={2}>
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
          </Box>
        </Box>
      </>
    );
  }

  return (
    <>
      <h2>Detail skladové položky</h2>
      <Box display="flex" gap={5}>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
        >
          <Typography
            variant="h6"
            component="span"
            display="inline"
            sx={{ marginBottom: 1 }}
          >
            Úprava položky
          </Typography>
          <form onSubmit={handleSubmit(saveStoreItem)}>
            <Box
              display="flex"
              flexDirection="column"
              alignItems="flex-start"
              gap={2}
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
              <Box display="flex" alignItems="center" gap={1}>
                {storeItem.isContainerItem ? (
                  <CheckBox />
                ) : (
                  <CheckBoxOutlineBlank />
                )}
                Kegová položka
              </Box>
              <div>Aktuální cena: {storeItem.currentCost}czk</div>
              <Button type="submit" variant="contained">
                Uložit změny
              </Button>
            </Box>
          </form>
        </Box>

        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
        >
          <Typography
            variant="h6"
            component="span"
            display="inline"
            sx={{ marginBottom: 1 }}
          >
            Nastavení ceny
          </Typography>
          <CostCreateForm
            id="costCreateForm"
            storeItemId={Number(id)}
            afterSubmit={(newCost) => {
              setStoreItem((prev) =>
                !prev ? prev : { ...prev, currentCost: newCost.amount },
              );
            }}
          />
        </Box>
      </Box>
    </>
  );
};

export default StoreItemDetail;
