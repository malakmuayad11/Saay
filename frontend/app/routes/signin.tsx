import { redirect } from "react-router";
import SignInForm from "~/components/auth/SignInForm";
import { getCurrentUser } from "~/services/localStorage/users";

export const meta = () => [{ title: "Sign In | Saay" }];

export async function clientLoader() {
  const currentUserId = getCurrentUser();

  if (currentUserId) throw redirect("/dashboard");

  return null;
}

export default function SignIn() {
  return <SignInForm />;
}
