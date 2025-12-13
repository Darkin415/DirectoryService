"use client";

import { Button } from "@/shared/components/ui/button";
import { Plus } from "lucide-react";
import { locationsApi } from "@/entities/locations/api";
import { LocationCard } from "@/entities/locations/ui/location-card";
import { Spinner } from "@/shared/components/ui/spinner";
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/shared/components/ui/pagination";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";

const PAGE_SIZE = 2;

export default function LocationPage() {
  const [page, setPage] = useState(1);
  const queryClient = useQueryClient();

  const { data, isLoading, error } = useQuery({
    queryFn: () => locationsApi.getLocations({ page, pageSize: PAGE_SIZE }),
    queryKey: ["locations", page],
  });

  const {
    mutate: createLocation,
    isPending,
    error: mutationError,
  } = useMutation({
    mutationFn: () =>
      locationsApi.createLocations({
        name: "Кропыска",
        timeZone: "Europe/Moscow",
        address: {
          country: "Россия",
          city: "Москва",
          street: "Мкр-4",
          building: "д-12",
          roomNumber: 35,
        },
      }),
    onSettled: () => queryClient.invalidateQueries({ queryKey: ["locations"] }),
  });

  if (isLoading) {
    return <Spinner />;
  }

  if (mutationError) {
    return <div>Ошибка : {mutationError.message}</div>;
  }

  if (error) {
    return <div>Ошибка : {error.message}</div>;
  }

  if (!data) {
    return <div>Нет данных</div>;
  }

  const { items, totalPages, page: currentPage, totalCount } = data;

  const generatePageNumbers = () => {
    const pages: (number | "ellipsis")[] = [];
    const maxVisible = 5;

    if (totalPages <= maxVisible) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      if (currentPage <= 3) {
        for (let i = 1; i <= 4; i++) {
          pages.push(i);
        }
        pages.push("ellipsis");
        pages.push(totalPages);
      } else if (currentPage >= totalPages - 2) {
        pages.push(1);
        pages.push("ellipsis");
        for (let i = totalPages - 3; i <= totalPages; i++) {
          pages.push(i);
        }
      } else {
        pages.push(1);
        pages.push("ellipsis");
        for (let i = currentPage - 1; i <= currentPage + 1; i++) {
          pages.push(i);
        }
        pages.push("ellipsis");
        pages.push(totalPages);
      }
    }

    return pages;
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Локации</h1>
          <p className="text-muted-foreground mt-1">
            Управление офисами и локациями компании
          </p>
          {data && (
            <p className="text-sm text-muted-foreground mt-1">
              Показано {items.length} из {totalCount} локаций
            </p>
          )}
        </div>
        <Button onClick={() => createLocation()} disabled={isPending}>
          <Plus className="h-4 w-4 mr-2" />
          Добавить локацию
        </Button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {items.map((location, index) => (
          <LocationCard
            key={`${location.name}-${location.city}-${index}`}
            location={location}
          />
        ))}
      </div>

      {totalPages > 1 && (
        <Pagination>
          <PaginationContent>
            <PaginationItem>
              <PaginationPrevious
                href="#"
                onClick={(e) => {
                  e.preventDefault();
                  if (currentPage > 1) {
                    setPage((prev) => prev - 1);
                  }
                }}
                className={
                  currentPage === 1
                    ? "pointer-events-none opacity-50"
                    : "cursor-pointer"
                }
              />
            </PaginationItem>

            {generatePageNumbers().map((pageNum, index) => (
              <PaginationItem key={index}>
                {pageNum === "ellipsis" ? (
                  <PaginationEllipsis />
                ) : (
                  <PaginationLink
                    href="#"
                    onClick={(e) => {
                      e.preventDefault();
                      setPage(pageNum);
                    }}
                    isActive={currentPage === pageNum}
                    className="cursor-pointer"
                  >
                    {pageNum}
                  </PaginationLink>
                )}
              </PaginationItem>
            ))}

            <PaginationItem>
              <PaginationNext
                href="#"
                onClick={(e) => {
                  e.preventDefault();
                  if (currentPage < totalPages) {
                    setPage((prev) => prev + 1);
                  }
                }}
                className={
                  currentPage === totalPages
                    ? "pointer-events-none opacity-50"
                    : "cursor-pointer"
                }
              />
            </PaginationItem>
          </PaginationContent>
        </Pagination>
      )}
    </div>
  );
}
