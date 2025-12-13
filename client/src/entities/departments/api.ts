import { Department } from "./type";
import { apiClient } from "@/shared/api/axios-instance";

export type CreateDepartmentRequest = {
  name: string;
  identifier: string;
  parentId: string;
  locationsId: [string];
};
export const departmentsApi = {
  getDepartments: async () => {
    const response = await apiClient.get<Department[]>("/departments");
    return response.data || [];
  },

  createDepartment: async (request: CreateDepartmentRequest) => {
    const response = await apiClient.post("/departments", request);
    return response.data;
  },
};
