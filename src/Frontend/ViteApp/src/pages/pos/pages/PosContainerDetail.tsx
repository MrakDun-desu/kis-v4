import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Box, Typography, Skeleton, Paper } from "@mui/material";
import ContainerSaleItemView from "../../../components/views/ContainerSaleItemView";
import type { ContainerOperatorReadResponse } from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const PosContainerDetail = () => {
  const [container, setContainer] = useState<ContainerOperatorReadResponse>();
  const { id } = useParams();

  useEffect(() => {
    const getContainer = async () => {
      const { response, data } = await apiClient.GET(
        "/containers/{id}/operator",
        { params: { path: { id: Number(id) } } },
      );
      setContainer(data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getContainer();
  }, []);

  if (!container) {
    return (
      <Box padding={1} display="flex" flexDirection="column" gap={2}>
        <Typography variant="h5" component="h2" marginBottom={3}>
          Kegy
        </Typography>
        <Skeleton variant="rounded" width="100%" height="5em" />
        <Skeleton variant="rounded" width="100%" height="5em" />
        <Skeleton variant="rounded" width="100%" height="5em" />
      </Box>
    );
  }

  return (
    <Box padding={1} flex="1" display="flex" flexDirection="column">
      <Typography variant="h5" component="h2" marginBottom={3}>
        Prodejní položky v kegu {container.template.name} (ID {container.id})
      </Typography>
      <Box
        display="grid"
        gridTemplateColumns="repeat(4, 1fr)"
        gridTemplateRows="repeat(4, 1fr)"
        flexGrow="1"
        gap={1}
        padding={1}
      >
        {container.saleItems.map((item) => (
          <Paper key={item.id} elevation={4} sx={{ position: "relative" }}>
            <ContainerSaleItemView saleItem={item} />
          </Paper>
        ))}
      </Box>
    </Box>
  );
};

export default PosContainerDetail;
