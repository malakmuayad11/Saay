import type { GroupDto } from "~/types/groups/GroupDto";
import { apiFetch } from "./main";
import type { AddGroupDto } from "~/types/groups/AddGroupDto";

const Base_URL: string = "https://saay.runasp.net";

export async function acceptGroupInvitation(
  token: string,
): Promise<boolean | string> {
  const url: URL = new URL("api/saay/groups/users", Base_URL);

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

export async function getUserGroups(
  userId: number,
): Promise<GroupDto[] | string> {
  const url: URL = new URL(`api/saay/groups/${userId}`, Base_URL);

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

  return (await response.json()) as GroupDto[];
}

export async function InviteUserToGroup(
  groupId: number,
  userEmail: string,
): Promise<GroupDto[] | string> {
  const url: URL = new URL(`api/saay/groups/invite`, Base_URL);

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      groupId,
      userEmail,
    }),
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (response.status === 404) {
    return "User with this email is not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as GroupDto[];
}

export async function deleteGroup(groupId: number): Promise<boolean | string> {
  const url: URL = new URL(`api/saay/groups/${groupId}`, Base_URL);

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
    return "Group not found.";
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as boolean;
}

export async function addGroup(
  addGroupDto: AddGroupDto,
): Promise<number | string> {
  const url: URL = new URL("api/saay/groups", Base_URL);

  const options: RequestInit = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(addGroupDto),
  };

  const response = await apiFetch(url, options);

  if (typeof response === "string") {
    return response;
  }

  if (!response.ok) {
    return "An error occurred. Please try again later.";
  }

  return (await response.json()) as number;
}
