export interface UpdateUserDto {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  profilePictureURL: string | null;
}
