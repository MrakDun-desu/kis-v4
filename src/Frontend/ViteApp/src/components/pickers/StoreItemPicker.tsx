import { useRef, useState } from "react";
import {
  Autocomplete,
  FormControl,
  FormHelperText,
  TextField,
} from "@mui/material";
import type { StoreItemListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const StoreItemPicker = ({
  onChange,
  containerItemsOnly,
  error,
  helperText,
}: {
  onChange: (val: number | undefined) => void;
  containerItemsOnly?: boolean;
  error: boolean;
  helperText: string | undefined;
}) => {
  const [storeItems, setStoreItems] = useState<StoreItemListModel[]>();
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
            setStoreItems(undefined);
          } else {
            debounceRef.current = setTimeout(async () => {
              const IsContainerItem = containerItemsOnly ?? undefined;
              const { response, data, error } = await apiClient.GET(
                "/store-items",
                {
                  params: {
                    query: { PageSize: 20, name: value, IsContainerItem },
                  },
                },
              );
              setStoreItems(data?.data);
              if (!response.ok) {
                handleApiError(response, error);
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
