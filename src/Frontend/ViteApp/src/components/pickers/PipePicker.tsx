import { useEffect, useState } from "react";
import {
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import { PipesApi, type PipeListModel } from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";
import type { EntityWithName } from "../../stores/posStore";

const api = new PipesApi(defaultConfiguration);

const PipePicker = ({
  onChange,
  options,
  error,
  helperText,
  initialValue,
}: {
  onChange: (val: EntityWithName | undefined) => void;
  options?: PipeListModel[];
  error?: boolean;
  helperText?: string;
  initialValue?: number;
}) => {
  const [pipes, setPipes] = useState<PipeListModel[] | undefined>(options);
  const [value, setValue] = useState(initialValue);

  useEffect(() => {
    if (pipes) {
      return;
    }
    const getPipes = async () => {
      const resp = await handleApiCall(api.pipesReadAll());
      if (resp) {
        setPipes(resp.data);
      }
    };
    getPipes();
  }, []);

  return (
    <FormControl error={error} fullWidth>
      <InputLabel id="pipePicker">Výběr pípy</InputLabel>
      <Select
        label="Výběr pípy"
        labelId="pipePicker"
        value={value ?? ""}
        error={error}
        onChange={(evt) => {
          setValue(evt.target.value);
          if (evt.target.value === -1) {
            onChange(undefined);
          } else {
            onChange({
              id: evt.target.value,
              name: pipes!.find((c) => c.id === evt.target.value)!.name,
            });
          }
        }}
      >
        {pipes
          ? [
              <MenuItem key="empty" value={-1}>
                Žádná
              </MenuItem>,
              ...pipes.map((val) => (
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

export default PipePicker;
