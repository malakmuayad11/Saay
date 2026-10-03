import type { HabitLogDto } from "~/types/habits/HabitLogDto";
import { apiFetch } from "./main";
import type { HabitDto } from "~/types/habits/HabitDto";
import type { AddHabitDto } from "~/types/habits/AddHabitDto";

const Base_URL: string = "https://saay.runasp.net";

export async function getUserHabits(
  userId: number,
  pageNumber: number = 1,
  pageSize: number = 10,
): Promise<HabitDto[] | string> {
  const url: URL = new URL(
    `api/saay/habits/${userId}/${pageNumber}/${pageSize}`,
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
    return "Habit not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as HabitDto[];
}

export async function getHabitLogs(
  habitId: number,
): Promise<HabitLogDto[] | string> {
  const url: URL = new URL(`api/saay/habits/habit-logs/${habitId}`, Base_URL);

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
    return "Habit not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as HabitLogDto[];
}

export async function markHabitAsCompleted(
  habitId: number,
  dayNumber: number,
): Promise<boolean | string> {
  const url: URL = new URL(
    `api/saay/habits/mark-completed/${habitId}/${dayNumber}`,
    Base_URL,
  );

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (response.status === 404) {
    return "Habit not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as boolean;
}

export async function addHabit(
  addHabitDto: AddHabitDto,
): Promise<HabitDto | string> {
  const url: URL = new URL("api/saay/habits", Base_URL);

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      userId: addHabitDto.userId,
      title: addHabitDto.title,
      reasonForHabit: addHabitDto.reasonForHabit,
      steps: addHabitDto.steps,
      targetDuration: addHabitDto.targetDuration,
    }),
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

  return (await response.json()) as HabitDto;
}

export async function deleteHabit(habitId: number): Promise<boolean | string> {
  const url: URL = new URL(`api/saay/habits/${habitId}`, Base_URL);

  const options: RequestInit = {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (response.status === 404) {
    return "Habit not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as boolean;
}
