import { useEffect, useState } from "react";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import {
  ContainerTemplatesApi,
  type ContainerTemplateModel,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";
import type { EntityWithName } from "../../stores/posStore";

const api = new ContainerTemplatesApi(defaultConfiguration);

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
      const resp = await handleApiCall(api.containerTemplatesReadAll());
      if (resp) {
        setContainerTemplates(resp.data);
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
