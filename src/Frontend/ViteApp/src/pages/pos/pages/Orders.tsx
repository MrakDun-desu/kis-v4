import { Box, Paper, Skeleton } from "@mui/material";
import { usePosStore } from "../../../stores/posStore";
import { useShallow } from "zustand/react/shallow";
import { useEffect } from "react";
import { useSnackbar } from "../../../contexts/SnackbarContext";
import LayoutItemView from "../../../components/views/LayoutItemView";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

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
    const { response, data, error } = !layoutId
      ? await apiClient.GET("/layouts/top-level", {
          params: { query: { StoreId: currentStore?.id } },
        })
      : await apiClient.GET("/layouts/{id}", {
          params: {
            path: { id: layoutId },
            query: { StoreId: currentStore?.id },
          },
        });

    if (data) {
      setLayout(data);
    } else {
      handleApiError(response, error, () =>
        showSnackbar("Není nastaveno výchozí rozložení!", "warning"),
      );
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
