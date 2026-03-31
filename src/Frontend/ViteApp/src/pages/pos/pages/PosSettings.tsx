import { useShallow } from "zustand/react/shallow";
import { usePosStore } from "../../../stores/posStore";
import { Controller, useForm, type SubmitHandler } from "react-hook-form";
import z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Box, Typography, TextField, Button } from "@mui/material";
import CashBoxPicker from "../../../components/pickers/CashBoxPicker";
import StorePicker from "../../../components/pickers/StorePicker";

const ValidationSchema = z.object({
  store: z
    .object(
      {
        id: z.number(),
        name: z.string(),
      },
      "Vyberte platný sklad",
    )
    .refine((val) => val?.id > 0, "Vyberte platný sklad"),
  cashBox: z
    .object(
      {
        id: z.number(),
        name: z.string(),
      },
      "Vyberte platnout kasu",
    )
    .refine((val) => val?.id > 0, "Vyberte platnou kasu"),
  readerUri: z.union([z.url("Zadejte platné URL"), z.literal("")]),
});

type FormType = z.infer<typeof ValidationSchema>;

const PosSettings = () => {
  const { store, cashBox, readerUri, updateMetadata } = usePosStore(
    useShallow((state) => ({
      store: state.currentStore,
      cashBox: state.currentCashBox,
      readerUri: state.readerUri,
      updateMetadata: state.updateMetadata,
    })),
  );
  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormType>({
    defaultValues: {
      store,
      cashBox,
      readerUri,
    },
    resolver: zodResolver(ValidationSchema),
  });

  const setValues: SubmitHandler<FormType> = (data) => {
    updateMetadata(data.cashBox, data.store, data.readerUri);
  };

  return (
    <Box padding={1}>
      <Typography variant="h5" component="h2" marginBottom={3}>
        POS nastavení
      </Typography>
      <form onSubmit={handleSubmit(setValues)}>
        <Box display="flex" flexDirection="column" gap={2}>
          <Controller
            control={control}
            name="store"
            render={({ field }) => (
              <StorePicker
                initialValue={field.value?.id}
                onChange={(val) => field.onChange(val)}
                error={!!errors.store}
                helperText={errors.store?.message}
              />
            )}
          />

          <Controller
            control={control}
            name="cashBox"
            render={({ field }) => (
              <CashBoxPicker
                initialValue={field.value?.id}
                onChange={(val) => field.onChange(val)}
                error={!!errors.cashBox}
                helperText={errors.cashBox?.message}
              />
            )}
          />

          <TextField
            label="URI čtečky"
            {...register("readerUri")}
            error={!!errors.readerUri}
            helperText={errors.readerUri?.message}
          />

          <Button type="submit">Uložit nastavení</Button>
        </Box>
      </form>
    </Box>
  );
};

export default PosSettings;
