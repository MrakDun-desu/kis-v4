import { Link, useParams } from "react-router-dom";
import {
  ContainerChangesApi,
  ContainersApi,
  type ContainerChangeCreateRequest,
  type ContainerReadResponse,
  type ContainerUpdateModel,
} from "../../../api-generated";
import { useEffect, useState } from "react";
import { Box, Button, Skeleton, Typography } from "@mui/material";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { useLoading } from "../../../contexts/LoadingContext";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import StorePicker from "../../../components/pickers/StorePicker";
import PipePicker from "../../../components/pickers/PipePicker";
import ContainerChangeListView from "../../../components/views/ContainerChangeListView";
import { containerStates } from "../../../constants/containerStates";

const api = new ContainersApi(defaultConfiguration);
const changesApi = new ContainerChangesApi(defaultConfiguration);

const ContainerDetail = () => {
  const [container, setContainer] = useState<ContainerReadResponse | null>(
    null,
  );
  const { startLoading, stopLoading } = useLoading();
  const [changeRefreshCounter, setChangeRefreshCounter] = useState(0);
  const { id } = useParams();

  useEffect(() => {
    const getContainer = async () => {
      const response = await handleApiCall(
        api.containersRead({
          id: Number(id),
        }),
      );
      setContainer(response);
    };
    getContainer();
  }, []);

  const updateContainer = async (data: ContainerUpdateModel) => {
    startLoading();
    const response = await handleApiCall(
      api.containersUpdate({
        id: Number(id),
        containerUpdateModel: data,
      }),
    );
    stopLoading();
    if (response) {
      setChangeRefreshCounter((prev) => prev + 1);
      setContainer((prev) =>
        prev
          ? {
              ...response,
              containerChanges: prev.containerChanges,
            }
          : null,
      );
    }
  };

  const changeContainer = async (data: ContainerChangeCreateRequest) => {
    startLoading();
    const response = await handleApiCall(
      changesApi.containerChangesCreate({
        containerChangeCreateRequest: data,
      }),
    );
    stopLoading();
    if (response) {
      setChangeRefreshCounter((prev) => prev + 1);
      setContainer((prev) =>
        prev
          ? {
              ...prev,
              amount: response.newAmount,
              state: response.newState,
              pipe: null,
            }
          : null,
      );
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
