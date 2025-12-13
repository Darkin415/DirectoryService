"use client";

import { Building2, Users, Hash, FolderTree, Calendar } from "lucide-react";
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
  CardFooter,
} from "@/shared/components/ui/card";
import { Badge } from "@/shared/components/ui/badge";
import type { Department } from "../type";

type DepartmentCardProps = {
  department: Department;
};

export function DepartmentCard({ department }: DepartmentCardProps) {
  const formatDate = (date: Date) => {
    return new Date(date).toLocaleDateString("ru-RU", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  return (
    <Card className="hover:shadow-md transition-shadow">
      <CardHeader>
        <div className="flex items-start justify-between">
          <div className="flex items-center gap-3">
            <div className="p-2 rounded-lg bg-primary/10">
              <Building2 className="h-5 w-5 text-primary" />
            </div>
            <div>
              <CardTitle className="text-lg">{department.name}</CardTitle>
              <CardDescription className="mt-1">
                {department.path || department.identifier}
              </CardDescription>
            </div>
          </div>
          <Badge variant={department.isActive ? "default" : "secondary"}>
            {department.isActive ? "Активен" : "Неактивен"}
          </Badge>
        </div>
      </CardHeader>
      <CardContent>
        <div className="space-y-3">
          <div className="flex items-center gap-2 text-sm text-muted-foreground">
            <Hash className="h-4 w-4" />
            <span>ID: {department.identifier}</span>
          </div>
          <div className="flex items-center gap-2 text-sm text-muted-foreground">
            <FolderTree className="h-4 w-4" />
            <span>Уровень: {department.depth}</span>
          </div>
          {department.childCount > 0 && (
            <div className="flex items-center gap-2 text-sm text-muted-foreground">
              <Users className="h-4 w-4" />
              <span>Подразделений: {department.childCount}</span>
            </div>
          )}
          {department.parentId && (
            <div className="text-sm text-muted-foreground">
              <span>Родительский ID: {department.parentId}</span>
            </div>
          )}
        </div>
      </CardContent>
      <CardFooter className="flex items-center justify-between text-xs text-muted-foreground border-t pt-4">
        <div className="flex items-center gap-1">
          <Calendar className="h-3 w-3" />
          <span>Создан: {formatDate(department.createdAt)}</span>
        </div>
        {department.updatedAt && (
          <div className="flex items-center gap-1">
            <Calendar className="h-3 w-3" />
            <span>Обновлен: {formatDate(department.updatedAt)}</span>
          </div>
        )}
      </CardFooter>
    </Card>
  );
}


