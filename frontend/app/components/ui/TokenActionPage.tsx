import { useState } from "react";
import { Link, useSearchParams } from "react-router";
import { t } from "i18next";
import Alert from "~/components/ui/Alert";
import { confirm } from "~/services/api/auth";
import { acceptGroupInvitation } from "~/services/api/groups";

type TokenActionType = "emailConfirmation" | "groupInvitation";

interface TokenActionPageProps {
  type: TokenActionType;
}

const translations = {
  emailConfirmation: "confirmEmail",
  groupInvitation: "acceptInvitation",
} as const;

export default function TokenActionPage({ type }: TokenActionPageProps) {
  const [searchParams] = useSearchParams();
  const [isConfirmed, setIsConfirmed] = useState<boolean | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const translationKey = translations[type];
  const isInvitation = type === "groupInvitation";

  async function handleAction() {
    setError(null);

    const token = searchParams.get("token");

    if (!token) {
      setIsConfirmed(false);
      setError(t(`${translationKey}.errors.missingToken`));
      return;
    }

    try {
      setIsLoading(true);

      const result = isInvitation
        ? await acceptGroupInvitation(token)
        : await confirm(token);

      if (typeof result === "string") {
        setIsConfirmed(false);
        setError(result);
        return;
      }

      setIsConfirmed(true);
    } catch {
      setIsConfirmed(false);
      setError(t(`${translationKey}.errors.unexpected`));
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <main className="flex min-h-screen items-center justify-center bg-gray-50 px-4 py-12 transition-colors duration-200 dark:bg-gray-950">
      <section className="w-full max-w-md rounded-2xl border border-gray-200 bg-white p-8 text-center shadow-sm transition-colors duration-200 sm:p-10 dark:border-gray-800 dark:bg-gray-900">
        {/* Icon */}
        <div className="mx-auto mb-6 flex h-20 w-20 items-center justify-center rounded-full bg-brand-50 dark:bg-brand-950/50">
          {isConfirmed === true ? (
            <svg
              className="h-10 w-10 text-green-600 dark:text-green-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
              aria-hidden="true"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M5 13l4 4L19 7"
              />
            </svg>
          ) : isInvitation ? (
            <svg
              className="h-10 w-10 text-brand-600 dark:text-brand-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
              aria-hidden="true"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={1.7}
                d="M17 20h5v-2a4 4 0 00-5.5-3.7M9 20H2v-2a4 4 0 017-2.65M16 3.13a4 4 0 010 7.75M12 7a4 4 0 11-8 0 4 4 0 018 0z"
              />
            </svg>
          ) : (
            <svg
              className="h-10 w-10 text-brand-600 dark:text-brand-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
              aria-hidden="true"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={1.7}
                d="M3 7l9 6 9-6M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
              />
            </svg>
          )}
        </div>

        {/* Heading */}
        <h1 className="mb-3 text-3xl font-bold tracking-tight text-gray-900 dark:text-white">
          {isConfirmed === true
            ? t(`${translationKey}.success.title`)
            : t(`${translationKey}.title`)}
        </h1>

        {/* Description */}
        <p className="mb-8 text-base leading-7 text-gray-500 dark:text-gray-400">
          {isConfirmed === true
            ? t(`${translationKey}.success.description`)
            : t(`${translationKey}.description`)}
        </p>

        {/* Action */}
        {isConfirmed !== true && (
          <button
            type="button"
            onClick={handleAction}
            disabled={isLoading}
            className="w-full rounded-xl bg-brand-600 px-6 py-3.5 font-semibold text-white shadow-sm transition duration-200 hover:bg-brand-700 focus:outline-none focus:ring-2 focus:ring-brand-500 focus:ring-offset-2 focus:ring-offset-white active:scale-[0.98] disabled:cursor-not-allowed disabled:opacity-60 dark:focus:ring-offset-gray-900"
          >
            {isLoading
              ? t(
                  `${translationKey}.actions.${isInvitation ? "accepting" : "confirming"}`,
                )
              : t(
                  `${translationKey}.actions.${isInvitation ? "accept" : "confirm"}`,
                )}
          </button>
        )}

        {/* Error Alert */}
        {error && (
          <div className="mt-5 text-start">
            <Alert
              variant="error"
              title={t(`${translationKey}.errors.title`)}
              message={error}
            />
          </div>
        )}

        {/* Success Action */}
        {isConfirmed === true && (
          <Link
            to="/"
            className="block w-full rounded-xl bg-brand-600 px-6 py-3.5 font-semibold text-white shadow-sm transition duration-200 hover:bg-brand-700 focus:outline-none focus:ring-2 focus:ring-brand-500 focus:ring-offset-2 focus:ring-offset-white dark:bg-brand-400 dark:focus:ring-offset-gray-900"
          >
            {t(`${translationKey}.actions.home`)}
          </Link>
        )}

        {/* Footer */}
        <p className="mt-6 text-sm text-gray-400 dark:text-gray-500">
          {t(`${translationKey}.footer.notice`)}
        </p>

        {/* Branding */}
        <div className="mt-8 border-t border-gray-100 pt-5 dark:border-gray-800">
          <span className="text-lg font-bold tracking-tight text-brand-600 dark:text-brand-400">
            Saay
          </span>

          <p className="mt-1 text-xs text-gray-400 dark:text-gray-500">
            {t(`${translationKey}.footer.tagline`)}
          </p>
        </div>
      </section>
    </main>
  );
}
