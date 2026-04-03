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
import type { ContainerChangeModel } from "../../api/apiTypes";
import { containerStates } from "../../constants/containerStates";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const ContainerChangeListView = ({
  containerId,
  refreshCounter,
}: {
  containerId: number;
  refreshCounter: number;
}) => {
  const [containerChanges, setContainerChanges] =
    useState<ContainerChangeModel[]>();

  useEffect(() => {
    const getContainerChanges = async () => {
      const { response, data, error } = await apiClient.GET(
        "/container-changes",
        { params: { query: { ContainerId: containerId } } },
      );
      setContainerChanges(data?.data);
      if (!response.ok) {
        handleApiError(response, error);
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
            <TableRow key={c.timestamp}>
              <TableCell>
                {new Date(c.timestamp).toLocaleString("cs")}
              </TableCell>
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
