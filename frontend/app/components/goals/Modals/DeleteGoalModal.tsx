import { Modal } from "~/components/ui/Modal";
import { useState } from "react";
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
    <Modal isOpen={isOpen} onClose={onCancel}>
      {error && <Alert variant="error" title="Error" message={error} />}
      {success && (
        <Alert
          variant="success"
          title="Success"
          message="Goal is deleted successfully"
        />
      )}
      <h3>Are you sure you want to delete this goal?</h3>
      <p>This action cannot be undone.</p>
      <div className="flex ">
        <Button size="sm" variant="outline" className="flex-1" onClick={onSave}>
          Delete
        </Button>
        <Button size="sm" className="flex-1" onClick={onCancel}>
          Cancel
        </Button>
      </div>
    </Modal>
  );
}
