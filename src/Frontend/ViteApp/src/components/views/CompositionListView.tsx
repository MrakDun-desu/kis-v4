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
import { CompositionsApi, type CompositionModel } from "../../api-generated";
import { defaultConfiguration } from "../../configuration/apiConfiguration";
import handleApiCall from "../../errorHandling/apiResponseHandler";

const api = new CompositionsApi(defaultConfiguration);

const CompositionListView = ({
  compositeId,
  refreshCounter,
}: {
  compositeId: number;
  refreshCounter: number;
}) => {
  const [compositions, setCompositions] = useState<CompositionModel[] | null>(
    null,
  );

  useEffect(() => {
    const getCompositions = async () => {
      const response = await handleApiCall(
        api.compositionsReadAll({ compositeId }),
      );
      if (!response) {
        setCompositions(null);
      } else {
        setCompositions(response.data);
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
