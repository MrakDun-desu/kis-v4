import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import { CategoriesApi, type CategoryModel } from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new CategoriesApi(defaultConfiguration);

const CategoryFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [categories, setCategories] = useState<CategoryModel[] | null>(null);
  useEffect(() => {
    const getCategories = async () => {
      const response = await handleApiCall(api.categoriesReadAll());
      if (!response) {
        setCategories(null);
        return;
      }
      setCategories(response.data);
    };
    getCategories();
  }, []);

  return (
    <FormControl fullWidth>
      <InputLabel id="categoryFilterLabel">
        Filtrování podle kategorie
      </InputLabel>
      <Select
        id="categoryFilter"
        label="Filtrování podle kategorie"
        labelId="categoryFilterLabel"
        value={current ?? ""}
        onChange={(evt) => {
          const newValue =
            evt.target.value === 0 ? undefined : evt.target.value;
          onChange(newValue);
          setCurrent(newValue);
        }}
      >
        {categories
          ? [
              <MenuItem key="empty" value={0}>
                Zobrazit všechny
              </MenuItem>,
              ...categories.map((category) => (
                <MenuItem key={category.id} value={category.id}>
                  {category.name}
                </MenuItem>
              )),
            ]
          : null}
      </Select>
    </FormControl>
  );
};

export default CategoryFilter;
