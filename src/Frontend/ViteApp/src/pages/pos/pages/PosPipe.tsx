import { Box, Button, Paper, Skeleton, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useLoading } from "../../../contexts/LoadingContext";
import { usePosStore } from "../../../stores/posStore";
import type {
  ContainerChangeCreateRequest,
  PipeReadResponse,
} from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const PosPipe = () => {
  const { id } = useParams();
  const [pipe, setPipe] = useState<PipeReadResponse>();
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { startLoading, stopLoading } = useLoading();
  const currentStore = usePosStore((state) => state.currentStore);
  const navigate = useNavigate();

  useEffect(() => {
    const getPipe = async () => {
      const { data, response, error } = await apiClient.GET("/pipes/{id}", {
        params: {
          path: { id: Number(id) },
          query: { StoreId: currentStore?.id },
        },
      });
      if (!response.ok) {
        handleApiError(response, error);
      }
      setPipe(data);
    };
    getPipe();
  }, [refreshCounter]);

  const changeContainer = async (requestBody: ContainerChangeCreateRequest) => {
    startLoading();
    const { response, error } = await apiClient.POST("/container-changes", {
      body: requestBody,
    });
    if (response.ok) {
      setRefreshCounter((prev) => prev + 1);
    } else {
      handleApiError(response, error);
    }
    stopLoading();
  };

  const removeFromPipe = async (id: number, storeId: number) => {
    startLoading();
    const { response, error } = await apiClient.PUT("/containers/{id}", {
      params: { path: { id } },
      body: {
        storeId,
      },
    });
    if (response.ok) {
      setRefreshCounter((prev) => prev + 1);
    } else {
      handleApiError(response, error);
    }
    stopLoading();
  };

  if (!pipe) {
    return (
      <Box padding={1} display="flex" flexDirection="column" gap={2}>
        <Typography variant="h5" component="h2" marginBottom={3}>
          Kegy na pípě ...
        </Typography>
        <Skeleton variant="rounded" width="100%" height="5em" />
        <Skeleton variant="rounded" width="100%" height="5em" />
        <Skeleton variant="rounded" width="100%" height="5em" />
      </Box>
    );
  }

  return (
    <Box padding={1} flex="1" minHeight={0} overflow="auto">
      <Typography variant="h5" component="h2" marginBottom={3}>
        Kegy na pípě {pipe.name}
      </Typography>

      {pipe.containers.length === 0 ? (
        <Typography>Pípa je prázdná</Typography>
      ) : (
        <Box display="flex" flexDirection="column" gap={2}>
          {pipe.containers.map((c) => (
            <Paper
              key={c.id}
              elevation={4}
              sx={{
                padding: 2,
              }}
              onClick={() => navigate(`/pos/containers/${c.id}`)}
            >
              <Box display="flex" justifyContent="space-between">
                <Box display="flex" flexDirection="column" gap={1}>
                  <Typography fontWeight="bold">{c.template.name}</Typography>

                  <Typography>
                    Aktuální množství: {c.amount}
                    {c.template.storeItem.unitName}
                  </Typography>
                </Box>

                <Box display="flex" gap={1}>
                  <Button
                    variant="outlined"
                    color="error"
                    onClick={(evt) => {
                      evt.stopPropagation();
                      changeContainer({
                        containerId: c.id,
                        newAmount: c.amount,
                        newState: "WrittenOff",
                      });
                    }}
                  >
                    Odepsat
                  </Button>
                  <Button
                    variant="outlined"
                    color="error"
                    onClick={(evt) => {
                      evt.stopPropagation();
                      changeContainer({
                        containerId: c.id,
                        newAmount: c.amount,
                        newState: "Bad",
                      });
                    }}
                  >
                    Označit
                    <br /> za špatný
                  </Button>
                  <Button
                    variant="outlined"
                    onClick={(evt) => {
                      evt.stopPropagation();
                      removeFromPipe(c.id, c.storeId);
                    }}
                  >
                    Odebrat
                    <br />z pípy
                  </Button>
                </Box>
              </Box>
            </Paper>
          ))}
        </Box>
      )}
    </Box>
  );
};

export default PosPipe;
