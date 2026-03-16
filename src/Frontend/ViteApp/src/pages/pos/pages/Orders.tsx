import { Box, Paper } from "@mui/material";
import usePosStore from "../../../stores/posStore";

const Orders = () => {
  return (
    <Box
      display="grid"
      gridTemplateColumns="repeat(4, 1fr)"
      gridTemplateRows="repeat(4, 1fr)"
      height="100%"
      gap={1}
      padding={1}
    >
      {Array.apply(null, Array(16)).map((_, i) => (
        <Paper key={i} elevation={4}>
          Test
        </Paper>
      ))}
    </Box>
  );
};

export default Orders;
