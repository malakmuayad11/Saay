import { t } from "i18next";
import TokenActionPage from "~/components/ui/TokenActionPage";

export const meta = () => [{ title: t("routes.confirm") }];

export default function ConfirmEmail() {
  return <TokenActionPage type="emailConfirmation" />;
}
