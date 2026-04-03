import {
  Box,
  Typography,
  Skeleton,
  Paper,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import { usePosStore } from "../../../stores/posStore";
import { useEffect, useState } from "react";
import { useLoading } from "../../../contexts/LoadingContext";
import PipePicker from "../../../components/pickers/PipePicker";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";
import type { ContainerListModel } from "../../../api/apiTypes";

const PosContainers = () => {
  const currentStore = usePosStore((state) => state.currentStore);
  const [containers, setContainers] = useState<ContainerListModel[]>();
  const [showPipeDialog, setShowPipeDialog] = useState(false);
  const [changePipeRequest, setChangePipeRequest] = useState<{
    pipeId?: number;
    containerId: number;
  }>({ containerId: 0 });
  const { startLoading, stopLoading } = useLoading();

  useEffect(() => {
    const getContainers = async () => {
      const { response, data } = await apiClient.GET("/containers", {
        params: { query: { IncludeUnusable: false, PageSize: 100 } },
      });
      setContainers(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getContainers();
  }, []);

  const transferContainer = async (
    containerId: number,
    pipeId: number | undefined,
  ) => {
    if (!currentStore) {
      return;
    }
    startLoading();
    const { response, data, error } = await apiClient.PUT("/containers/{id}", {
      params: { path: { id: containerId } },
      body: { storeId: currentStore.id, pipeId },
    });

    if (data) {
      setContainers((prev) =>
        prev?.map((c) => {
          if (c.id !== data.id) {
            return c;
          }
          const output: ContainerListModel = {
            ...data,
          };
          return output;
        }),
      );
    } else {
      handleApiError(response, error);
    }
    stopLoading();
  };

  if (!containers) {
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
    <Box padding={1} flex="1" minHeight={0} overflow="auto">
      <Typography variant="h5" component="h2" marginBottom={3}>
        Kegy
      </Typography>

      {containers.length === 0 ? (
        <Typography>Nejsou dostupné žádné kegy</Typography>
      ) : (
        <Box
          display="flex"
          flexDirection="column"
          gap={2}
          overflow="scroll"
          flex={1}
        >
          {containers.map((c) => (
            <Paper
              key={c.id}
              elevation={4}
              sx={{
                padding: 2,
              }}
            >
              <Box
                display="flex"
                justifyContent="space-between"
                alignItems="center"
              >
                <Box display="flex" flexDirection="column" gap={1}>
                  <Typography fontWeight="bold" fontSize="large">
                    {c.template.name}
                  </Typography>

                  <Typography>
                    Aktuální množství: {c.amount}
                    {c.template.storeItem.unitName}
                  </Typography>

                  <Typography>
                    Sklad: <b>{c.store.name}</b>
                  </Typography>

                  <Typography>
                    Pípa: <b>{c.pipe?.name ?? "Žádná"}</b>
                  </Typography>
                </Box>

                {currentStore && (
                  <Box display="flex" gap={1}>
                    {currentStore.id !== c.store.id && (
                      <Button
                        variant="contained"
                        size="large"
                        onClick={() => transferContainer(c.id, c.pipe?.id)}
                      >
                        Přesunout do
                        <br />
                        aktuálního skladu
                      </Button>
                    )}

                    {currentStore.id === c.store.id && (
                      <Button
                        variant="contained"
                        size="large"
                        onClick={() => {
                          setChangePipeRequest({
                            containerId: c.id,
                            pipeId: c.pipe?.id,
                          });
                          setShowPipeDialog(true);
                        }}
                      >
                        Změnit pípu
                      </Button>
                    )}
                  </Box>
                )}
              </Box>
            </Paper>
          ))}
        </Box>
      )}

      <Dialog open={showPipeDialog} onClose={() => setShowPipeDialog(false)}>
        <DialogTitle>Změnit pípu kegu</DialogTitle>
        <DialogContent>
          <Box
            display="flex"
            flexDirection="column"
            alignItems="flex-start"
            gap={2}
            marginTop={1}
          >
            <PipePicker
              onChange={(val) =>
                setChangePipeRequest((prev) => ({ ...prev, pipeId: val?.id }))
              }
              initialValue={changePipeRequest.pipeId}
            />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowPipeDialog(false)}>Zrušit</Button>
          <Button
            onClick={() => {
              transferContainer(
                changePipeRequest.containerId,
                changePipeRequest.pipeId,
              );
              setShowPipeDialog(false);
            }}
          >
            Změnit
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default PosContainers;
