import { t } from "i18next";
import ComponentCard from "~/components/ui/cards/ComponentCard";
import Button from "~/components/ui/button/Button";
import ChartTab from "~/components/ui/ChartTab";
import TasksTable from "~/components/ui/table/TasksTable";
import { useEffect, useState } from "react";
import { useAuth } from "~/context/AuthContext";
import { getUserTasksToday } from "~/services/api/tasks";
import type { TaskDto } from "~/types/tasks/TaskDto";

export const meta = () => [{ title: "Tasks | Saay" }];

export default function Tasks() {
  const currentUserId = useAuth()?.currentUserId;
  const [tasks, setTasks] = useState<TaskDto[] | null>(null);
  const [completedTasksCount, setCompletedTasksCount] = useState<number>(0);

  async function loadUserTasksToday() {
    if (currentUserId === null || currentUserId === undefined) return;

    const result = await getUserTasksToday(currentUserId);

    if (typeof result !== "string") {
      setTasks(result);
    }
  }

  // By default, today's tab is selected
  useEffect(() => {
    loadUserTasksToday();
  }, [currentUserId]);

  return (
    <div>
      <div className="min-h-screen rounded-2xl border border-gray-200 bg-white p-2 xl:py-3 dark:border-gray-800 dark:bg-white/3">
        <div className="mx-auto w-full ">
          <ComponentCard title={t("goals.myGoals")}>
            <div className="flex justify-between">
              <h3 className="text-xl font-semibold text-gray-800 dark:text-white/90">
                {t("common.overview")}
              </h3>

              <Button size="sm">Add a Task</Button>
            </div>
            <ChartTab />
            <div>
              <h3>Today</h3>
              <p>{"Tasks . Completed"}</p>
            </div>
            <TasksTable
              header="completed"
              tableData={tasks ?? []}
              onEdit={() => {}}
              onDelete={() => {}}
            />
          </ComponentCard>
        </div>
      </div>
    </div>
  );
}
