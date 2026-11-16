import type { GroupDto } from "~/types/groups/GroupDto";
import ComponentCard from "../ui/cards/ComponentCard";
import Button from "../ui/button/Button";
import { useEffect, useState } from "react";
import { Modal } from "../ui/modal/Modal";
import Input from "../form/input/InputField";
import { EMAIL_REGEX } from "~/validation";
import { t } from "i18next";
import { deleteGroup, InviteUserToGroup } from "~/services/api/groups";
import { DeleteModal } from "../ui/modal/DeleteModal";

type GroupCardProps = {
  group: GroupDto;
  onDelete: () => void;
};

export default function GroupCard({ group, onDelete }: GroupCardProps) {
  const [inviteModalVisible, setInviteModalVisible] = useState(false);
  const [email, setEmail] = useState("");
  const [emailValid, setEmailValid] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [deleteModalVisible, setDeleteModalVisible] = useState<boolean>(false);
  const [deleteResult, setDeleteResult] = useState<boolean | null>(null);
  const [deleteError, setDeleteError] = useState<string | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    if (deleteResult !== true) return;

    const timeoutId = setTimeout(() => {
      setDeleteModalVisible(false);
      setDeleteResult(null);
      onDelete();
    }, 2000);

    return () => clearTimeout(timeoutId);
  }, [deleteResult, onDelete]);

  const handleInvite = async () => {
    const trimmedEmail = email.trim();

    if (!trimmedEmail || !EMAIL_REGEX.test(trimmedEmail)) {
      setEmailValid(false);
      return;
    }

    setError("");
    setSuccess("");
    setIsSubmitting(true);

    try {
      const result = await InviteUserToGroup(group.groupId, trimmedEmail);

      if (typeof result === "string") {
        setError(result);
        return;
      }

      setSuccess(t("groups.invitationSent"));
      setEmail("");
      setEmailValid(true);
    } catch {
      setError(t("common.unexpectedError"));
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleDelete = async () => {
    setDeleteError(null);
    setDeleteResult(null);
    setIsDeleting(true);

    try {
      const result = await deleteGroup(group.groupId);

      if (typeof result === "string") {
        setDeleteError(result);
        setDeleteResult(false);
        return;
      }

      if (!result) {
        setDeleteError(t("common.unexpectedError"));
        setDeleteResult(false);
        return;
      }

      setDeleteResult(true);
    } catch {
      setDeleteError(t("common.unexpectedError"));
      setDeleteResult(false);
    } finally {
      setIsDeleting(false);
    }
  };

  const handleCancel = () => {
    setEmail("");
    setEmailValid(true);
    setError("");
    setSuccess("");
    setInviteModalVisible(false);
  };

  return (
    <ComponentCard
      title={group.groupName}
      desc={
        group.groupDescription
          ? `${group.groupDescription} - ${t("groups.memberCount", {
              count: group.membersCount,
            })}`
          : t("groups.memberCount", { count: group.membersCount })
      }
    >
      <div>
        <p className="text-gray-800 dark:text-white/90">
          {t("groups.members")}
        </p>

        <ul>
          {group.members.map((m) => (
            <li key={m.userId} className="flex gap-1 my-2">
              <img
                className="w-6 h-6 overflow-hidden rounded-full"
                src={
                  m.profilePictureUrl ??
                  "/assets/profile-picture-placeholder.png"
                }
                alt={t("groups.profilePictureAlt", { name: m.fullName })}
              />
              <p className="text-gray-800 dark:text-white/90">{m.fullName}</p>
            </li>
          ))}
        </ul>

        {group.isRequestedUserOwner && (
          <div className="flex gap-2">
            <Button
              size="sm"
              className="flex-3"
              onClick={() => {
                setError("");
                setSuccess("");
                setInviteModalVisible(true);
              }}
            >
              {t("groups.inviteMember")}
            </Button>

            <Button
              size="sm"
              variant="outline"
              onClick={() => setDeleteModalVisible(true)}
            >
              {t("groups.removeGroup")}
            </Button>
          </div>
        )}

        <Modal
          isOpen={inviteModalVisible}
          onClose={handleCancel}
          className="max-w-md p-6"
        >
          <form
            noValidate
            onSubmit={(e) => {
              e.preventDefault();
              handleInvite();
            }}
          >
            <h3 className="text-lg font-semibold text-gray-800 dark:text-white/90 mb-4">
              {t("groups.inviteMember")}
            </h3>

            <label
              htmlFor="invite-email"
              className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2"
            >
              {t("groups.emailAddress")}
            </label>

            <Input
              id="invite-email"
              type="email"
              value={email}
              onChange={(e) => {
                setEmail(e.target.value);
                setEmailValid(true);
                setError("");
                setSuccess("");
              }}
              placeholder={t("groups.emailPlaceholder")}
              hint={!emailValid ? t("signUp.emailError") : undefined}
              error={!emailValid}
              onBlur={() => setEmailValid(EMAIL_REGEX.test(email.trim()))}
              className="w-full"
            />

            {error && <p className="text-sm text-error-500 mt-2">{error}</p>}

            {success && (
              <p className="text-sm text-success-500 mt-2">{success}</p>
            )}

            <div className="flex justify-end gap-3 mt-6">
              <Button
                size="sm"
                variant="outline"
                onClick={handleCancel}
                disabled={isSubmitting}
              >
                {t("groups.cancel")}
              </Button>

              <Button size="sm" disabled={isSubmitting}>
                {isSubmitting ? t("common.sending") : t("groups.send")}
              </Button>
            </div>
          </form>
        </Modal>
      </div>
      {deleteModalVisible && (
        <DeleteModal
          isOpen={deleteModalVisible}
          onCancel={() => setDeleteModalVisible(false)}
          onSave={handleDelete}
          title={t("groups.removeGroupConfirmation", {
            groupName: group.groupName,
          })}
          description={t("groups.deleteWarning")}
          error={deleteResult === false ? t("groups.deleteFailed") : null}
          success={deleteResult === true}
          successMessage={t("groups.groupDeleted")}
        />
      )}
    </ComponentCard>
  );
}
