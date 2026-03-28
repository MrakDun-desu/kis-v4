import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import { StoresApi, type StoreListModel } from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new StoresApi(defaultConfiguration);

const StoreFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [stores, setStores] = useState<StoreListModel[] | null>(null);
  useEffect(() => {
    const getStores = async () => {
      const response = await handleApiCall(api.storesReadAll());
      if (!response) {
        setStores(null);
        return;
      }
      setStores(response.data);
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
