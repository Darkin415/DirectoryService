"use client";

import { Button } from "@/shared/components/ui/button";
import { Plus } from "lucide-react";
import { locationsApi } from "@/entities/locations/api";
import { LocationCard } from "@/entities/locations/ui/location-card";
import { Spinner } from "@/shared/components/ui/spinner";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { CreateLocationsDialog } from "@/features/locations/create-locations-dialog";
import { LocationsPagination } from "@/features/locations/locations-pagination";
import { useLocationsList } from "@/features/locations/model/use-locaitons-list";

export default function LocationPage() {
  const [page, setPage] = useState(1);

  const [open, setOpen] = useState(false);

  const { locations, isPending, error, totalPages, totalCount } =
    useLocationsList({ page });

  if (isPending) {
    return <Spinner />;
  }

  if (error) {
    return <div>Ошибка : {error.message}</div>;
  }

  if (!locations) {
    return <div>Нет данных</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Локации</h1>
          <p className="text-muted-foreground mt-1">
            Управление офисами и локациями компании
          </p>
          {locations && (
            <p className="text-sm text-muted-foreground mt-1">
              Показано {locations.length} из {totalCount} локаций
            </p>
          )}
        </div>
        <Button onClick={() => setOpen(true)}>
          <Plus className="h-4 w-4 mr-2" />
          Добавить локацию
        </Button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {locations.map((location, index) => (
          <LocationCard
            key={`${location.name}-${location.city}-${index}`}
            location={location}
          />
        ))}
      </div>

      {totalPages && (
        <LocationsPagination
          currentPage={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      )}

      <CreateLocationsDialog open={open} onOpenChange={setOpen} />
    </div>
  );
}
