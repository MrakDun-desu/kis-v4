import { useEffect, useState } from "react";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import type { EntityWithName } from "../../stores/posStore";
import type { StoreListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const StorePicker = ({
  onChange,
  options,
  error,
  helperText,
  initialValue,
  labelText,
}: {
  onChange: (val: EntityWithName | undefined) => void;
  options?: StoreListModel[];
  error?: boolean;
  helperText?: string;
  initialValue?: number;
  labelText?: string;
}) => {
  const [stores, setStores] = useState<StoreListModel[] | undefined>(options);
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    if (stores) {
      return;
    }
    const getStores = async () => {
      const { response, data, error } = await apiClient.GET("/stores");
      setStores(data?.data);
      if (!response.ok) {
        handleApiError(response, error);
      }
    };
    getStores();
  }, []);

  return (
    <FormControl error={error} fullWidth>
      <InputLabel id="storePicker">{labelText ?? "Výběr skladu"}</InputLabel>
      <Select
        label={labelText ?? "Výběr skladu"}
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
