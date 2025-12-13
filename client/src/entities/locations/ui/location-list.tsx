"use client";

import type { Location } from "../type";

type LocationListProps = {
  locations: Location[];
};

export function LocationList({ locations }: LocationListProps) {
  if (locations.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12 text-center">
        <p className="text-muted-foreground text-lg">Локации не найдены</p>
        <p className="text-muted-foreground text-sm mt-2">
          Добавьте первую локацию, чтобы начать работу
        </p>
      </div>
    );
  }
}

