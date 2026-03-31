import { Box, Paper, Skeleton } from "@mui/material";
import { usePosStore } from "../../../stores/posStore";
import { useShallow } from "zustand/react/shallow";
import { useEffect } from "react";
import { LayoutsApi } from "../../../api-generated";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { useSnackbar } from "../../../contexts/SnackbarContext";
import LayoutItemView from "../../../components/views/LayoutItemView";

const layoutsApi = new LayoutsApi(defaultConfiguration);

const Orders = () => {
  const { currentStore, currentLayout, layoutId, setLayout } = usePosStore(
    useShallow((state) => ({
      currentStore: state.currentStore,
      currentLayout: state.currentLayout,
      layoutId: state.currentLayoutId,
      setLayout: state.setCurrentLayout,
    })),
  );
  const { showSnackbar } = useSnackbar();

  const fetchLayout = async () => {
    const resp = !layoutId
      ? await handleApiCall(
          layoutsApi.layoutsReadTopLevel({ storeId: currentStore?.id }),
          () => showSnackbar("Není nastaveno výchozí rozložení!", "warning"),
        )
      : await handleApiCall(
          layoutsApi.layoutsRead({ id: layoutId, storeId: currentStore?.id }),
        );

    if (resp) {
      setLayout(resp);
    }
  };

  useEffect(() => {
    if (currentLayout?.id === layoutId && layoutId !== undefined) {
      return;
    }

    fetchLayout();
  }, [layoutId, currentLayout]);

  useEffect(() => {
    if (currentStore) {
      fetchLayout();
    }
  }, [currentStore?.id]);

  return (
    <Box
      display="grid"
      gridTemplateColumns="repeat(4, 1fr)"
      gridTemplateRows="repeat(4, 1fr)"
      flexGrow="1"
      gap={1}
      padding={1}
    >
      {Array.apply(null, Array(4)).map((_, x) =>
        Array.apply(null, Array(4)).map((_, y) =>
          !currentLayout ? (
            <Skeleton key={`${x}${y}`} variant="rounded" height="auto" />
          ) : (
            <Paper
              key={`${x}${y}`}
              elevation={4}
              sx={{
                position: "relative",
              }}
            >
              <LayoutItemView x={x + 1} y={y + 1} />
            </Paper>
          ),
        ),
      )}
    </Box>
  );
};

export default Orders;
