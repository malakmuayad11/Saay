import type { TaskDto } from "~/types/tasks/TaskDto";
import { apiFetch } from "./main";

const Base_URL: string = "https://saay.runasp.net";

export async function getUserTasksToday(
  userId: number,
  pageNumber: number = 1,
  pageSize: number = 10,
): Promise<TaskDto[] | string> {
  const url: URL = new URL(
    `api/saay/tasks/today/${userId}/${pageNumber}/${pageSize}`,
    Base_URL,
  );

  const options: RequestInit = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (response.status === 404) {
    return "User not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as TaskDto[];
}

export async function getUserTasksTomorrow(
  userId: number,
  pageNumber: number = 1,
  pageSize: number = 10,
): Promise<TaskDto[] | string> {
  const url: URL = new URL(
    `api/saay/tasks/tomorrow/${userId}/${pageNumber}/${pageSize}`,
    Base_URL,
  );

  const options: RequestInit = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (response.status === 404) {
    return "User not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as TaskDto[];
}

export async function getUserTasksForWeek(
  userId: number,
  pageNumber: number = 1,
  pageSize: number = 10,
): Promise<TaskDto[] | string> {
  const url: URL = new URL(
    `api/saay/tasks/week/${userId}/${pageNumber}/${pageSize}`,
    Base_URL,
  );

  const options: RequestInit = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (response.status === 404) {
    return "User not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as TaskDto[];
}
