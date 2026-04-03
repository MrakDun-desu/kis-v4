import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import type { PipeListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const PipeFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [pipes, setPipes] = useState<PipeListModel[]>();
  useEffect(() => {
    const getPipes = async () => {
      const { data, response } = await apiClient.GET("/pipes");
      setPipes(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
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
