import { locationsApi, locationsQueryOptions } from "@/entities/locations/api";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { toast } from "sonner";

export function useCreateLesson() {
  const queryClient = useQueryClient();
  const [timeZone, setTimeZone] = useState("");
  const mutation = useMutation({
    mutationFn: locationsApi.createLocations,
    onSettled: () =>
      queryClient.invalidateQueries({
        queryKey: [locationsQueryOptions.baseKey],
      }),
    onError: () => {
      toast.error("Ошибка при создании локации");
    },
    onSuccess: () => {
      toast.success("Урок успешно создан");
    },
  });

  return {
    createLocation: mutation.mutate,
    isError: mutation.isError,
    error: mutation.error,
    isPending: mutation.isPending,
  };
}
