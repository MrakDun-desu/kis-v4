import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useEffect, useState } from "react";
import {
  ContainerTemplatesApi,
  type ContainerTemplateModel,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new ContainerTemplatesApi(defaultConfiguration);

const ContainerTemplateFilter = ({
  onChange,
}: {
  onChange: (id: number | undefined) => void;
}) => {
  const [current, setCurrent] = useState<number>();
  const [containerTemplates, setContainerTemplates] = useState<
    ContainerTemplateModel[] | null
  >(null);
  useEffect(() => {
    const getContainerTemplates = async () => {
      const response = await handleApiCall(api.containerTemplatesReadAll());
      if (!response) {
        setContainerTemplates(null);
        return;
      }
      setContainerTemplates(response.data);
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
