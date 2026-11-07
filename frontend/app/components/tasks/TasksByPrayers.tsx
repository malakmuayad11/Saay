import { useAuth } from "~/context/AuthContext";
import { useCallback, useEffect, useState } from "react";
import type { TasksByPrayerDto } from "~/types/tasks/TasksByPrayerDto";
import { getTasksByPrayers } from "~/services/api/tasks";
import PrayerCard from "../ui/cards/PrayerCard";
import { t } from "i18next";

type TasksByPrayersProps = {
  latitude: string;
  longtitude: string;
  onDelete: (taskId: number) => void | Promise<void>;
  refreshTrigger: number;
};

export default function TasksByPrayers({
  latitude,
  longtitude,
  onDelete,
  refreshTrigger,
}: TasksByPrayersProps) {
  const currentUserId = useAuth()?.currentUserId;

  const [tasksByPrayersData, setTasksByPrayersData] =
    useState<TasksByPrayerDto | null>(null);

  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadTasksByPrayers = useCallback(
    async (signal?: AbortSignal) => {
      if (currentUserId == null || !latitude || !longtitude) {
        setIsLoading(false);
        return;
      }

      setIsLoading(true);
      setError(null);

      try {
        const response = await getTasksByPrayers(
          currentUserId,
          latitude,
          longtitude,
        );

        if (typeof response === "string") {
          setError(response);
          return;
        }

        setTasksByPrayersData(response);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "An unexpected error occurred.",
        );
      } finally {
        if (!signal?.aborted) {
          setIsLoading(false);
        }
      }
    },
    [currentUserId, latitude, longtitude],
  );

  async function handleDelete(taskId: number) {
    await onDelete(taskId);
    await loadTasksByPrayers();
  }

  useEffect(() => {
    const controller = new AbortController();

    void loadTasksByPrayers(controller.signal);

    return () => {
      controller.abort();
    };
  }, [loadTasksByPrayers, refreshTrigger]);

  if (isLoading) {
    return (
      <div className="py-8 text-center text-theme-sm text-gray-500 dark:text-gray-400">
        {t("common.loading")}
      </div>
    );
  }

  if (error) {
    return (
      <div className="py-8 text-center text-theme-sm text-red-500">{error}</div>
    );
  }

  if (!tasksByPrayersData) {
    return (
      <div className="py-8 text-center text-theme-sm text-gray-500 dark:text-gray-400">
        {t("tasks.noTasksDescription")}
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
      <PrayerCard
        title={t("tasks.prayers.fajr")}
        time={tasksByPrayersData.prayerTimes[0]}
        statNum={tasksByPrayersData.fajrTasks.length}
        tasks={tasksByPrayersData.fajrTasks}
        onDelete={handleDelete}
      />

      <PrayerCard
        title={t("tasks.prayers.dhuha")}
        time={tasksByPrayersData.prayerTimes[1]}
        statNum={tasksByPrayersData.dhuhaTasks.length}
        tasks={tasksByPrayersData.dhuhaTasks}
        onDelete={handleDelete}
      />

      <PrayerCard
        title={t("tasks.prayers.dhuhr")}
        time={tasksByPrayersData.prayerTimes[2]}
        statNum={tasksByPrayersData.dhuhrTasks.length}
        tasks={tasksByPrayersData.dhuhrTasks}
        onDelete={handleDelete}
      />

      <PrayerCard
        title={t("tasks.prayers.asr")}
        time={tasksByPrayersData.prayerTimes[3]}
        statNum={tasksByPrayersData.asrTasks.length}
        tasks={tasksByPrayersData.asrTasks}
        onDelete={handleDelete}
      />

      <PrayerCard
        title={t("tasks.prayers.maghrib")}
        time={tasksByPrayersData.prayerTimes[4]}
        statNum={tasksByPrayersData.maghribTasks.length}
        tasks={tasksByPrayersData.maghribTasks}
        onDelete={handleDelete}
      />

      <PrayerCard
        title={t("tasks.prayers.isha")}
        time={tasksByPrayersData.prayerTimes[5]}
        statNum={tasksByPrayersData.ishaTasks.length}
        tasks={tasksByPrayersData.ishaTasks}
        onDelete={handleDelete}
      />

      <PrayerCard
        title={t("tasks.tasksWithoutPrayers")}
        time=""
        statNum={tasksByPrayersData.tasksWithoutPrayers.length}
        tasks={tasksByPrayersData.tasksWithoutPrayers}
        onDelete={handleDelete}
        className="md:col-span-2"
      />
    </div>
  );
}
