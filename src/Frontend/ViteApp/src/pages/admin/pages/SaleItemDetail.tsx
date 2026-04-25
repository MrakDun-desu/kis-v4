import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import {
  Box,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  InputAdornment,
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
import { useLoading } from "../../../contexts/LoadingContext";
import CompositionListView from "../../../components/views/CompositionListView";
import CompositionCreateForm from "../../../components/forms/CompositionCreateForm";
import type {
  CategoryModel,
  SaleItemReadResponse,
  SaleItemUpdateModel,
} from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

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

const SaleItemDetail = () => {
  const [saleItem, setSaleItem] = useState<SaleItemReadResponse>();
  const [categories, setCategories] = useState<CategoryModel[]>();
  const [compositionRefreshCounter, setCompositionRefreshCounter] = useState(0);
  const [saleItemRefreshCounter, setSaleItemRefreshCounter] = useState(0);
  const { startLoading, stopLoading } = useLoading();
  const { id } = useParams();

  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
    watch,
  } = useForm<SaleItemUpdateModel>({
    values: !saleItem
      ? {
          name: "Prodejní položka",
          image: "",
          marginPercent: "0",
          marginStatic: "0.00",
          prestigeAmount: "0",
          categoryIds: [],
          modifierIds: [],
          triggerTablePicker: false,
          sendToFood: false,
        }
      : {
          name: saleItem.name,
          image: saleItem.image,
          marginPercent: String(saleItem.marginPercent),
          marginStatic: String(saleItem.marginStatic),
          prestigeAmount: String(saleItem.prestigeAmount),
          triggerTablePicker: saleItem.triggerTablePicker,
          sendToFood: saleItem.sendToFood,
          categoryIds: saleItem.categories.map((cat) => cat.id),
          modifierIds: saleItem.applicableModifiers.map((mod) => mod.id),
        },
    resolver: zodResolver(ValidationSchema),
  });

  const sendToFood = watch("sendToFood");
  const triggerTablePicker = watch("triggerTablePicker");

  useEffect(() => {
    const getSaleItem = async () => {
      const { data, response } = await apiClient.GET("/sale-items/{id}", {
        params: { path: { id: Number(id) } },
      });
      setSaleItem(data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getSaleItem();
  }, [saleItemRefreshCounter]);

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

  const saveSaleItem: SubmitHandler<SaleItemUpdateModel> = async (
    requestBody,
  ) => {
    startLoading();
    const { response, data, error } = await apiClient.PUT("/sale-items/{id}", {
      params: { path: { id: Number(id) } },
      body: requestBody,
    });
    if (data) {
      setSaleItem(data);
    } else {
      handleApiError(response, error);
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

              <Typography>Aktuální cena: {saleItem.currentCost} kč</Typography>

              <TextField
                fullWidth
                label="Procentuální marže"
                {...register("marginPercent")}
                error={!!errors.marginPercent}
                helperText={errors.marginPercent?.message}
                slotProps={{
                  input: {
                    endAdornment: (
                      <InputAdornment position="end">%</InputAdornment>
                    ),
                  },
                }}
              />

              <TextField
                fullWidth
                label="Statická marže"
                {...register("marginStatic")}
                error={!!errors.marginStatic}
                helperText={errors.marginStatic?.message}
                slotProps={{
                  input: {
                    endAdornment: (
                      <InputAdornment position="end">kč</InputAdornment>
                    ),
                  },
                }}
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
                control={
                  <Checkbox
                    {...register("triggerTablePicker")}
                    checked={triggerTablePicker}
                  />
                }
              />

              <FormControlLabel
                label="Dlouhá příprava"
                control={
                  <Checkbox {...register("sendToFood")} checked={sendToFood} />
                }
              />

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
          <CompositionListView
            compositeId={Number(id)}
            refreshCounter={compositionRefreshCounter}
          />
          <CompositionCreateForm
            compositeId={Number(id)}
            afterSubmit={() => {
              setCompositionRefreshCounter((val) => val + 1);
              setSaleItemRefreshCounter((val) => val + 1);
            }}
          />
        </Box>
      </Box>
    </>
  );
};

export default SaleItemDetail;
