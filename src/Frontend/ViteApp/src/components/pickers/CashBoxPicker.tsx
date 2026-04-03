import { useEffect, useState } from "react";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import type { EntityWithName } from "../../stores/posStore";
import type { CashBoxListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const CashBoxPicker = ({
  onChange,
  options,
  error,
  helperText,
  initialValue,
}: {
  onChange: (val: EntityWithName | undefined) => void;
  options?: CashBoxListModel[];
  error: boolean;
  helperText?: string;
  initialValue: number | undefined;
}) => {
  const [cashBoxes, setCashBoxes] = useState<CashBoxListModel[] | undefined>(
    options,
  );
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    if (cashBoxes) {
      return;
    }
    const getCashBoxes = async () => {
      const { data, response } = await apiClient.GET("/cashboxes");
      setCashBoxes(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getCashBoxes();
  }, []);

  return (
    <FormControl error={error}>
      <InputLabel id="cashBoxPicker">Výběr kasy</InputLabel>
      <Select
        label="Výběr kasy"
        labelId="cashBoxPicker"
        value={value ?? ""}
        onChange={(evt) => {
          setValue(evt.target.value);
          if (evt.target.value === 0) {
            onChange(undefined);
          } else {
            onChange({
              id: evt.target.value,
              name: cashBoxes!.find((c) => c.id === evt.target.value)!.name,
            });
          }
        }}
      >
        {cashBoxes
          ? cashBoxes.map((val) => (
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

export default CashBoxPicker;
