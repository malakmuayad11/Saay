import { refreshAccessToken } from "./auth";

export async function apiFetch(
  input: RequestInfo | URL,
  options: RequestInit = {},
): Promise<Response | string> {
  try {
    let accessToken = sessionStorage.getItem("accessToken");

    const response = await fetch(input, {
      ...options,
      headers: {
        ...options.headers,
        Authorization: `Bearer ${accessToken}`,
      },
    });

    if (response.status !== 401) {
      return response;
    }

    // Access token expired, try to refresh it
    const refreshed = await refreshAccessToken();

    if (typeof refreshed === "string") {
      return refreshed;
    }

    accessToken = refreshed.accessToken;

    // Retry the original request with the new access token
    return await fetch(input, {
      ...options,
      headers: {
        ...options.headers,
        Authorization: `Bearer ${accessToken}`,
      },
    });
  } catch (error) {
    console.error("API request error:", error);
    return "An error occurred. Please try again later.";
  }
}
