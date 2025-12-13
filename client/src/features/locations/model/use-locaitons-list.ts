import { locationsApi, locationsQueryOptions } from "@/entities/locations/api";
import { useQuery } from "@tanstack/react-query";

const PAGE_SIZE = 2;
export function useLocationsList({ page }: { page: number }) {
  const { data, isPending, error } = useQuery(
    locationsQueryOptions.getLocationsOptions({ page, pageSize: PAGE_SIZE })
  );

  return {
    locations: data?.items,
    totalPages: data?.totalPages,
    totalCount: data?.totalCount,
    isPending,
    error,
  };
}
