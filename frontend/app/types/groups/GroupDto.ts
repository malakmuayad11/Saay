import type { MemberDto } from "./MemberDto";

export interface GroupDto {
  groupId: number;
  isRequestedUserOwner: boolean;
  groupName: string;
  groupDescription: string | null;
  membersCount: number;
  members: MemberDto[];
}
