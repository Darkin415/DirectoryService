"use client";

import { DepartmentCard } from "./department-card";
import type { Department } from "../type";

type DepartmentListProps = {
  departments: Department[];
};

export function DepartmentList({ departments }: DepartmentListProps) {
  if (departments.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12 text-center">
        <p className="text-muted-foreground text-lg">Департаменты не найдены</p>
        <p className="text-muted-foreground text-sm mt-2">
          Добавьте первый департамент, чтобы начать работу
        </p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      {departments.map((department) => (
        <DepartmentCard key={department.id} department={department} />
      ))}
    </div>
  );
}



