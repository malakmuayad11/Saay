import { t } from "i18next";
import { TableLayout, TableHeader, TableRow, TableCell, TableBody } from ".";
import type { Goal } from "~/types/goals/Goal";
import Badge from "../Badge";
import Button from "../button/Button";

type TableProps = {
  headers: string[];
  tableData: Goal[];
  onEdit: (goal: Goal) => void;
  onDelete: (goalId: number) => void;
};

export default function GoalsTable({
  tableData,
  headers,
  onEdit,
  onDelete,
}: TableProps) {
  return (
    <TableLayout>
      {/* Table Header */}
      <TableHeader className="border-y border-gray-100 dark:border-gray-800">
        <TableRow>
          {headers.map((h) => (
            <TableCell
              key={h}
              isHeader
              className={`px-2 py-3 text-start text-theme-xs font-medium text-gray-500 sm:px-4 dark:text-gray-400 ${
                h === t("goals.headers.timeFrame") ? "hidden md:table-cell" : ""
              }`}
            >
              {h}
            </TableCell>
          ))}
        </TableRow>
      </TableHeader>

      {/* Table Body */}
      <TableBody className="divide-y divide-gray-100 dark:divide-gray-800">
        {tableData.map((goal) => (
          <TableRow key={goal.goalId}>
            <TableCell className="px-2 py-3 sm:px-4">
              <p className="text-theme-sm font-medium text-gray-800 dark:text-white/90">
                {goal.title}
              </p>
            </TableCell>

            <TableCell className="px-2 py-3 text-theme-sm text-gray-500 sm:px-4 dark:text-gray-400">
              {t(`goals.categories.${goal.categoryTitle}`)}
            </TableCell>

            <TableCell className="hidden px-2 py-3 text-theme-sm text-gray-500 sm:px-4 md:table-cell dark:text-gray-400">
              {t(`goals.timeFrames.${goal.timeFrame}`)}
            </TableCell>

            <TableCell className="px-2 py-3 text-theme-sm text-gray-500 sm:px-4 dark:text-gray-400">
              {goal.deadline.toString().slice(2)}
            </TableCell>

            <TableCell className="px-2 py-3 text-theme-sm text-gray-500 sm:px-4 dark:text-gray-400">
              <Badge color={goal.isDone ? "success" : "warning"}>
                {goal.isDone
                  ? t("goals.status.done")
                  : t("goals.status.progress")}
              </Badge>
            </TableCell>

            <TableCell className="px-2 py-3 text-theme-sm text-gray-500 sm:px-4 dark:text-gray-400">
              <div className="flex gap-1 sm:gap-2">
                <Button
                  size="sm"
                  className="flex-1 px-2 sm:px-3"
                  onClick={() => onEdit(goal)}
                >
                  {t("goals.headers.actions.edit")}
                </Button>

                <Button
                  size="sm"
                  className="flex-1 px-2 sm:px-3"
                  onClick={() => onDelete(goal.goalId)}
                >
                  {t("goals.headers.actions.delete")}
                </Button>
              </div>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </TableLayout>
  );
}
