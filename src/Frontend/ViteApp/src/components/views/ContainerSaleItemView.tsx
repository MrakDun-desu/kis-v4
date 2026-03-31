import { ShoppingBag } from "@mui/icons-material";
import { Box, Typography } from "@mui/material";
import { type SaleItemContainerModel } from "../../api-generated";
import { usePosStore } from "../../stores/posStore";
import { useShallow } from "zustand/react/shallow";

const ContainerSaleItemView = ({
  saleItem,
}: {
  saleItem: SaleItemContainerModel;
}) => {
  const { transactionItems, addItem, updateItem } = usePosStore(
    useShallow((state) => ({
      transactionItems: state.transactionItems,
      addItem: state.addTransactionItem,
      updateItem: state.updateTransactionItem,
    })),
  );

  const amountInOrder = transactionItems.reduce((acc, curr) => {
    if (curr.saleItemId === saleItem.id) {
      return acc + curr.amount;
    }
    return acc;
  }, 0);

  return (
    <Box
      display="flex"
      justifyContent="center"
      alignItems="center"
      height="100%"
      padding={1}
      onClick={() => {
        const existingIndex = transactionItems.findIndex(
          (sti) =>
            sti.saleItemId === saleItem.id && sti.modifications.length === 0,
        );
        if (existingIndex !== -1) {
          const prevAmount = transactionItems[existingIndex].amount;
          updateItem(existingIndex, { amount: prevAmount + 1 });
        } else {
          addItem({
            amount: 1,
            saleItemName: saleItem.name,
            saleItemId: saleItem.id,
            modifications: [],
          });
        }
      }}
    >
      <Typography
        fontWeight="bold"
        fontSize={25}
        sx={{ userSelect: "none" }}
        align="center"
      >
        {saleItem.name}
      </Typography>

      <Box position="absolute" bottom="0" left="5px">
        <ShoppingBag fontSize="large" />
      </Box>

      <Box position="absolute" top="10px" left="10px">
        <Typography fontSize={16} sx={{ userSelect: "none" }} lineHeight={1.2}>
          Cena: {saleItem.currentCost},-
          <br />
          Ve skladu: {saleItem.amountInStore} ks
          <br />V kegu: {saleItem.amountInContainer} ks
          {amountInOrder !== 0 && (
            <>
              <br />V objednávce: {amountInOrder} ks
            </>
          )}
        </Typography>
      </Box>
    </Box>
  );
};

export default ContainerSaleItemView;
