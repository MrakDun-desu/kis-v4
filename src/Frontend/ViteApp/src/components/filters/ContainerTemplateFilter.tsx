import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import type { ContainerTemplateModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const ContainerTemplateFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [containerTemplates, setContainerTemplates] =
    useState<ContainerTemplateModel[]>();
  useEffect(() => {
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
    <FormControl fullWidth>
      <InputLabel id="containerTemplateFilterLabel">
        Filtrování podle typu kegu
      </InputLabel>
      <Select
        id="containerTemplateFilter"
        label="Filtrování podle typu kegu"
        labelId="containerTemplateFilterLabel"
        value={current ?? ""}
        onChange={(evt) => {
          const newValue =
            evt.target.value === 0 ? undefined : evt.target.value;
          onChange(newValue);
          setCurrent(newValue);
        }}
      >
        {containerTemplates
          ? [
              <MenuItem key="empty" value={0}>
                Zobrazit všechny
              </MenuItem>,
              ...containerTemplates.map((containerTemplate) => (
                <MenuItem
                  key={containerTemplate.id}
                  value={containerTemplate.id}
                >
                  {containerTemplate.name}
                </MenuItem>
              )),
            ]
          : null}
      </Select>
    </FormControl>
  );
};

export default ContainerTemplateFilter;
