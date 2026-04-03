import { useEffect, useState } from "react";
import {
  Autocomplete,
  FormControl,
  FormHelperText,
  TextField,
} from "@mui/material";
import type { LayoutListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const LayoutPicker = ({
  onChange,
  options,
  error,
  helperText,
  initialValue,
}: {
  onChange: (val: number | undefined) => void;
  options?: LayoutListModel[];
  error: boolean;
  helperText?: string;
  initialValue?: string;
}) => {
  const [layouts, setLayouts] = useState<LayoutListModel[] | undefined>(
    options,
  );
  const [loading, setLoading] = useState(false);
  const [inputValue, setInputValue] = useState(initialValue);

  useEffect(() => {
    if (layouts) {
      return;
    }
    const getLayouts = async () => {
      setLoading(true);
      const { data, response } = await apiClient.GET("/layouts");
      setLayouts(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
      setLoading(false);
    };
    getLayouts();
  }, []);

  return (
    <FormControl fullWidth error={error}>
      <Autocomplete
        options={layouts ?? []}
        loading={loading}
        noOptionsText={layouts ? "Žádné možnosti" : "Začněte vyhledávat..."}
        loadingText={"Hledání..."}
        inputValue={inputValue}
        onInputChange={(_e, v, reason) => {
          if (reason !== "reset") {
            setInputValue(v);
          }
        }}
        getOptionLabel={(opt) => `${opt.name}`}
        renderInput={(params) => (
          <TextField {...params} error={error} size="small" label="Položka" />
        )}
        onChange={(_e, val) => {
          onChange(val ? val.id : undefined);
        }}
      />
      <FormHelperText>{helperText}</FormHelperText>
    </FormControl>
  );
};

export default LayoutPicker;
