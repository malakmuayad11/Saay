import type AddUserDto from "~/types/users/addUserDto";
import type { User } from "~/types/users/user";
import { apiFetch } from "./main";

const Base_URL = "https://saay.runasp.net";

export async function addUser(user: AddUserDto): Promise<string | null> {
  const url: URL = new URL("api/saay/users", Base_URL);

  const options = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      firstName: user.firstName,
      lastName: user.lastName,
      email: user.email,
      password: user.password,
      profilePictureURL: user.profilePictureURL,
    }),
  };

  try {
    const response = await fetch(url, options);

    if (response.status === 400) {
      return "Email already registered! Sign In instead.";
    }

    if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

    await response.json();
    return null; // registeration successed
  } catch (error) {
    return "An error occurred. Please try again later.";
  }
}

export async function getUser(userId: number): Promise<User | string> {
  const url: URL = new URL(`api/saay/users/${userId}`, Base_URL);

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

  return (await response.json()) as User;
}

export async function getUserMission(userId: number): Promise<string | null> {
  const url: URL = new URL(`api/saay/users/mission/${userId}`, Base_URL);

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

  return (await response.json()) as string | null;
}
