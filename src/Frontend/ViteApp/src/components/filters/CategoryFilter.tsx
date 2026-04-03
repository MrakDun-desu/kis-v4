import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import type { CategoryModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const CategoryFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [categories, setCategories] = useState<CategoryModel[]>();
  useEffect(() => {
    const getCategories = async () => {
      const { data, response } = await apiClient.GET("/categories");
      setCategories(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
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
