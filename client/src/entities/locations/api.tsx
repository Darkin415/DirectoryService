"use client";
import { apiClient } from "@/shared/api/axios-instance";
import { Location } from "./type";
import { PaginationResponse } from "@/shared/api/type";

export type PaginationRequest = {
  page: number;
  pageSize: number;
};

export type AddressDto = {
  country: string;
  city: string;
  street: string;
  building: string;
  roomNumber: number;
};

export type CreateLocationsRequest = {
  name: string;
  address: AddressDto;
  timeZone: string;
};

export type GetLocationWithPaginationRequest = {
  search?: string;
  country?: string;
  city?: string;
  locationsId?: string[];
  page: number;
  pageSize: number;
};

export type Envelope<T = unknown> = {
  result: T | null;
  error: ApiError | null;
  isError: boolean;
  timeGenerated: string;
};

export type ApiError = {
  messages: ErrorMessage[];
  type: ErrorType;
};

export type ErrorMessage = {
  code: string;
  message: string;
  invalidField?: string | null;
};

export type ErrorType =
  | "validation"
  | "not_found"
  | "failure"
  | "conflict"
  | "authentication"
  | "authorization";

export const locationsApi = {
  getLocations: async (
    request: GetLocationWithPaginationRequest
  ): Promise<PaginationResponse<Location> | null> => {
    const response = await apiClient
      .get<Envelope<PaginationResponse<Location>>>("/locations", {
        params: request,
      })
      .then((data) => data);

    return response.data.result;
  },

  createLocations: async (request: CreateLocationsRequest) => {
    const response = await apiClient.post("/locations", request);

    return response.data;
  },
};
