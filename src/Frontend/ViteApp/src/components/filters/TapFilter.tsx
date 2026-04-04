import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import type { TapListModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const TapFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [taps, setTaps] = useState<TapListModel[]>();
  useEffect(() => {
    const getTaps = async () => {
      const { data, response } = await apiClient.GET("/taps");
      setTaps(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getTaps();
  }, []);

  return (
    <FormControl fullWidth>
      <InputLabel id="tapFilterLabel">Filtrování podle pípy</InputLabel>
      <Select
        id="tapFilter"
        label="Filtrování podle pípy"
        labelId="tapFilterLabel"
        value={current ?? ""}
        onChange={(evt) => {
          const newValue =
            evt.target.value === 0 ? undefined : evt.target.value;
          onChange(newValue);
          setCurrent(newValue);
        }}
      >
        {taps
          ? [
              <MenuItem key="empty" value={0}>
                Zobrazit všechny
              </MenuItem>,
              ...taps.map((tap) => (
                <MenuItem key={tap.id} value={tap.id}>
                  {tap.name}
                </MenuItem>
              )),
            ]
          : null}
      </Select>
    </FormControl>
  );
};

export default TapFilter;
