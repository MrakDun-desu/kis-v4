import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import type { StoreListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const StoreFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [stores, setStores] = useState<StoreListModel[]>();
  useEffect(() => {
    const getStores = async () => {
      const { response, data } = await apiClient.GET("/stores");
      setStores(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getStores();
  }, []);

  return (
    <FormControl fullWidth>
      <InputLabel id="storeFilterLabel">Filtrování podle skladu</InputLabel>
      <Select
        id="storeFilter"
        label="Filtrování podle skladu"
        labelId="storeFilterLabel"
        value={current ?? ""}
        onChange={(evt) => {
          const newValue =
            evt.target.value === 0 ? undefined : evt.target.value;
          onChange(newValue);
          setCurrent(newValue);
        }}
      >
        {stores
          ? [
              <MenuItem key="empty" value={0}>
                Zobrazit všechny
              </MenuItem>,
              ...stores.map((store) => (
                <MenuItem key={store.id} value={store.id}>
                  {store.name}
                </MenuItem>
              )),
            ]
          : null}
      </Select>
    </FormControl>
  );
};

export default StoreFilter;
