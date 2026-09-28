import { Modal } from "~/components/ui/Modal";
import Button from "~/components/ui/button/Button";
import Alert from "~/components/ui/Alert";

type DeleteGoalModalProps = {
  isOpen: boolean;
  onCancel: () => void;
  onSave: () => void;
  error: string | null;
  success: boolean;
};

export function DeleteGoalModal({
  isOpen,
  onCancel,
  onSave,
  error,
  success,
}: DeleteGoalModalProps) {
  return (
    <Modal
      isOpen={isOpen}
      onClose={onCancel}
      className="w-[calc(100%-2rem)] max-w-[450px] sm:w-[450px]"
    >
      <div className="p-6">
        {/* Alert */}
        {error && (
          <div className="mb-4">
            <Alert variant="error" title="Error" message={error} />
          </div>
        )}

        {success && (
          <div className="mb-4">
            <Alert
              variant="success"
              title="Success"
              message="Goal is deleted successfully"
            />
          </div>
        )}

        {/* Content */}
        <div className="mb-6 -mt-2">
          <h3 className="mb-2 text-lg font-semibold text-gray-800 dark:text-white/90">
            Are you sure you want to delete this goal?
          </h3>

          <p className="text-sm text-gray-500 dark:text-gray-400">
            This action cannot be undone.
          </p>
        </div>

        {/* Actions */}
        <div className="flex gap-3">
          <Button
            size="sm"
            variant="outline"
            className="flex-1"
            onClick={onCancel}
          >
            Cancel
          </Button>

          <Button size="sm" className="flex-1" onClick={onSave}>
            Delete
          </Button>
        </div>
      </div>
    </Modal>
  );
}
