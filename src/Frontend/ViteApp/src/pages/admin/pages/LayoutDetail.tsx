import z from "zod";
import validationConstants from "../../../constants/validationConstants";
import {
  Controller,
  useFieldArray,
  useForm,
  useWatch,
  type Control,
  type FieldErrors,
  type SubmitHandler,
  type UseFieldArrayReturn,
} from "react-hook-form";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import {
  Box,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Skeleton,
  TextField,
} from "@mui/material";
import { zodResolver } from "@hookform/resolvers/zod";
import { useLoading } from "../../../contexts/LoadingContext";
import { GridView, ShoppingBag, WaterDrop } from "@mui/icons-material";
import { usePosStore } from "../../../stores/posStore";
import SaleItemPicker from "../../../components/pickers/SaleItemPicker";
import LayoutPicker from "../../../components/pickers/LayoutPicker";
import TapPicker from "../../../components/pickers/TapPicker";
import type {
  LayoutItemModel,
  LayoutItemType,
  LayoutListModel,
  LayoutReadResponse,
  TapListModel,
} from "../../../api/apiTypes";
import { apiClient } from "../../../api/apiClient";
import handleApiError from "../../../errorHandling/apiResponseHandler";

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  image: z.string().nullish(),
  topLevel: z.boolean(),
  layoutItems: z.array(
    z.object({
      x: z
        .number()
        .refine((x) => x >= 1 && x <= 4, "Pozice položky je mimo rozsah"),
      y: z
        .number()
        .refine((x) => x >= 1 && x <= 4, "Pozice položky je mimo rozsah"),
      targetId: z
        .number("Vyberte platnou položku")
        .gt(0, "Vyberte platnou položku"),
      type: z.custom<LayoutItemType>(),
    }),
  ),
});

type LayoutUpdateFormData = z.infer<typeof ValidationSchema>;

