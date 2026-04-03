import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import type { CompositionModel } from "../../api/apiTypes";
import { apiClient } from "../../api/apiClient";
import handleApiError from "../../errorHandling/apiResponseHandler";

const CompositionListView = ({
  compositeId,
  refreshCounter,
}: {
  compositeId: number;
  refreshCounter: number;
}) => {
  const [compositions, setCompositions] = useState<CompositionModel[]>();

  useEffect(() => {
    const getCompositions = async () => {
      const { response, data, error } = await apiClient.GET("/compositions", {
        params: { query: { CompositeId: compositeId } },
      });
      setCompositions(data?.data);
      if (!response.ok) {
        handleApiError(response, error);
      }
    };
    getCompositions();
  }, [refreshCounter]);

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Skladová položka</TableCell>
            <TableCell>Množství</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {compositions?.map((c) => (
            <TableRow key={c.storeItem.id}>
              <TableCell>
                <Link to={`/admin/store-items/${c.storeItem.id}`}>
                  {c.storeItem.name}
                </Link>
              </TableCell>
              <TableCell>{`${c.amount} ${c.storeItem.unitName}`}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default CompositionListView;
