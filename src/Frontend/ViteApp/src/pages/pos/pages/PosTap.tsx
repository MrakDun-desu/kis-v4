import { Box, Button, Paper, Skeleton, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useLoading } from "../../../contexts/LoadingContext";
import type {
  ContainerChangeCreateRequest,
  ContainerListModel,
  TapReadResponse,
} from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const PosTap = () => {
  const { id } = useParams();
  const [tap, setTap] = useState<TapReadResponse>();
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { startLoading, stopLoading } = useLoading();
  const navigate = useNavigate();

  useEffect(() => {
    const getTap = async () => {
      const { data, response, error } = await apiClient.GET("/taps/{id}", {
        params: {
          path: { id: Number(id) },
        },
      });
      if (!response.ok) {
        handleApiError(response, error);
      }
      setTap(data);
    };
    getTap();
  }, [refreshCounter]);

  const addContainer = async (containerId: number) => {
    if (!tap) {
      return;
    }
    startLoading();
    const { response, data, error } = await apiClient.PUT("/taps/{id}", {
      params: { path: { id: Number(id) } },
      body: { name: tap.name, containerId },
    });

    if (data) {
      setTap(
        (prev) =>
          prev && {
            ...prev,
            containerId: data.containerId,
            containers: prev.containers?.map((c) => {
              if (c.id !== data.containerId) {
                return c;
              }
              const output: ContainerListModel = {
                ...c,
                store: data.store,
                tap: {
                  ...data,
                },
              };
              return output;
            }),
          },
      );
    } else {
      handleApiError(response, error);
    }
    stopLoading();
  };

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

  const removeContainer = async () => {
    if (!tap) {
      return;
    }
    startLoading();
    const { response, error } = await apiClient.PUT("/taps/{id}", {
      params: { path: { id: Number(id) } },
      body: {
        name: tap.name,
      },
    });
    if (response.ok) {
      setRefreshCounter((prev) => prev + 1);
    } else {
      handleApiError(response, error);
    }
    stopLoading();
  };

  if (!tap) {
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
        Pípa {tap.name}
      </Typography>

      {tap.containers.length === 0 ? (
        <Typography>Žádné dostupné kegy</Typography>
      ) : (
        <Box display="flex" flexDirection="column-reverse" gap={2}>
          {tap.containers.map((c) => (
            <Paper
              key={c.id}
              elevation={4}
              sx={{
                padding: 2,
                order: tap.containerId == c.id ? 1 : undefined,
              }}
              onClick={() => {
                if (c.tap?.id === tap.id) {
                  navigate(`/pos/containers/${c.id}`);
                }
              }}
            >
              <Box
                display="flex"
                justifyContent="space-between"
                alignItems="center"
              >
                <Box display="flex" flexDirection="column" gap={1}>
                  <Typography fontWeight="bold">{c.template.name}</Typography>

                  <Typography>
                    Aktuální množství: {c.amount}
                    {c.template.storeItem.unitName}
                  </Typography>

                  <Typography>
                    Sklad: <b>{c.store.name}</b>
                  </Typography>

                  {tap.containerId === c.id && (
                    <Typography fontWeight="bold">Naražen zde</Typography>
                  )}

                  {c.tap && tap.containerId !== c.id && (
                    <Typography fontWeight="bold">
                      Naražen na pípě {c.tap.name}
                    </Typography>
                  )}
                </Box>

                <Box display="flex" gap={1}>
                  <Button
                    variant="outlined"
                    color="error"
                    onClick={(evt) => {
                      evt.stopPropagation();
                      const confirmed = confirm(
                        "Opravdu chcete tento keg odepsat?",
                      );
                      if (!confirmed) {
                        return;
                      }
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
                      const confirmed = confirm(
                        "Opravdu chcete tento keg označit za špatný?",
                      );
                      if (!confirmed) {
                        return;
                      }
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

                  {tap.containerId === c.id && (
                    <Button
                      variant="outlined"
                      onClick={(evt) => {
                        evt.stopPropagation();
                        removeContainer();
                      }}
                    >
                      Odrazit
                    </Button>
                  )}

                  {tap.containerId === null && !c.tap && (
                    <Button
                      variant="contained"
                      size="large"
                      onClick={(evt) => {
                        evt.stopPropagation();
                        addContainer(c.id);
                      }}
                    >
                      Narazit
                    </Button>
                  )}
                </Box>
              </Box>
            </Paper>
          ))}
        </Box>
      )}
    </Box>
  );
};

export default PosTap;