const LayoutDetail = () => {
  const { id } = useParams();
  const { startLoading, stopLoading } = useLoading();
  const [layout, setLayout] = useState<LayoutReadResponse>();
  const [layouts, setLayouts] = useState<LayoutListModel[]>();
  const [taps, setTaps] = useState<TapListModel[]>();
  const setCurrentLayout = usePosStore((state) => state.setCurrentLayout);

  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<LayoutUpdateFormData>({
    values: !layout
      ? {
          name: "Rozložení",
          layoutItems: [],
          image: "",
          topLevel: false,
        }
      : {
          name: layout.name,
          image: layout.image,
          topLevel: layout.topLevel,
          layoutItems: layout.layoutItems.map((li) => ({
            x: li.x,
            y: li.y,
            type: li.type,
            targetId: li.target.id,
          })),
        },
    resolver: zodResolver(ValidationSchema),
  });

  const layoutItems = useFieldArray({
    control,
    name: "layoutItems",
  });

  useEffect(() => {
    const getLayout = async () => {
      const { response, data } = await apiClient.GET("/layouts/{id}", {
        params: { path: { id: Number(id) } },
      });

      setLayout(data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getLayout();
  }, []);

  useEffect(() => {
    if (taps) {
      return;
    }
    const getTaps = async () => {
      const { response, data } = await apiClient.GET("/taps");
      setTaps(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getTaps();
  }, []);

  useEffect(() => {
    if (layouts) {
      return;
    }
    const getLayouts = async () => {
      const { response, data } = await apiClient.GET("/layouts");
      setLayouts(data?.data);
      if (!response.ok) {
        handleApiError(response);
      }
    };
    getLayouts();
  }, []);

  const updateLayout: SubmitHandler<LayoutUpdateFormData> = async (
    requestBody,
  ) => {
    startLoading();
    const { data, response, error } = await apiClient.PUT("/layouts/{id}", {
      params: { path: { id: Number(id) } },
      body: requestBody,
    });
    if (data) {
      setLayout(data);
      setCurrentLayout(undefined);
    }
    if (!response.ok) {
      handleApiError(response, error);
    }
    stopLoading();
  };

  if (!layout || !taps || !layouts) {
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
      <h2>Detail rozložení</h2>

      <Box
        display="flex"
        gap={2}
        paddingBottom={2}
        flexDirection="column"
        alignItems="flex-start"
      >
        <form onSubmit={handleSubmit(updateLayout)}>
          <Box
            display="flex"
            flexDirection="column"
            alignItems="flex-start"
            gap={2}
          >
            <TextField
              fullWidth
              label="Název"
              {...register("name")}
              error={!!errors.name}
              helperText={errors?.name?.message}
            />

            <Controller
              control={control}
              name="topLevel"
              render={({ field }) => (
                <FormControlLabel
                  label="Výchozí rozložení"
                  control={
                    <Checkbox
                      {...field}
                      checked={!!field.value}
                      disabled={!!field.value}
                    />
                  }
                />
              )}
            />

            <Box
              sx={{
                aspectRatio: 1,
                flexGrow: 1,
              }}
              display="grid"
              gridTemplateColumns="repeat(4, 1fr)"
              gridTemplateRows="repeat(4, 1fr)"
              gap={1}
            >
              {Array.apply(null, Array(4)).flatMap((_, x) =>
                Array.apply(null, Array(4)).map((_, y) => (
                  <Paper key={`${x}${y}`} elevation={4}>
                    <Box
                      height="100%"
                      display="flex"
                      alignItems="center"
                      justifyContent="center"
                      padding={1}
                    >
                      <LayoutGridItem
                        parentLayoutId={layout.id}
                        control={control}
                        index={layoutItems.fields.findIndex(
                          (li) => li.x === x + 1 && li.y === y + 1,
                        )}
                        existingItem={layout.layoutItems.find(
                          (li) => li.x === x + 1 && li.y === y + 1,
                        )}
                        x={x + 1}
                        y={y + 1}
                        formLayoutItems={layoutItems}
                        errors={errors}
                        layouts={layouts}
                        taps={taps}
                      />
                    </Box>
                  </Paper>
                )),
              )}
            </Box>

            <Button type="submit" variant="contained">
              Uložit změny
            </Button>
          </Box>
        </form>
      </Box>
    </>
  );
};

const LayoutGridItem = ({
  control,
  parentLayoutId,
  index,
  formLayoutItems,
  existingItem,
  errors,
  layouts,
  taps,
  x,
  y,
}: {
  control: Control<LayoutUpdateFormData, any, LayoutUpdateFormData>;
  parentLayoutId: number;
  index: number;
  existingItem?: LayoutItemModel;
  formLayoutItems: UseFieldArrayReturn<
    LayoutUpdateFormData,
    "layoutItems",
    "id"
  >;
  errors: FieldErrors<LayoutUpdateFormData>;
  layouts: LayoutListModel[];
  taps: TapListModel[];
  x: number;
  y: number;
}) => {
  const type = useWatch({ control, name: `layoutItems.${index}.type` });
  const layoutItem = formLayoutItems.fields[index];

  if (layoutItem) {
    const labelId = `typeSelect${index}`;
    return (
      <Box
        display="flex"
        flexDirection="column"
        gap={1}
        width="100%"
        alignItems="center"
      >
        {type === "SaleItem" && <ShoppingBag />}
        {type === "Layout" && <GridView />}
        {type === "Tap" && <WaterDrop />}

        <FormControl fullWidth>
          <InputLabel size="small" id={labelId}>
            Typ položky
          </InputLabel>

          <Controller
            name={`layoutItems.${index}.type`}
            control={control}
            render={({ field }) => (
              <Select
                size="small"
                labelId={labelId}
                label="Typ položky"
                {...field}
              >
                <MenuItem value="SaleItem">Prodejní položka</MenuItem>
                <MenuItem value="Layout">Rozložení</MenuItem>
                <MenuItem value="Tap">Pípa</MenuItem>
              </Select>
            )}
          />
        </FormControl>

        {type === "SaleItem" && (
          <Controller
            name={`layoutItems.${index}.targetId`}
            control={control}
            render={({ field }) => (
              <SaleItemPicker
                onChange={field.onChange}
                error={!!errors.layoutItems?.[index]?.targetId}
                helperText={errors.layoutItems?.[index]?.targetId?.message}
                initialValue={existingItem?.target.name}
              />
            )}
          />
        )}

        {type === "Layout" && (
          <Controller
            name={`layoutItems.${index}.targetId`}
            control={control}
            render={({ field }) => (
              <LayoutPicker
                onChange={field.onChange}
                excludeId={parentLayoutId}
                error={!!errors.layoutItems?.[index]?.targetId}
                helperText={errors.layoutItems?.[index]?.targetId?.message}
                initialValue={existingItem?.target.name}
                options={layouts}
              />
            )}
          />
        )}

        {type === "Tap" && (
          <Controller
            name={`layoutItems.${index}.targetId`}
            control={control}
            render={({ field }) => (
              <TapPicker
                small
                onChange={(val) => field.onChange(val?.id)}
                error={!!errors.layoutItems?.[index]?.targetId}
                helperText={errors.layoutItems?.[index]?.targetId?.message}
                initialValue={existingItem?.target.id}
                options={taps}
              />
            )}
          />
        )}

        <Button
          variant="outlined"
          color="error"
          size="small"
          onClick={() => {
            formLayoutItems.remove(index);
          }}
        >
          Smazat
        </Button>
      </Box>
    );
  } else {
    return (
      <Button
        onClick={() => {
          formLayoutItems.append({
            targetId: 0,
            x,
            y,
            type: "SaleItem",
          });
        }}
      >
        Přidat položku
      </Button>
    );
  }
};

export default LayoutDetail;
