import { Modal } from "~/components/ui/Modal";
import Label from "~/components/form/Label";
import Input from "~/components/form/input/InputField";
import Select from "~/components/form/input/Select";
import { useState, useEffect } from "react";
import Button from "~/components/ui/button/Button";
import { addTask, getCategories } from "~/services/api/tasks";
import { useAuth } from "~/context/AuthContext";
import Alert from "~/components/ui/Alert";
import type { TaskCategoryDto } from "~/types/tasks/TaskCategoryDto";

type AddTaskModalProps = {
  isOpen: boolean;
  onClose: () => void;
  onTaskAdded: () => void;
  dueDate: string;
};

export function AddTaskModal({
  isOpen,
  onClose,
  onTaskAdded,
  dueDate,
}: AddTaskModalProps) {
  const currentUserId = useAuth()?.currentUserId;

  const [taskTitle, setTaskTitle] = useState("");
  const [taskTitleValid, setTaskTitleValid] = useState(true);

  const [taskCategoryId, setTaskCategoryId] = useState("");
  const [taskCategoryValid, setTaskCategoryValid] = useState(true);

  const [repetition, setRepetition] = useState("");
  const [repetitionValid, setRepetitionValid] = useState(true);

  const [dueTime, setDueTime] = useState("");

  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const [categories, setCategories] = useState<TaskCategoryDto[]>([]);

  /*
   * Load task categories.
   */
  useEffect(() => {
    let ignore = false;

    async function loadCategories() {
      const result = await getCategories();

      if (!ignore && typeof result !== "string") {
        setCategories(result);
      }
    }

    loadCategories();

    return () => {
      ignore = true;
    };
  }, []);

  /*
   * Reset the form whenever the modal opens.
   */
  useEffect(() => {
    if (!isOpen) return;

    resetForm();
    setError(null);
    setSuccess(false);
  }, [isOpen]);

  const categoryOptions = categories.map((category) => ({
    value: String(category.taskCategoryId),
    label: category.title,
  }));

  const repetitionOptions = [
    { value: "0", label: "Once" },
    { value: "1", label: "Daily" },
    { value: "2", label: "Weekly" },
    { value: "3", label: "Monthly" },
  ];

  function resetForm() {
    setTaskTitle("");
    setTaskCategoryId("");
    setRepetition("");
    setDueTime("");

    setTaskTitleValid(true);
    setTaskCategoryValid(true);
    setRepetitionValid(true);
  }

  function validateFields() {
    return (
      taskTitle.trim() !== "" && taskCategoryId !== "" && repetition !== ""
    );
  }

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const titleValid = taskTitle.trim() !== "";
    const categoryValid = taskCategoryId !== "";
    const repetitionIsValid = repetition !== "";

    setTaskTitleValid(titleValid);
    setTaskCategoryValid(categoryValid);
    setRepetitionValid(repetitionIsValid);

    if (!validateFields()) {
      return;
    }

    if (currentUserId == null) {
      setError("User information is not available.");
      return;
    }

    setSaving(true);
    setError(null);

    const result = await addTask({
      userId: currentUserId,
      taskCategoryId: Number(taskCategoryId),
      title: taskTitle.trim(),
      repetition: Number(repetition),
      dueDate,
      dueTime: dueTime || null,
    });

    if (typeof result === "string") {
      setError(result);
      setSaving(false);
      return;
    }

    setSaving(false);
    setSuccess(true);

    onTaskAdded();

    setTimeout(() => {
      resetForm();
      onClose();
      setSuccess(false);
    }, 1500);
  }

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      className="w-[calc(100%-2rem)] max-w-[500px] sm:w-[500px]"
    >
      <div className="p-6">
        {/* Alert */}
        {error && (
          <div className="mt-6 mb-4">
            <Alert variant="error" title="Error" message={error} />
          </div>
        )}

        {success && (
          <div className="mt-6 mb-4">
            <Alert
              variant="success"
              title="Success"
              message="Task is added successfully!"
            />
          </div>
        )}

        <h3 className="mb-6 text-lg font-semibold">Add New Task</h3>

        <form onSubmit={handleSubmit}>
          <div className="space-y-6">
            {/* Task Title */}
            <div>
              <Label htmlFor="taskTitle">Task Title</Label>

              <Input
                id="taskTitle"
                value={taskTitle}
                onChange={(e) => setTaskTitle(e.target.value)}
                hint={!taskTitleValid ? "This field is required" : undefined}
                onBlur={() => setTaskTitleValid(taskTitle.trim() !== "")}
                error={!taskTitleValid}
              />
            </div>

            {/* Task Category */}
            <div>
              <Label>Task Category</Label>

              <Select
                options={categoryOptions}
                value={taskCategoryId}
                placeholder="Select a category"
                onChange={(value) => {
                  setTaskCategoryId(value);
                  setTaskCategoryValid(value !== "");
                }}
              />

              {!taskCategoryValid && (
                <p className="mt-1.5 text-sm text-error-500">
                  This field is required
                </p>
              )}
            </div>

            {/* Repetition */}
            <div>
              <Label>Repetition</Label>

              <Select
                options={repetitionOptions}
                value={repetition}
                placeholder="Select repetition"
                onChange={(value) => {
                  setRepetition(value);
                  setRepetitionValid(value !== "");
                }}
              />

              {!repetitionValid && (
                <p className="mt-1.5 text-sm text-error-500">
                  This field is required
                </p>
              )}
            </div>

            {/* Due Time */}
            <div>
              <Label htmlFor="dueTime">
                Due Time <span className="text-gray-400">(Optional)</span>
              </Label>

              <Input
                id="dueTime"
                type="time"
                value={dueTime}
                onChange={(e) => setDueTime(e.target.value)}
              />
            </div>
          </div>

          <Button className="mt-6 w-full" size="md" disabled={saving}>
            {saving ? "Adding..." : "Add Task"}
          </Button>
        </form>
      </div>
    </Modal>
  );
}
