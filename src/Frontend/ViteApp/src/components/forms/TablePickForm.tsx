import { useShallow } from "zustand/react/shallow";
import { usePosStore } from "../../stores/posStore";
import { Box, Button, Typography } from "@mui/material";
import { useState } from "react";

type ToggleKey =
  | "Levý"
  | "Prostřední"
  | "Pravý"
  | "1"
  | "2"
  | "3"
  | "4"
  | "5"
  | "6";

const toggleOrder: Record<ToggleKey, number> = {
  Levý: 0,
  Prostřední: 1,
  Pravý: 2,
  "1": 3,
  "2": 4,
  "3": 5,
  "4": 6,
  "5": 7,
  "6": 8,
};

const topValues: ToggleKey[] = ["Levý", "Prostřední", "Pravý"];

const TablePickForm = ({ id }: { id: string }) => {
  const { setPickingTable, setOrderNote } = usePosStore(
    useShallow((state) => ({
      orderNote: state.orderNote,
      setPickingTable: state.setPickingTable,
      setOrderNote: state.setOrderNote,
    })),
  );
  const [toggles, setToggles] = useState<ToggleKey[]>([]);

  const displayedValue =
    toggles.length > 0
      ? "Stůl " +
        toggles.sort((a, b) => toggleOrder[a] - toggleOrder[b]).join(" ")
      : "Bez stolu";

  return (
    <form
      id={id}
      onSubmit={(evt) => {
        evt.preventDefault();

        setOrderNote(displayedValue);
        setPickingTable(false);
      }}
    >
      <Box
        display="flex"
        flexDirection="column"
        alignItems="stretch"
        gap={3}
        marginTop={1}
      >
        <Box display="grid" gridTemplateColumns="repeat(3, 1fr)" gap={2}>
          {topValues.map((val, i) => (
            <Button
              key={i}
              color={toggles.includes(val) ? "primary" : "inherit"}
              variant="contained"
              size="large"
              sx={{ fontSize: "20px" }}
              onClick={() =>
                setToggles((prev) =>
                  prev.includes(val)
                    ? prev.filter((v) => v != val)
                    : [...prev, val],
                )
              }
            >
              {val}
            </Button>
          ))}
        </Box>

        <Box display="grid" gridTemplateColumns="repeat(3, 1fr)" gap={2}>
          {Array.apply(null, Array(6)).map((_, i) => (
            <Button
              key={i}
              color={
                toggles.includes(String(i + 1) as ToggleKey)
                  ? "primary"
                  : "inherit"
              }
              variant="contained"
              size="large"
              sx={{ fontSize: "20px" }}
              onClick={() => {
                const val = String(i + 1) as ToggleKey;
                return setToggles((prev) =>
                  prev.includes(val)
                    ? prev.filter((v) => v != val)
                    : [...prev, val],
                );
              }}
            >
              {i + 1}
            </Button>
          ))}
        </Box>

        <Typography textAlign="center">
          Poznámka k vytištění: {displayedValue}
        </Typography>
      </Box>
    </form>
  );
};

export default TablePickForm;
