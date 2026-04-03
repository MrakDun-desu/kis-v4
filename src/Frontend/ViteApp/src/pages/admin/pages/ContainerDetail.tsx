import { Link, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Box, Button, Skeleton, Typography } from "@mui/material";
import { useLoading } from "../../../contexts/LoadingContext";
import StorePicker from "../../../components/pickers/StorePicker";
import PipePicker from "../../../components/pickers/PipePicker";
import ContainerChangeListView from "../../../components/views/ContainerChangeListView";
import { containerStates } from "../../../constants/containerStates";
import type {
  ContainerChangeCreateRequest,
  ContainerReadResponse,
  ContainerUpdateModel,
} from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const ContainerDetail = () => {
  const [container, setContainer] = useState<ContainerReadResponse>();
  const { startLoading, stopLoading } = useLoading();
  const [changeRefreshCounter, setChangeRefreshCounter] = useState(0);
  const { id } = useParams();

  useEffect(() => {
    const getContainer = async () => {
      const { response, data } = await apiClient.GET("/containers/{id}", {
        params: { path: { id: Number(id) } },
      });
      if (!data) {
        handleApiError(response);
      }
      setContainer(data);
    };
    getContainer();
  }, []);

  const updateContainer = async (requestBody: ContainerUpdateModel) => {
    startLoading();
    const { response, data } = await apiClient.PUT("/containers/{id}", {
      params: { path: { id: Number(id) } },
      body: requestBody,
    });
    stopLoading();
    if (data) {
      setChangeRefreshCounter((prev) => prev + 1);
      setContainer((prev) => ({
        ...data,
        containerChanges: prev?.containerChanges ?? [],
      }));
    } else {
      handleApiError(response);
    }
  };

  const changeContainer = async (requestBody: ContainerChangeCreateRequest) => {
    startLoading();
    const { data, response } = await apiClient.POST("/container-changes", {
      body: requestBody,
    });
    stopLoading();
    if (data) {
      setChangeRefreshCounter((prev) => prev + 1);
      setContainer((prev) =>
        prev
          ? {
              ...prev,
              amount: data.newAmount,
              state: data.newState,
            }
          : undefined,
      );
    } else {
      handleApiError(response);
    }
  };

  if (!container) {
    return (
      <>
        <Skeleton variant="rounded" width={300} height={30} />
        <Box display="flex" gap={5} marginTop={5}>
          <Box display="flex" flexDirection="column" gap={2}>
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
          </Box>
          <Box display="flex" flexDirection="column" gap={2}>
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
            <Skeleton variant="rounded" width={300} height={60} />
          </Box>
        </Box>
      </>
    );
  }

  return (
    <>
      <h2>Detail kegu</h2>
      <Box display="flex" gap={5}>
        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
          minWidth={300}
        >
          <h3>Aktuální stav kegu</h3>
          <Typography>
            Množství: {container.amount}
            {container.template.storeItem.unitName}
          </Typography>

          <Typography>Stav: {containerStates[container.state]}</Typography>

          <Typography>Typ: {container.template.name}</Typography>

          <Typography>
            Skladová položka:{" "}
            <Link to={`/admin/store-items/${container.template.storeItem.id}`}>
              {container.template.storeItem.name}
            </Link>
          </Typography>

          <StorePicker
            onChange={(val) =>
              val &&
              updateContainer({
                storeId: val.id,
                pipeId: container.pipe?.id,
              })
            }
            initialValue={container.store.id}
          />

          {(container.state === "New" || container.state === "Opened") && (
            <>
              <PipePicker
                onChange={(val) =>
                  updateContainer({
                    storeId: container.store.id,
                    pipeId: val?.id,
                  })
                }
                initialValue={container.pipe?.id}
              />

              <Button
                variant="outlined"
                color="error"
                onClick={() =>
                  changeContainer({
                    containerId: Number(id),
                    newAmount: container.amount,
                    newState: "WrittenOff",
                  })
                }
              >
                Odepsat
              </Button>

              <Button
                variant="contained"
                color="error"
                onClick={() =>
                  changeContainer({
                    containerId: Number(id),
                    newAmount: container.amount,
                    newState: "Bad",
                  })
                }
              >
                Označit jako špatný
              </Button>
            </>
          )}
        </Box>

        <Box
          display="flex"
          flexDirection="column"
          alignItems="flex-start"
          gap={2}
        >
          <h3>Historie změn</h3>

          <ContainerChangeListView
            containerId={container.id}
            refreshCounter={changeRefreshCounter}
          />
        </Box>
      </Box>
    </>
  );
};

export default ContainerDetail;
