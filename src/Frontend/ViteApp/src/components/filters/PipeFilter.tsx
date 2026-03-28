import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import { PipesApi, type PipeListModel } from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new PipesApi(defaultConfiguration);

const PipeFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [pipes, setPipes] = useState<PipeListModel[] | null>(null);
  useEffect(() => {
    const getPipes = async () => {
      const response = await handleApiCall(api.pipesReadAll());
      if (!response) {
        setPipes(null);
        return;
      }
      setPipes(response.data);
    };
    getPipes();
  }, []);

  return (
    <FormControl fullWidth>
      <InputLabel id="pipeFilterLabel">Filtrování podle pípy</InputLabel>
      <Select
        id="pipeFilter"
        label="Filtrování podle pípy"
        labelId="pipeFilterLabel"
        value={current ?? ""}
        onChange={(evt) => {
          const newValue =
            evt.target.value === 0 ? undefined : evt.target.value;
          onChange(newValue);
          setCurrent(newValue);
        }}
      >
        {pipes
          ? [
              <MenuItem key="empty" value={0}>
                Zobrazit všechny
              </MenuItem>,
              ...pipes.map((pipe) => (
                <MenuItem key={pipe.id} value={pipe.id}>
                  {pipe.name}
                </MenuItem>
              )),
            ]
          : null}
      </Select>
    </FormControl>
  );
};

export default PipeFilter;
