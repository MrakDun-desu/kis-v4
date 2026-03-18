import { useRef, useState } from "react";
import { StoreItemsApi, type StoreItemListModel } from "../api-generated";
import handleApiCall from "../errorHandling/apiResponseHandler";
import {
  Autocomplete,
  FormControl,
  FormHelperText,
  TextField,
} from "@mui/material";
import { defaultConfiguration } from "../configuration/apiConfiguration";

const api = new StoreItemsApi(defaultConfiguration);

const StoreItemPicker = ({
  onChange,
  error,
  helperText,
}: {
  onChange: (val: number | undefined) => void;
  error: boolean;
  helperText: string | undefined;
}) => {
  const [storeItems, setStoreItems] = useState<StoreItemListModel[] | null>(
    null,
  );
  const [loading, setLoading] = useState(false);
  const debounceRef = useRef<ReturnType<typeof setTimeout>>(undefined);

  return (
    <FormControl fullWidth error={error}>
      <Autocomplete
        options={storeItems ?? []}
        loading={loading}
        noOptionsText={storeItems ? "Žádné možnosti" : "Začněte vyhledávat..."}
        loadingText={"Hledání..."}
        filterOptions={(x) => x}
        onInputChange={(_e, value, reason) => {
          if (reason !== "input") {
            return;
          }

          if (debounceRef.current) {
            clearTimeout(debounceRef.current);
          }

          if (!value || value.length < 1) {
            setStoreItems(null);
          } else {
            debounceRef.current = setTimeout(async () => {
              const response = await handleApiCall(
                api.storeItemsReadAll({ page: 1, pageSize: 100, name: value }),
              );
              if (!response) {
                setStoreItems(null);
              } else {
                setStoreItems(response.data);
              }
              setLoading(false);
            }, 500);
          }
        }}
        getOptionLabel={(opt) => `${opt.name} (${opt.unitName})`}
        renderInput={(params) => (
          <TextField {...params} label="Skladová položka" />
        )}
        onChange={(_e, val) => {
          onChange(val ? val.id : undefined);
        }}
      />
      <FormHelperText>{helperText}</FormHelperText>
    </FormControl>
  );
};

export default StoreItemPicker;
