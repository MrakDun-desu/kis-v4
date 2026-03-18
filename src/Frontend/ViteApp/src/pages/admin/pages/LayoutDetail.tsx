import z from "zod";
import {
  type LayoutItemType,
  type LayoutCreateRequestModel,
  type LayoutUpdateRequestModel,
  type LayoutReadResponse,
  LayoutsApi,
} from "../../../api-generated";
import validationConstants from "../../../constants/validationConstants";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import handleApiCall from "../../../errorHandling/apiResponseHandler";
import { useParams } from "react-router-dom";
import { defaultConfiguration } from "../../../configuration/apiConfiguration";

const api = new LayoutsApi(defaultConfiguration);

const ValidationSchema = z.object({
  name: z
    .string()
    .min(1, "Jméno nesmí být prázdné")
    .max(validationConstants.maxNameLength, "Jméno přesahuje maximální délku"),
  image: z.string().nullish(),
  topLevel: z.boolean().optional(),
  layoutItems: z.array(
    z.object({
      x: z
        .number()
        .refine((x) => x >= 1 && x <= 4, "Pozice položky je mimo rozsah"),
      y: z
        .number()
        .refine((x) => x >= 1 && x <= 4, "Pozice položky je mimo rozsah"),
      targetId: z.number(),
      type: z.custom<LayoutItemType>(),
    }),
  ),
});

const defaultValue: LayoutUpdateRequestModel = {
  name: "Nové rozložení",
  layoutItems: [],
  topLevel: false,
};

const LayoutDetail = () => {
  const [layout, setLayout] = useState<LayoutReadResponse | null>(null);
  const [refreshCounter, setRefreshCounter] = useState(0);
  const { id } = useParams();

  useEffect(() => {
    const getStore = async () => {
      const response = await handleApiCall(
        api.layoutsRead({
          id: Number(id),
        }),
      );
      if (response) {
      } else {
      }
    };
    getStore();
  }, [refreshCounter]);

  const refreshLayout = () => setRefreshCounter((val) => val + 1);
};

export default LayoutDetail;
