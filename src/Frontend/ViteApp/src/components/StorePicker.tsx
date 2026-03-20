import { useEffect, useState } from "react";
import { StoresApi, type StoreListModel } from "../api-generated";
import handleApiCall from "../errorHandling/apiResponseHandler";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import { defaultConfiguration } from "../configuration/apiConfiguration";
import type { EntityWithName } from "../stores/posStore";

const api = new StoresApi(defaultConfiguration);

const StorePicker = ({
  onChange,
  options,
  error,
  helperText,
  initialValue,
}: {
  onChange: (val: EntityWithName | undefined) => void;
  options?: StoreListModel[];
  error: boolean;
  helperText?: string;
  initialValue: number | undefined;
}) => {
  const [stores, setStores] = useState<StoreListModel[] | undefined>(options);
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    if (stores) {
      return;
    }
    const getStores = async () => {
      const resp = await handleApiCall(api.storesReadAll());
      if (resp) {
        setStores(resp.data);
      }
    };
    getStores();
  }, []);

  return (
    <FormControl error={error}>
      <InputLabel id="storePicker">Výběr skladu</InputLabel>
      <Select
        label="Výběr skladu"
        labelId="storePicker"
        value={value ?? ""}
        error={error}
        onChange={(evt) => {
          setValue(evt.target.value);
          if (evt.target.value === null) {
            onChange(undefined);
          } else {
            onChange({
              id: evt.target.value,
              name: stores!.find((c) => c.id === evt.target.value)!.name,
            });
          }
        }}
      >
        {stores
          ? stores.map((val) => (
              <MenuItem key={val.id} value={val.id}>
                {val.name}
              </MenuItem>
            ))
          : null}
      </Select>
      <FormHelperText>{helperText}</FormHelperText>
    </FormControl>
  );
};

export default StorePicker;
