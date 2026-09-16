export default interface AddUserDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  profilePictureURL: string | null;
}
