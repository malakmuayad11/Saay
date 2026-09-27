import { t } from "i18next";
import { useState } from "react";
import { SparklesIcon, TotalIcon } from "~/assets/icons";
import Button from "../button/Button";
import Input from "~/components/form/input/InputField";
import { updateMission } from "~/services/api/users";

export type CardProps = {
  userId: number;
  initialMission: string;
  cardClassName?: string;
  onMissionUpdated: () => void;
};

export default function MissionCard({
  userId,
  initialMission,
  cardClassName,
  onMissionUpdated,
}: CardProps) {
  const [displayInput, setDisplayInput] = useState<boolean>(false);
  const [mission, setMission] = useState<string>(initialMission);

  async function handleSave() {
    if (mission === "") return;

    const response = await updateMission(userId, mission);

    if (typeof response === "string" || response === false) return;

    setDisplayInput(false);
    onMissionUpdated();
  }
  return (
    <div
      className={`${cardClassName ?? ""} rounded-2xl border border-gray-200 bg-white p-2 md:p-3 dark:border-gray-800 dark:bg-white/3`}
    >
      <div className="flex items-center justify-between">
        <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-gray-100 dark:bg-gray-800">
          <SparklesIcon className="size-6 text-gray-800 dark:text-white/90" />
        </div>
        <Button
          variant="outline"
          onClick={() => setDisplayInput(!displayInput)}
        >
          <TotalIcon />
        </Button>
      </div>

      <div className="mt-5">
        <span className="text-sm text-gray-500 dark:text-gray-400">
          {displayInput ? (
            <div className="flex w-full items-center gap-2">
              <div className="min-w-0 flex-1">
                <Input
                  placeholder={t("goals.missionPlaceholder")}
                  onChange={(e) => setMission(e.target.value.trim())}
                />
              </div>
              <Button
                className="shrink-0 whitespace-nowrap"
                size="sm"
                onClick={handleSave}
              >
                {t("common.save")}
              </Button>
            </div>
          ) : (
            t("goals.myMission")
          )}
        </span>

        {!displayInput && (
          <p className="mt-2 text-sm font-bold text-gray-800 dark:text-white/90">
            {initialMission}
          </p>
        )}
      </div>
    </div>
  );
}
