import type { Goal } from "~/types/goals/Goal";
import { apiFetch } from "./main";
import type { GoalCategoryDto } from "~/types/goals/GoalCategoryDto";
import type { UpdateGoalDto } from "~/types/goals/UpdateGoalDto";

const Base_URL: string = "https://saay.runasp.net";

export async function getCompletedGoalsCount(
  userId: number,
): Promise<number | string> {
  const url: URL = new URL(
    `api/saay/goals/completed/count/${userId}`,
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

  return (await response.json()) as number;
}

export async function getPendingGoalsCount(
  userId: number,
): Promise<number | string> {
  const url: URL = new URL(`api/saay/goals/pending/count/${userId}`, Base_URL);

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

  return (await response.json()) as number;
}

export async function getUserGoals(
  userId: number,
  pageNumber: number = 1,
  pageSize: number = 10,
): Promise<Goal[] | string> {
  const url: URL = new URL(
    `api/saay/goals/${userId}/${pageNumber}/${pageSize}`,
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

  return (await response.json()) as Goal[];
}

export async function addGoal(addGoalDto: AddGoalDto): Promise<Goal | string> {
  const url: URL = new URL("api/saay/goals", Base_URL);

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      userId: addGoalDto.userId,
      goalCategoryId: addGoalDto.goalCategoryId,
      title: addGoalDto.title,
      timeFrame: addGoalDto.timeFrame,
      deadline: addGoalDto.deadline,
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

  return (await response.json()) as Goal;
}

export async function updateGoal(
  updateGoalDto: UpdateGoalDto,
): Promise<Goal | string> {
  const url: URL = new URL("api/saay/goals", Base_URL);

  const options: RequestInit = {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      goalId: updateGoalDto.goalId,
      goalCategoryId: updateGoalDto.goalCategoryId,
      title: updateGoalDto.title,
      timeFrame: updateGoalDto.timeFrame,
      deadline: updateGoalDto.deadline,
      isDone: updateGoalDto.isDone,
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

  return (await response.json()) as Goal;
}

export async function getCategories(): Promise<GoalCategoryDto[] | string> {
  const url: URL = new URL(`api/saay/goals/categories`, Base_URL);

  const options: RequestInit = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  };

  try {
    const response = await fetch(url, options);

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return (await response.json()) as GoalCategoryDto[];
  } catch (error) {
    console.error("Login error:", error);
    return "An error occurred. Please try again later.";
  }
}

export async function getGoal(goalId: number): Promise<Goal | string> {
  const url: URL = new URL(`api/saay/goals/${goalId}`, Base_URL);

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

  return (await response.json()) as Goal;
}

export async function deleteGoal(goalId: number): Promise<boolean | string> {
  const url: URL = new URL(`api/saay/goals/${goalId}`, Base_URL);

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
    return "User not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as boolean;
}
