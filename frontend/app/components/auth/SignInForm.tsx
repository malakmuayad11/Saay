import Label from "~/components/form/Label";
import Input from "~/components/form/input/InputField";
import Button from "../ui/button/Button";
import { EyeCloseIcon, EyeIcon } from "~/assets/icons";
import { useState } from "react";
import { Link, useNavigate } from "react-router";
import { login } from "~/services/auth";
import Alert from "../ui/Alert";
import { setCurrentUser } from "~/services/localStorage";

export default function SignInForm() {
  const [showPassword, setShowPassword] = useState(false);
  const [email, setEmail] = useState<string>("");
  const [emailValid, setEmailValid] = useState<boolean>(true);

  const [password, setPassword] = useState<string>("");
  const [passwordValid, setPasswordValid] = useState<boolean>(true);

  const [error, setError] = useState<string | null>(null);
  const [signingIn, setSigningIn] = useState<boolean>(false);

  const navigator = useNavigate();

  function validateFields() {
    return password !== "" && email !== "";
  }

  async function handleSignIn(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    if (!validateFields()) {
      setEmailValid(email !== "");
      setPasswordValid(password !== "");
      setSigningIn(false);
      return;
    }

    setError(null);
    setSigningIn(true);

    const result: string | number = await login(email, password);

    if (typeof result === "string") {
      setError(result);
      setSigningIn(false);
      return;
    }

    setEmail("");
    setPassword("");

    setEmailValid(true);
    setPasswordValid(true);

    setError(null);
    setSigningIn(false);

    setCurrentUser(result);

    navigator("/dashboard");
  }

  return (
    <div className="flex min-h-screen w-full items-center justify-center overflow-y-auto px-4 py-8 sm:px-6">
      <div className="w-full max-w-md">
        <div>
          <div className="mb-5 sm:mb-8">
            <h1 className="mb-2 mt-4 text-title-sm font-semibold text-gray-800 sm:text-title-md dark:text-white/90">
              Sign In
            </h1>
            <p className="text-sm text-gray-500 dark:text-gray-400">
              Enter your email and password to sign in!
            </p>
          </div>
          <div>
            {error && <Alert variant="error" title="Error" message={error} />}
            <form onSubmit={handleSignIn}>
              <div className="space-y-6">
                <div>
                  <Label htmlFor="email">
                    Email <span className="text-error-500">*</span>{" "}
                  </Label>
                  <Input
                    id="email"
                    placeholder="info@gmail.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    hint={!emailValid ? "Email is required" : undefined}
                    error={!emailValid}
                    onBlur={() => setEmailValid(email !== "")}
                  />
                </div>
                <div>
                  <Label htmlFor="password">
                    Password <span className="text-error-500">*</span>{" "}
                  </Label>
                  <div className="relative">
                    <Input
                      type={showPassword ? "text" : "password"}
                      placeholder="Enter your password"
                      id="password"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      hint={!passwordValid ? "Password is required" : undefined}
                      error={!passwordValid}
                      onBlur={() => setPasswordValid(password !== "")}
                    />
                    <span
                      onClick={() => setShowPassword(!showPassword)}
                      className="absolute inset-e-4 top-1/2 z-30 -translate-y-1/2 cursor-pointer"
                    >
                      {showPassword ? (
                        <EyeIcon
                          className={`size-5 fill-gray-500 dark:fill-gray-400 ${!passwordValid && "-translate-y-1/2"}`}
                        />
                      ) : (
                        <EyeCloseIcon
                          className={`size-5 fill-gray-500 dark:fill-gray-400 ${!passwordValid && "-translate-y-1/2"}`}
                        />
                      )}
                    </span>
                  </div>
                </div>
                <div className="flex items-center justify-between">
                  <Link
                    to="/reset-password"
                    className="text-sm text-brand-500 hover:text-brand-600 dark:text-brand-400"
                  >
                    Forgot password?
                  </Link>
                </div>
                <div>
                  <Button className="w-full" size="sm" disabled={signingIn}>
                    {signingIn ? "Signing in..." : "Sign in"}
                  </Button>
                </div>
              </div>
            </form>

            <div className="mt-5">
              <p className="text-center text-sm font-normal text-gray-700 sm:text-start dark:text-gray-400">
                Don&apos;t have an account? {""}
                <Link
                  to="/"
                  className="text-brand-500 hover:text-brand-600 dark:text-brand-400"
                >
                  Sign Up
                </Link>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
