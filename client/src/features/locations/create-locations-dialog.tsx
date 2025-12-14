import {
  AddressDto,
  locationsApi,
  locationsQueryOptions,
} from "@/entities/locations/api";
import { queryClient } from "@/shared/api/query-client";
import { Button } from "@/shared/components/ui/button";
import {
  DialogHeader,
  DialogFooter,
  Dialog,
  DialogContent,
  DialogTitle,
  DialogDescription,
} from "@/shared/components/ui/dialog";
import { Input } from "@/shared/components/ui/input";
import { Label } from "@/shared/components/ui/label";
import { FieldError } from "@/shared/components/ui/field";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { toast } from "sonner";
import { z } from "zod";
import { useCreateLesson } from "./model/use-create-location";
import { useForm, SubmitHandler } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

type Props = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

type CreateLocationData = {
  name: string;
  address: AddressDto;
  timeZone: string;
};

const initialData: CreateLocationData = {
  name: "",
  address: {
    country: "",
    city: "",
    street: "",
    building: "",
    roomNumber: 0,
  },
  timeZone: "",
};

// Схема валидации Zod
const addressSchema = z.object({
  country: z.string().min(1, "Страна обязательна для заполнения"),
  city: z.string().min(1, "Город обязателен для заполнения"),
  street: z.string().min(1, "Улица обязательна для заполнения"),
  building: z.string().min(1, "Здание обязательно для заполнения"),
  roomNumber: z.number().min(1, "Номер комнаты должен быть больше 0"),
});

const createLocationSchema = z.object({
  name: z.string().min(1, "Название локации обязательно для заполнения"),
  timeZone: z
    .string()
    .min(1, "Часовой пояс обязателен для заполнения")
    .regex(
      /^[A-Za-z_]+\/[A-Za-z_]+$/,
      "Неверный формат часового пояса (например: Europe/Moscow)"
    ),
  address: addressSchema,
});

export function CreateLocationsDialog({ open, onOpenChange }: Props) {
  const {
    register,
    handleSubmit,
    formState: { errors, isValid },
    reset,
  } = useForm<CreateLocationData>({
    defaultValues: initialData,
    resolver: zodResolver(createLocationSchema),
  });

  const { createLocation, isPending, error } = useCreateLesson();

  const onSubmit = (data: CreateLocationData) => {
    createLocation(data, {
      onSuccess: () => {
        onOpenChange(false);
      },
    });
  };

  const handleOpenChange = (newOpen: boolean) => {
    if (!newOpen) {
    }
    onOpenChange(newOpen);
  };

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent className="sm:max-w-[600px]">
        <DialogHeader>
          <DialogTitle>Создание локации</DialogTitle>
          <DialogDescription>
            Заполните форму для создания новой локации
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="space-y-6 py-4">
            {/* Основная информация */}
            <div className="space-y-4">
              <h3 className="text-sm font-medium">Основная информация</h3>

              <div className="space-y-2">
                <Label htmlFor="name">Название локации *</Label>
                <Input
                  id="name"
                  placeholder="Например: Офис в Москве"
                  {...register("name")}
                />
                {errors.name && (
                  <p className="text-sm text-destructive">
                    {errors.name.message}
                  </p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="timeZone">Часовой пояс *</Label>
                <Input
                  id="timeZone"
                  placeholder="Например: Europe/Moscow"
                  {...register("timeZone")}
                />
                {errors.timeZone && (
                  <p className="text-sm text-destructive">
                    {errors.timeZone.message}
                  </p>
                )}
              </div>
            </div>

            {/* Адрес */}
            <div className="space-y-4">
              <h3 className="text-sm font-medium">Адрес</h3>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="country">Страна *</Label>
                  <Input
                    id="country"
                    placeholder="Например: Россия"
                    {...register("address.country")}
                  />
                  {errors.address?.country && (
                    <p className="text-sm text-destructive">
                      {errors.address?.country.message}
                    </p>
                  )}
                </div>

                <div className="space-y-2">
                  <Label htmlFor="city">Город *</Label>
                  <Input
                    id="city"
                    placeholder="Например: Москва"
                    {...register("address.city")}
                  />
                </div>
                {errors.address?.city && (
                  <p className="text-sm text-destructive">
                    {errors.address?.city.message}
                  </p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="street">Улица *</Label>
                <Input
                  id="street"
                  placeholder="Например: Тверская улица"
                  {...register("address.street")}
                />
                {errors.address?.street && (
                  <p className="text-sm text-destructive">
                    {errors.address?.street.message}
                  </p>
                )}
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="building">Здание *</Label>
                  <Input
                    id="building"
                    placeholder="Например: 10 или д-12"
                    {...register("address.building")}
                  />
                  {errors.address?.building && (
                    <p className="text-sm text-destructive">
                      {errors.address?.building.message}
                    </p>
                  )}
                </div>

                <div className="space-y-2">
                  <Label htmlFor="roomNumber">Номер комнаты *</Label>
                  <Input
                    id="roomNumber"
                    type="number"
                    placeholder="Например: 35"
                    {...register("address.roomNumber")}
                  />
                  {errors.address?.roomNumber && (
                    <p className="text-sm text-destructive">
                      {errors.address?.roomNumber.message}
                    </p>
                  )}
                </div>
              </div>
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              disabled={isPending || !isValid}
              onClick={() => onOpenChange(false)}
            >
              Отмена
            </Button>
            <Button type="submit">Создать локацию</Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
