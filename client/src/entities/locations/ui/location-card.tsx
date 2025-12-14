"use client";

import { MapPin, Globe, Building2, DoorOpen, Calendar, Navigation } from "lucide-react";
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
  CardFooter,
} from "@/shared/components/ui/card";
import { Badge } from "@/shared/components/ui/badge";
import type { Location } from "../type";

type LocationCardProps = {
  location: Location;
};

export function LocationCard({ location }: LocationCardProps) {
  const formatDate = (date: Date) => {
    return new Date(date).toLocaleDateString("ru-RU", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  const fullAddress = `${location.street}, ${location.building}${
    location.roomNumber ? `, офис ${location.roomNumber}` : ""
  }`;

  return (
    <Card className="hover:shadow-md transition-shadow">
      <CardHeader>
        <div className="flex items-start justify-between">
          <div className="flex items-center gap-3">
            <div className="p-2 rounded-lg bg-primary/10">
              <MapPin className="h-5 w-5 text-primary" />
            </div>
            <div>
              <CardTitle className="text-lg">{location.name}</CardTitle>
              <CardDescription className="mt-1">
                {location.city}, {location.country}
              </CardDescription>
            </div>
          </div>
          <Badge variant={location.isActive ? "default" : "secondary"}>
            {location.isActive ? "Активна" : "Неактивна"}
          </Badge>
        </div>
      </CardHeader>
      <CardContent>
        <div className="space-y-3">
          <div className="flex items-center gap-2 text-sm text-muted-foreground">
            <Globe className="h-4 w-4" />
            <span>
              {location.country}, {location.city}
            </span>
          </div>
          <div className="flex items-center gap-2 text-sm text-muted-foreground">
            <Navigation className="h-4 w-4" />
            <span>{location.street}</span>
          </div>
          <div className="flex items-center gap-2 text-sm text-muted-foreground">
            <Building2 className="h-4 w-4" />
            <span>Здание: {location.building}</span>
          </div>
          {location.roomNumber && (
            <div className="flex items-center gap-2 text-sm text-muted-foreground">
              <DoorOpen className="h-4 w-4" />
              <span>Офис: {location.roomNumber}</span>
            </div>
          )}
          <div className="pt-2 border-t">
            <p className="text-sm font-medium">Полный адрес:</p>
            <p className="text-sm text-muted-foreground mt-1">{fullAddress}</p>
          </div>
        </div>
      </CardContent>
      <CardFooter className="flex items-center justify-between text-xs text-muted-foreground border-t pt-4">
        <div className="flex items-center gap-1">
          <Calendar className="h-3 w-3" />
          <span>Создана: {formatDate(location.createdAt)}</span>
        </div>
        {location.updatedAt && (
          <div className="flex items-center gap-1">
            <Calendar className="h-3 w-3" />
            <span>Обновлена: {formatDate(location.updatedAt)}</span>
          </div>
        )}
      </CardFooter>
    </Card>
  );
}




