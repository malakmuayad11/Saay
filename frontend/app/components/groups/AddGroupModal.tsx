import { useEffect, useRef, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "~/context/AuthContext";
import { addGroup } from "~/services/api/groups";
import { Modal } from "~/components/ui/modal/Modal";
import Label from "~/components/form/Label";
import Input from "~/components/form/input/InputField";
import Button from "~/components/ui/button/Button";
import Alert from "~/components/ui/Alert";
import type { AddGroupDto } from "~/types/groups/AddGroupDto";

type AddGroupModalProps = {
  isOpen: boolean;
  onClose: () => void;
  onGroupAdded: () => void;
};

export function AddGroupModal({
  isOpen,
  onClose,
  onGroupAdded,
}: AddGroupModalProps) {
  const { t } = useTranslation();
  const currentUserId = useAuth()?.currentUserId;

  const [groupName, setGroupName] = useState("");
  const [groupNameValid, setGroupNameValid] = useState(true);

  const [description, setDescription] = useState("");

  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  function resetForm() {
    setGroupName("");
    setDescription("");
    setGroupNameValid(true);
    setError(null);
    setSuccess(false);
  }

  function handleClose() {
    if (saving) return;

    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
      timeoutRef.current = null;
    }

    resetForm();
    onClose();
  }

  useEffect(() => {
    return () => {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
    };
  }, []);

  async function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const trimmedName = groupName.trim();

    if (!trimmedName) {
      setGroupNameValid(false);
      return;
    }

    if (trimmedName.length > 100 || description.trim().length > 200) {
      setError(t("groups.validation.maxLength"));
      return;
    }

    if (currentUserId == null) {
      setError(t("common.unexpectedError"));
      return;
    }

    setSaving(true);
    setError(null);

    try {
      const result = await addGroup({
        userId: currentUserId,
        groupName: trimmedName,
        description: description.trim() || null,
      } as AddGroupDto);

      if (typeof result === "string") {
        setError(result);
        return;
      }

      setSuccess(true);
      onGroupAdded();

      timeoutRef.current = setTimeout(() => {
        resetForm();
        onClose();
        timeoutRef.current = null;
      }, 1500);
    } catch {
      setError(t("common.unexpectedError"));
    } finally {
      setSaving(false);
    }
  }

  return (
    <Modal
      isOpen={isOpen}
      onClose={handleClose}
      className="w-[calc(100%-2rem)] max-w-[500px] sm:w-[500px]"
    >
      <div className="p-6">
        {error && (
          <div className="mb-4">
            <Alert
              variant="error"
              title={t("groups.alerts.error")}
              message={error}
            />
          </div>
        )}

        {success && (
          <div className="mb-4">
            <Alert
              variant="success"
              title={t("groups.alerts.success")}
              message={t("groups.alerts.added")}
            />
          </div>
        )}

        <h3 className="mb-6 text-lg font-semibold dark:text-white">
          {t("groups.modal.addTitle")}
        </h3>

        <form noValidate onSubmit={handleSubmit}>
          <div className="space-y-6">
            {/* Group Name */}
            <div>
              <Label htmlFor="groupName">{t("groups.fields.name")}</Label>

              <Input
                id="groupName"
                value={groupName}
                maxLength={100}
                onChange={(e) => {
                  setGroupName(e.target.value);
                  setGroupNameValid(true);
                  setError(null);
                }}
                hint={
                  !groupNameValid ? t("groups.validation.required") : undefined
                }
                onBlur={() => setGroupNameValid(groupName.trim() !== "")}
                error={!groupNameValid}
                placeholder={t("groups.placeholders.name")}
              />
            </div>

            {/* Description */}
            <div>
              <Label htmlFor="groupDescription">
                {t("groups.fields.description")}
              </Label>

              <textarea
                id="groupDescription"
                value={description}
                maxLength={200}
                onChange={(e) => setDescription(e.target.value)}
                placeholder={t("groups.placeholders.description")}
                rows={4}
                className="w-full rounded-lg border border-gray-300 bg-transparent px-4 py-3 text-sm text-gray-800 outline-none focus:border-primary dark:border-gray-700 dark:text-white"
              />

              <p className="mt-1 text-right text-xs text-gray-500">
                {description.length}/200
              </p>
            </div>
          </div>

          <Button
            className="mt-6 w-full"
            size="md"
            disabled={saving || success}
          >
            {saving ? t("groups.buttons.adding") : t("groups.buttons.add")}
          </Button>
        </form>
      </div>
    </Modal>
  );
}
