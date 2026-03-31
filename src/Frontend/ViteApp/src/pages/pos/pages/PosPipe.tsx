import { Box, Button, Paper, Skeleton, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  ContainerChangesApi,
  ContainersApi,
  PipesApi,
  type ContainerChangeCreateRequest,
  type PipeReadResponse,
} from "../../../api-generated";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { useLoading } from "../../../contexts/LoadingContext";
import { usePosStore } from "../../../stores/posStore";

const api = new PipesApi(defaultConfiguration);
const changesApi = new ContainerChangesApi(defaultConfiguration);
const containersApi = new ContainersApi(defaultConfiguration);

const PosPipe = () => {
  const { id } = useParams();
  const [pipe, setPipe] = useState<PipeReadResponse | null>(null);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { startLoading, stopLoading } = useLoading();
  const currentStore = usePosStore((state) => state.currentStore);
  const navigate = useNavigate();

  useEffect(() => {
    const getPipe = async () => {
      const response = await handleApiCall(
        api.pipesRead({
          id: Number(id),
          storeId: currentStore?.id,
        }),
      );
      setPipe(response);
    };
    getPipe();
  }, [refreshCounter]);

  const changeContainer = async (data: ContainerChangeCreateRequest) => {
    startLoading();
    const response = await handleApiCall(
      changesApi.containerChangesCreate({
        containerChangeCreateRequest: data,
      }),
    );
    stopLoading();
    if (response) {
      setRefreshCounter((prev) => prev + 1);
    }
  };

  const removeFromPipe = async (id: number, storeId: number) => {
    startLoading();
    const response = await handleApiCall(
      containersApi.containersUpdate({
        id,
        containerUpdateModel: { storeId, pipeId: undefined },
      }),
    );
    stopLoading();
    if (response) {
      setRefreshCounter((prev) => prev + 1);
    }
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
