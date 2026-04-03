import { useEffect, useState } from "react";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import type { EntityWithName } from "../../stores/posStore";
import type { ContainerTemplateModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const ContainerTemplatePicker = ({
  onChange,
  options,
  error,
  helperText,
  initialValue,
}: {
  onChange: (val: EntityWithName | undefined) => void;
  options?: ContainerTemplateModel[];
  error: boolean;
  helperText?: string;
  initialValue?: number;
}) => {
  const [containerTemplates, setContainerTemplates] = useState<
    ContainerTemplateModel[] | undefined
  >(options);
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    if (containerTemplates) {
      return;
    }
    const getContainerTemplates = async () => {
      const { response, data } = await apiClient.GET("/container-templates");
      setContainerTemplates(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getContainerTemplates();
  }, []);

  return (
    <FormControl error={error} fullWidth>
      <InputLabel id="containerTemplatePicker">Typ kegu</InputLabel>
      <Select
        label="Výběr skladu"
        labelId="containerTemplatePicker"
        value={value ?? ""}
        error={error}
        onChange={(evt) => {
          setValue(evt.target.value);
          if (evt.target.value === null) {
            onChange(undefined);
          } else {
            onChange({
              id: evt.target.value,
              name: containerTemplates!.find((c) => c.id === evt.target.value)!
                .name,
            });
          }
        }}
      >
        {containerTemplates
          ? containerTemplates.map((val) => (
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

export default ContainerTemplatePicker;
