import type { DashboardNumbersDto } from "~/types/dashboard/DashboardNumbersDto";
import { apiFetch } from "./main";

const Base_URL: string = "https://saay.runasp.net";

export async function getDashboardNumbers(
  userId: number,
): Promise<DashboardNumbersDto | string> {
  const url: URL = new URL(`api/saay/dashboard/${userId}`, Base_URL);

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

  return (await response.json()) as DashboardNumbersDto;
}
