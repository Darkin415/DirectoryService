import axios from "axios";

export const apiClient = axios.create({
  baseURL: "http://localhost:5084/api",
  headers: { "Content-Type": "application/json" },
});
