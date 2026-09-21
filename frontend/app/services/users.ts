import type AddUserDto from "~/types/users/addUserDto";
import type { User } from "~/types/users/user";
import { getAccessToken } from "./localStorage/auth";

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
  const accessToken: string | null = getAccessToken();

  const options: RequestInit = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
  };

  try {
    const response = await fetch(url, options);

    if (response.status === 404) {
      return "User with the specified ID does not exist.";
    }

    if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

    const user: User = await response.json();
    return user;
  } catch (error) {
    return "An error occurred. Please try again later.";
  }
}
