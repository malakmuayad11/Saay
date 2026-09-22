import type { LoginResponseDto } from "~/types/auth/loginResponseDto";
import { removeAccessToken, setAccessToken } from "../sessionStorage/auth";
import { removeRefreshToken, setRefreshToken } from "../localStorage/auth";
import { removeCurrentUser } from "../localStorage/users";

const Base_URL = "https://saay.runasp.net/api/saay/auth/";

export async function login(
  email: string,
  password: string,
): Promise<LoginResponseDto | string> {
  const url = new URL("login", Base_URL);

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      email,
      password,
    }),
  };

  try {
    const response = await fetch(url, options);

    if (response.status === 401 || response.status === 404) {
      return "Invalid Credentials.";
    }

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return await response.json();
  } catch (error) {
    console.error("Login error:", error);
    return "An error occurred. Please try again later.";
  }
}

export async function refreshAccessToken(): Promise<
  { accessToken: string; refreshToken: string } | string
> {
  try {
    const refreshToken = localStorage.getItem("refreshToken");

    if (!refreshToken) {
      return "Session expired. Please sign in again.";
    }

    const response = await fetch(new URL("refresh", Base_URL), {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        refreshToken,
      }),
    });

    if (response.status === 401) {
      removeAccessToken();
      removeRefreshToken();
      removeCurrentUser();
      return "Session expired. Please sign in again.";
    }

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const data = await response.json();

    setAccessToken(data.accessToken);
    setRefreshToken(data.refreshToken);

    return data;
  } catch (error) {
    console.error("Refresh token error:", error);

    return "An error occurred. Please try again later.";
  }
}
