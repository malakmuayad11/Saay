import { t } from "i18next";
import TokenActionPage from "~/components/ui/TokenActionPage";

export const meta = () => [{ title: t("routes.acceptInvitation") }];

export default function AcceptInvitation() {
  return <TokenActionPage type="groupInvitation" />;
}
