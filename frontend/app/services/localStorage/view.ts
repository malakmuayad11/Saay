import type { TasksView } from "~/types/tasks/TasksView";

export function updateView(newView: TasksView) {
  localStorage.setItem("tasksView", newView);
}
export function getView() {
  return localStorage.getItem("tasksView");
}
