import { useParams } from "react-router-dom";
import {
  CategoriesApi,
  PrintType,
  SaleItemsApi,
  type CategoryModel,
  type SaleItemReadResponse,
  type SaleItemUpdateModel,
} from "../../../api-generated";
import { defaultConfiguration } from "../../../configuration";
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
import z from "zod";
import validationConstants from "../../../constants/validationConstants";
import { zodResolver } from "@hookform/resolvers/zod";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { printTypes } from "../../../constants/printTypes";
import CompositionCreateForm from "../../../components/CompositionCreateForm";
import { useLoading } from "../../../contexts/LoadingContext";
import CompositionDisplayTable from "../../../components/CompositionDisplayTable";

const api = new SaleItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);

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

export const SaleItemDetail = () => {
  const [saleItem, setSaleItem] = useState<SaleItemReadResponse | null>(null);
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const [compositionRefreshCounter, setCompositionRefreshCounter] = useState(0);
  const { startLoading, stopLoading } = useLoading();
  const { id } = useParams();

  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SaleItemUpdateModel>({
    values:
      saleItem === null
        ? {
            name: "Prodejní položka",
            image: "",
            marginPercent: "0",
            marginStatic: "0.00",
            prestigeAmount: "0",
            printType: "DontPrint",
            categoryIds: [],
            modifierIds: [],
          }
        : {
            name: saleItem.name,
            image: saleItem.image,
            marginPercent: String(saleItem.marginPercent),
            marginStatic: String(saleItem.marginStatic),
            prestigeAmount: String(saleItem.prestigeAmount),
            printType: saleItem.printType,
            categoryIds: saleItem.categories.map((cat) => cat.id),
            modifierIds: saleItem.applicableModifiers.map((mod) => mod.id),
          },
    resolver: zodResolver(ValidationSchema),
  });

  useEffect(() => {
    const getSaleItem = async () => {
      const response = await handleApiCall(
        api.saleItemsRead({
          id: Number(id),
        }),
      );
      setSaleItem(response);
    };
    getSaleItem();
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

  const saveSaleItem: SubmitHandler<SaleItemUpdateModel> = async (data) => {
    startLoading();
    const response = await handleApiCall(
      api.saleItemsUpdate({
        id: Number(id),
        saleItemUpdateModel: data,
      }),
    );
    if (response) {
      setSaleItem(response);
    }
    stopLoading();
  };

  if (!saleItem) {
    return (
      <>
        <Skeleton variant="rounded" width={300} height={30} />
        <Box display="flex" gap={5} marginTop={5}>
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
      <h2>Detail prodejní položky</h2>
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

          <form onSubmit={handleSubmit(saveSaleItem)}>
            <Box
              display="flex"
              flexDirection="column"
              alignItems="flex-start"
              gap={2}
            >
              <TextField
                fullWidth
                label="Název"
                {...register("name")}
                error={!!errors.name}
                helperText={errors?.name?.message}
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
            Zložení
          </Typography>
          <CompositionDisplayTable
            compositeId={Number(id)}
            refreshCounter={compositionRefreshCounter}
          />
          <CompositionCreateForm
            compositeId={Number(id)}
            afterSubmit={() => setCompositionRefreshCounter((val) => val + 1)}
          />
        </Box>
      </Box>
    </>
  );
};
