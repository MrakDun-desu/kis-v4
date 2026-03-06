import { useParams } from "react-router-dom";
import {
  CategoriesApi,
  CostsApi,
  StoreItemsApi,
  type CategoryModel,
  type CostCreateRequest,
  type StoreItemReadResponse,
  type StoreItemUpdateModel,
} from "../../../api-generated";
import { defaultConfiguration } from "../../../configuration";
import { useEffect, useState } from "react";
import {
  Backdrop,
  Box,
  Button,
  CircularProgress,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
  Typography,
} from "@mui/material";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import { CheckBox, CheckBoxOutlineBlank } from "@mui/icons-material";

const api = new StoreItemsApi(defaultConfiguration);
const categoryApi = new CategoriesApi(defaultConfiguration);
const costApi = new CostsApi(defaultConfiguration);

export const StoreItemDetail = () => {
  const { id } = useParams();
  const [storeItem, setStoreItem] = useState<StoreItemReadResponse | null>(
    null,
  );
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  const [currentCost, setCurrentCost] = useState<CostCreateRequest>();
  const {
    register: registerCost,
    handleSubmit: handleCostSubmit,
    formState: { errors: costErrors },
  } = useForm<CostCreateRequest>();
  const {
    control: controlStoreItem,
    register: registerStoreItem,
    handleSubmit: handleStoreItemSubmit,
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
  });

  useEffect(() => {
    const getStoreItem = async () => {
      const response = await api.storeItemsRead({
        id: id as unknown as number,
      });
      setStoreItem(response);
    };
    getStoreItem();
  }, []);
  useEffect(() => {
    const getCategories = async () => {
      const response = await categoryApi.categoriesReadAll();
      setCategories(response.data);
    };
    getCategories();
  }, []);

  const saveStoreItem: SubmitHandler<StoreItemUpdateModel> = async (data) => {
    if (!storeItem) {
      return;
    }
    setStoreItem(null);
    const response = await api.storeItemsUpdate({
      id: id as unknown as number,
      storeItemUpdateModel: data,
    });
    setStoreItem(response);
  };

  const updateCost: SubmitHandler<CostCreateRequest> = async (data) => {
    data.storeItemId = id as unknown as number;
    const response = await costApi.costsCreate({
      costCreateRequest: data,
    });
    setStoreItem((prev) => {
      if (!prev) {
        return null;
      }
      return { ...prev, currentCost: response.amount };
    });
  };

  if (!storeItem) {
    return (
      <Backdrop open={true}>
        <CircularProgress />
      </Backdrop>
    );
  }

  return (
    <>
      <h2>Detail skladové položky</h2>
      <Box display="flex" gap={5}>
        <form onSubmit={handleStoreItemSubmit(saveStoreItem)}>
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
            <TextField label="Název" {...registerStoreItem("name")} />
            <TextField
              label="Název jednotky"
              {...registerStoreItem("unitName")}
            />
            <FormControl>
              <InputLabel id="categorySelect">Kategorie</InputLabel>
              <Controller
                name="categoryIds"
                control={controlStoreItem}
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

        <form onSubmit={handleCostSubmit(updateCost)}>
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
            <TextField
              label="Popis nové ceny"
              {...registerCost("description", {
                required: true,
              })}
              error={costErrors.description ? true : false}
            />
            <TextField
              label="Nová cena"
              {...registerCost("amount", {
                pattern: /^-?(?:0|[1-9]\d+)(?:\.\d{1,2})?$/,
                required: true,
              })}
              error={costErrors.amount ? true : false}
            />
            <Button type="submit" variant="contained">
              Nastavit cenu
            </Button>
          </Box>
        </form>
      </Box>
    </>
  );
};
