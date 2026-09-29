import { t } from "i18next";
import { TableLayout, TableRow, TableCell, TableBody } from ".";
import Badge from "../Badge";
import Button from "../button/Button";
import type { TaskDto } from "~/types/tasks/TaskDto";
import Checkbox from "~/components/form/input/Checkbox";
import { DeleteIcon } from "~/assets/icons";

type TasksTableProps = {
  tableData: TaskDto[];
  onEdit: (task: TaskDto) => void;
  onDelete: (taskId: number) => void;
};

export default function TasksTable({
  tableData,
  onEdit,
  onDelete,
}: TasksTableProps) {
  return (
    <TableLayout>
      {/* Table Body */}
      <TableBody className="divide-y divide-gray-100 dark:divide-gray-800">
        {tableData.map((task) => (
          <TableRow key={task.taskId}>
            <TableCell className="px-2 py-3 text-theme-sm text-gray-500 sm:px-4 dark:text-gray-400">
              <Checkbox checked={task.isDone} onChange={() => onEdit(task)} />
            </TableCell>

            <TableCell className="px-2 py-3 sm:px-4">
              <p className="text-theme-sm font-medium text-gray-800 dark:text-white/90">
                {task.title}
              </p>
            </TableCell>

            <TableCell className="px-2 py-3 text-theme-sm text-gray-500 sm:px-4 dark:text-gray-400">
              {/* {t(`goals.categories.${task.taskCategoryTitle}`)} */}
              <Badge color="light">{task.taskCategoryTitle}</Badge>
              {/* {task.taskCategoryTitle} */}
              {/* we can add a badge */}
            </TableCell>

            <TableCell className="px-2 py-3 text-theme-sm text-gray-500 sm:px-4 dark:text-gray-400">
              <Button
                size="sm"
                variant="outline"
                className="flex-1 px-2 sm:px-3"
                onClick={() => onDelete(task.taskId)}
              >
                <DeleteIcon />
              </Button>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </TableLayout>
  );
}
