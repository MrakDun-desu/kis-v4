import { useEffect, useState } from "react";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import {
  ContainerChangesApi,
  type ContainerChangeModel,
} from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";
import { containerStates } from "../../constants/containerStates";

const api = new ContainerChangesApi(defaultConfiguration);

const ContainerChangeListView = ({
  containerId,
  refreshCounter,
}: {
  containerId: number;
  refreshCounter: number;
}) => {
  const [containerChanges, setContainerChanges] = useState<
    ContainerChangeModel[] | null
  >(null);

  useEffect(() => {
    const getContainerChanges = async () => {
      const response = await handleApiCall(
        api.containerChangesReadAll({ containerId }),
      );
      if (!response) {
        setContainerChanges(null);
      } else {
        setContainerChanges(response.data);
      }
    };
    getContainerChanges();
  }, [refreshCounter]);

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Čas změny</TableCell>
            <TableCell>Stav</TableCell>
            <TableCell>Množství</TableCell>
            {/*<TableCell>Uživatel</TableCell> */}
          </TableRow>
        </TableHead>
        <TableBody>
          {containerChanges?.map((c) => (
            <TableRow key={c.timestamp.toDateString()}>
              <TableCell>{c.timestamp.toLocaleString("cs")}</TableCell>
              <TableCell>{containerStates[c.newState]}</TableCell>
              <TableCell>{c.newAmount}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default ContainerChangeListView;
