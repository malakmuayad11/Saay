import { apiFetch } from "./main";

const Base_URL: string = "https://saay.runasp.net";

export async function acceptGroupInvitation(
  token: string,
): Promise<boolean | string> {
  const url: URL = new URL("api/saay/groups", Base_URL);

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      token: token,
    }),
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (response.status === 404) {
    return "User not found.";
  }

  if (response.status === 500) {
    return "User is already a member of the group.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as boolean;
}
