import { Box, Paper, Skeleton, Typography } from "@mui/material";
import {
  usePosStore,
  type SaleTransactionItemDisplay,
} from "../../../stores/posStore";
import { useShallow } from "zustand/react/shallow";
import { useEffect } from "react";
import {
  instanceOfLayoutItemModelLayoutSaleItemModel,
  LayoutsApi,
  type LayoutReadResponse,
} from "../../../api-generated";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { GridView, ShoppingBag, WaterDrop } from "@mui/icons-material";
import { useSnackbar } from "../../../contexts/SnackbarContext";

const layoutsApi = new LayoutsApi(defaultConfiguration);

const Orders = () => {
  const {
    transactionItems,
    currentStore,
    currentLayout,
    layoutId,
    setLayout,
    setLayoutId,
    addTransactionItem,
    updateTransactionItem,
  } = usePosStore(
    useShallow((state) => ({
      transactionItems: state.transactionItems,
      currentStore: state.currentStore,
      currentLayout: state.currentLayout,
      layoutId: state.currentLayoutId,
      setLayout: state.setCurrentLayout,
      setLayoutId: state.setLayoutId,
      addTransactionItem: state.addTransactionItem,
      updateTransactionItem: state.updateTransactionItem,
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
              <LayoutItem
                x={x + 1}
                y={y + 1}
                currentLayout={currentLayout}
                changeLayout={setLayoutId}
                addItem={addTransactionItem}
                updateItem={updateTransactionItem}
                transactionItems={transactionItems}
              />
            </Paper>
          ),
        ),
      )}
    </Box>
  );
};

export default Orders;

const LayoutItem = ({
  x,
  y,
  currentLayout,
  changeLayout,
  addItem,
  updateItem,
  transactionItems,
}: {
  x: number;
  y: number;
  currentLayout: LayoutReadResponse;
  changeLayout: (val: number) => void;
  addItem: (item: SaleTransactionItemDisplay) => void;
  updateItem: (
    index: number,
    item: Partial<SaleTransactionItemDisplay>,
  ) => void;
  transactionItems: SaleTransactionItemDisplay[];
}) => {
  const layoutItem = currentLayout.layoutItems.find(
    (li) => li.x === x && li.y === y,
  );

  if (!layoutItem) {
    return;
  }

  const amountInOrder =
    layoutItem.type === "SaleItem"
      ? transactionItems.reduce((acc, curr) => {
          if (curr.saleItemId === layoutItem.target.id) {
            return acc + curr.amount;
          }
          return acc;
        }, 0)
      : undefined;

  return (
    <>
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        height="100%"
        padding={1}
        onClick={() => {
          switch (layoutItem.type) {
            case "SaleItem": {
              const existingIndex = transactionItems.findIndex(
                (sti) =>
                  sti.saleItemId === layoutItem.target.id &&
                  sti.modifications.length === 0,
              );
              if (existingIndex !== -1) {
                const prevAmount = transactionItems[existingIndex].amount;
                updateItem(existingIndex, { amount: prevAmount + 1 });
              } else {
                addItem({
                  amount: 1,
                  saleItemName: layoutItem.target.name,
                  saleItemId: layoutItem.target.id,
                  modifications: [],
                });
              }
              break;
            }
            case "Layout": {
              changeLayout(layoutItem.target.id);
              break;
            }
            case "Pipe": {
              throw Error("Not implemented");
            }
          }
        }}
      >
        <Typography
          fontWeight="bold"
          fontSize={25}
          sx={{ userSelect: "none" }}
          align="center"
        >
          {layoutItem?.target.name}
        </Typography>
      </Box>

      <Box position="absolute" bottom="0" left="5px">
        {layoutItem.type === "SaleItem" && <ShoppingBag fontSize="large" />}
        {layoutItem.type === "Layout" && <GridView fontSize="large" />}
        {layoutItem.type === "Pipe" && <WaterDrop fontSize="large" />}
      </Box>

      {layoutItem.type === "SaleItem" &&
        instanceOfLayoutItemModelLayoutSaleItemModel(layoutItem) && (
          <Box position="absolute" top="10px" left="10px">
            <Typography
              fontSize={16}
              sx={{ userSelect: "none" }}
              lineHeight={1.2}
            >
              Cena: {layoutItem.target.currentCost},-
              {layoutItem.target.amountInStore !== null && (
                <>
                  <br />
                  Ve skladu: {layoutItem.target.amountInStore} ks
                </>
              )}
              {amountInOrder !== 0 && (
                <>
                  <br />V objednávce: {amountInOrder} ks
                </>
              )}
            </Typography>
          </Box>
        )}
    </>
  );
};
