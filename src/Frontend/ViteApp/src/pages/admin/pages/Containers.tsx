import { Box } from "@mui/material";
import ContainerListView from "../../../components/views/ContainerListView";

const Containers = () => {
  return (
    <>
      <h2>Kegy</h2>

      <Box
        display="flex"
        gap={2}
        flexDirection="column"
        alignItems="flex-start"
      >
        <ContainerListView showUnusableFilter showTemplateFilter />
      </Box>
    </>
  );
};

export default Containers;
