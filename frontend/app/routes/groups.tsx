import { t } from "i18next";
import { useEffect, useState } from "react";
import { AddGroupModal } from "~/components/groups/AddGroupModal";
import GroupCard from "~/components/groups/GroupCard";
import Button from "~/components/ui/button/Button";
import ComponentCard from "~/components/ui/cards/ComponentCard";
import { useAuth } from "~/context/AuthContext";
import { getUserGroups } from "~/services/api/groups";
import type { GroupDto } from "~/types/groups/GroupDto";

export const meta = () => [{ title: t("routes.groups") }];

export default function Groups() {
  const [groups, setGroups] = useState<GroupDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [addModalVisible, setAddModalVisible] = useState<boolean>(false);

  const currentUserId = useAuth()?.currentUserId;

  async function loadGroups() {
    if (currentUserId === null || currentUserId === undefined) {
      setLoading(false);
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const result = await getUserGroups(currentUserId);

      if (typeof result === "string") {
        setError(result);
        return;
      }

      setGroups(result);
    } catch {
      setError(t("common.unexpectedError"));
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadGroups();
  }, [currentUserId]);

  return (
    <div>
      <div className="min-h-screen rounded-2xl border border-gray-200 bg-white p-2 xl:py-3 dark:border-gray-800 dark:bg-gray-900">
        <div className="mx-auto w-full">
          <ComponentCard title={t("groups.myGroups")}>
            <div className="flex justify-between">
              <h3 className="text-xl font-semibold text-gray-800 dark:text-white/90">
                {t("common.overview")}
              </h3>

              <Button size="sm" onClick={() => setAddModalVisible(true)}>
                {t("groups.createGroup")}
              </Button>
            </div>

            {loading ? (
              <p>Loading groups...</p>
            ) : error ? (
              <p>{error}</p>
            ) : groups.length === 0 ? (
              <p>No groups found.</p>
            ) : (
              <div className="grid md:grid-cols-2 gap-2">
                {groups.map((g) => (
                  <GroupCard key={g.groupId} group={g} onDelete={loadGroups} />
                ))}
              </div>
            )}
            {addModalVisible && (
              <AddGroupModal
                isOpen={addModalVisible}
                onClose={() => setAddModalVisible(false)}
                onGroupAdded={loadGroups}
              />
            )}
          </ComponentCard>
        </div>
      </div>
    </div>
  );
}
