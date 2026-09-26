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
              className="py-3 text-start text-theme-xs font-medium text-gray-500 dark:text-gray-400"
            >
              {h}
            </TableCell>
          ))}
        </TableRow>
      </TableHeader>

      {/* Table Body */}
      <TableBody className="divide-y divide-gray-100 dark:divide-gray-800">
        {tableData.map((goal) => (
          <TableRow key={goal.goalId} className="">
            <TableCell className="py-3">
              <p className="text-theme-sm font-medium text-gray-800 dark:text-white/90">
                {goal.title}
              </p>
            </TableCell>
            <TableCell className="py-3 text-theme-sm text-gray-500 dark:text-gray-400">
              {goal.categoryTitle}
            </TableCell>
            <TableCell className="py-3 text-theme-sm text-gray-500 dark:text-gray-400">
              {goal.timeFrame}
            </TableCell>
            <TableCell className="py-3 text-theme-sm text-gray-500 dark:text-gray-400">
              {goal.deadline.toString().slice(2)}
            </TableCell>
            <TableCell className="py-3 text-theme-sm text-gray-500 dark:text-gray-400">
              <Badge color={goal.isDone ? "success" : "warning"}>
                {goal.isDone ? "Done" : "In Progress"}
              </Badge>
            </TableCell>
            <TableCell className="py-3 text-theme-sm text-gray-500 dark:text-gray-400">
              <div className="flex justify-between gap-1">
                <Button
                  size="sm"
                  className="flex-1"
                  onClick={() => onEdit(goal)}
                >
                  Edit
                </Button>
                <Button
                  size="sm"
                  className="flex-1"
                  onClick={() => onDelete(goal.goalId)}
                >
                  Delete
                </Button>
              </div>
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </TableLayout>
  );
}
