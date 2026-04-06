import { useEffect, useState } from "react";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import type { CashBoxListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const CashBoxPicker = ({
  onChange,
  excludeId,
  label,
  options,
  error,
  helperText,
  initialValue,
}: {
  onChange: (val: CashBoxListModel | undefined) => void;
  label?: string;
  excludeId?: number;
  error: boolean;
  options?: CashBoxListModel[];
  helperText?: string;
  initialValue?: number;
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
    <FormControl error={error} fullWidth>
      <InputLabel id="cashBoxPicker">{label ?? "Výběr kasy"}</InputLabel>
      <Select
        label={label ?? "Výběr kasy"}
        labelId="cashBoxPicker"
        value={value ?? ""}
        onChange={(evt) => {
          setValue(evt.target.value);
          if (evt.target.value === 0) {
            onChange(undefined);
          } else {
            const cashbox = cashBoxes?.find((c) => c.id === evt.target.value);
            if (!cashbox) {
              return;
            }
            onChange(cashbox);
          }
        }}
      >
        {cashBoxes
          ? cashBoxes.map((val) =>
              val.id !== excludeId ? (
                <MenuItem key={val.id} value={val.id}>
                  {val.name}
                </MenuItem>
              ) : null,
            )
          : null}
      </Select>
      <FormHelperText>{helperText}</FormHelperText>
    </FormControl>
  );
};

export default CashBoxPicker;
