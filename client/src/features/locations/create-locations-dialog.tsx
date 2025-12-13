import { locationsApi } from "@/entities/locations/api";
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
import { useMutation, useQueryClient } from "@tanstack/react-query";

type Props = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

export function CreateLocationsDialog({ open, onOpenChange }: Props) {
  const queryClient = useQueryClient();
  const {
    mutate: createLocation,
    isPending: createIsPending,
    error: mutationError,
  } = useMutation({
    mutationFn: () => locationsApi.createLocations({}),
    onSettled: () => queryClient.invalidateQueries({ queryKey: ["locations"] }),
  });

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    console.log("create locations");
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px]">
        <DialogHeader>
          <DialogTitle>Создание локации</DialogTitle>
          <DialogDescription>
            Заполните форму для создания новой локации
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit}>
          <div className="space-y-6 py-4">
            {/* Основная информация */}
            <div className="space-y-4">
              <h3 className="text-sm font-medium">Основная информация</h3>

              <div className="space-y-2">
                <Label htmlFor="name">Название локации *</Label>
                <Input id="name" placeholder="Например: Офис в Москве" />
              </div>

              <div className="space-y-2">
                <Label htmlFor="timeZone">Часовой пояс *</Label>
                <Input id="timeZone" placeholder="Например: Europe/Moscow" />
              </div>
            </div>

            {/* Адрес */}
            <div className="space-y-4">
              <h3 className="text-sm font-medium">Адрес</h3>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="country">Страна *</Label>
                  <Input id="country" placeholder="Например: Россия" />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="city">Город *</Label>
                  <Input id="city" placeholder="Например: Москва" />
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="street">Улица *</Label>
                <Input id="street" placeholder="Например: Тверская улица" />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="building">Здание *</Label>
                  <Input id="building" placeholder="Например: 10 или д-12" />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="roomNumber">Номер комнаты *</Label>
                  <Input
                    id="roomNumber"
                    type="number"
                    placeholder="Например: 35"
                  />
                </div>
              </div>
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
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
