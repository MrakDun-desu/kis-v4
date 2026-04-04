import { ShoppingBag, GridView, WaterDrop } from "@mui/icons-material";
import { Box, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { usePosStore } from "../../stores/posStore";
import { useShallow } from "zustand/react/shallow";

const LayoutItemView = ({ x, y }: { x: number; y: number }) => {
  const { currentLayout, transactionItems, changeLayout, addItem, updateItem } =
    usePosStore(
      useShallow((state) => ({
        currentLayout: state.currentLayout,
        currentStore: state.currentStore,
        transactionItems: state.transactionItems,
        changeLayout: state.setLayoutId,
        addItem: state.addTransactionItem,
        updateItem: state.updateTransactionItem,
      })),
    );
  const navigate = useNavigate();

  if (!currentLayout) {
    return;
  }

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
          case "Tap": {
            if (layoutItem.target.store.id !== currentStore)
              navigate(`/pos/taps/${layoutItem.target.id}`);
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

      <Box position="absolute" bottom="0" left="5px">
        {layoutItem.type === "SaleItem" && <ShoppingBag fontSize="large" />}
        {layoutItem.type === "Layout" && <GridView fontSize="large" />}
        {layoutItem.type === "Tap" && <WaterDrop fontSize="large" />}
      </Box>

      {layoutItem.type === "SaleItem" && (
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
    </Box>
  );
};

export default LayoutItemView;
