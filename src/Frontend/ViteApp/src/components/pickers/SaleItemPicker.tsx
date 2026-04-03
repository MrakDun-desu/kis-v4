import { useRef, useState } from "react";
import {
  Autocomplete,
  FormControl,
  FormHelperText,
  TextField,
} from "@mui/material";
import type { SaleItemListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const SaleItemPicker = ({
  onChange,
  error,
  helperText,
  initialValue,
}: {
  onChange: (val: number | undefined) => void;
  error: boolean;
  helperText?: string;
  initialValue?: string;
}) => {
  const [saleItems, setSaleItems] = useState<SaleItemListModel[]>();
  const [loading, setLoading] = useState(false);
  const [currentValue, setCurrentValue] = useState(initialValue);
  const debounceRef = useRef<ReturnType<typeof setTimeout>>(undefined);

  return (
    <FormControl fullWidth error={error}>
      <Autocomplete
        options={saleItems ?? []}
        loading={loading}
        noOptionsText={saleItems ? "Žádné možnosti" : "Začněte vyhledávat..."}
        loadingText={"Hledání..."}
        filterOptions={(x) => x}
        inputValue={currentValue ?? ""}
        onInputChange={(_e, value, reason) => {
          if (reason !== "reset") {
            setCurrentValue(value);
          }
          if (reason !== "input") {
            return;
          }

          if (debounceRef.current) {
            clearTimeout(debounceRef.current);
          }

          if (!value || value.length < 1) {
            setSaleItems(undefined);
          } else {
            debounceRef.current = setTimeout(async () => {
              const { response, data, error } = await apiClient.GET(
                "/sale-items",
                {
                  params: { query: { PageSize: 20, Name: value } },
                },
              );
              setSaleItems(data?.data);
              if (!response.ok) {
                handleApiError(response, error);
              }
              setLoading(false);
            }, 500);
          }
        }}
        getOptionLabel={(opt) => `${opt.name}`}
        renderInput={(params) => (
          <TextField {...params} size="small" label="Položka" />
        )}
        onChange={(_e, val) => {
          onChange(val ? val.id : undefined);
        }}
      />
      <FormHelperText>{helperText}</FormHelperText>
    </FormControl>
  );
};

export default SaleItemPicker;
