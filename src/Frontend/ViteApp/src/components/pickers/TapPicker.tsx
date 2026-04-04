import { useEffect, useState } from "react";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import type { EntityWithName } from "../../stores/posStore";
import type { TapListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const TapPicker = ({
  onChange,
  options,
  error,
  helperText,
  initialValue,
  small,
}: {
  onChange: (val: EntityWithName | undefined) => void;
  options?: TapListModel[];
  error?: boolean;
  helperText?: string;
  initialValue?: number;
  small?: boolean;
}) => {
  const [taps, setTaps] = useState<TapListModel[] | undefined>(options);
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    if (taps) {
      return;
    }
    const getTaps = async () => {
      const { response, data } = await apiClient.GET("/taps");
      setTaps(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getTaps();
  }, []);

  return (
    <FormControl error={error} fullWidth size={small ? "small" : "medium"}>
      <InputLabel id="tapPicker">Výběr pípy</InputLabel>
      <Select
        size={small ? "small" : "medium"}
        label="Výběr pípy"
        labelId="tapPicker"
        value={value ?? ""}
        error={error}
        onChange={(evt) => {
          setValue(evt.target.value);
          if (evt.target.value === -1) {
            onChange(undefined);
          } else {
            onChange({
              id: evt.target.value,
              name: taps!.find((c) => c.id === evt.target.value)!.name,
            });
          }
        }}
      >
        {taps
          ? [
              <MenuItem key="empty" value={-1}>
                Žádná
              </MenuItem>,
              ...taps.map((val) => (
                <MenuItem key={val.id} value={val.id}>
                  {val.name}
                </MenuItem>
              )),
            ]
          : null}
      </Select>
      <FormHelperText>{helperText}</FormHelperText>
    </FormControl>
  );
};

export default TapPicker;
