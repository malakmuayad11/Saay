import Card from "../components/ui/Card";
import { DoneIcon, PendingIcon, TotalIcon, SparklesIcon } from "~/assets/icons";
import { useEffect, useState } from "react";
import { useAuth } from "~/context/AuthContext";
import {
  deleteGoal,
  getCompletedGoalsCount,
  getPendingGoalsCount,
  getUserGoals,
} from "~/services/api/goals";
import { getUserMission } from "~/services/api/users";
import type { Goal } from "~/types/goals/Goal";
import GoalsTable from "~/components/ui/table/GoalsTable";
import ComponentCard from "~/components/ui/ComponentCard";
import Button from "~/components/ui/button/Button";
import { AddEditGoalModal } from "~/components/goals/Modals/AddEditGoalModal";
import { DeleteGoalModal } from "~/components/goals/Modals/DeleteGoalModal";

export const meta = () => [{ title: "Goals | Saay" }];

export default function Goals() {
  const currentUserId = useAuth()?.currentUserId;

  const [completedGoalsCount, setCompletedGoalsCount] = useState<number | null>(
    null,
  );
  const [pendingGoalsCount, setPendingGoalsCount] = useState<number | null>(
    null,
  );
  const [mission, setMission] = useState<string | null>();
  const [goals, setGoals] = useState<Goal[] | null>(null);

  const [goalModalOpen, setGoalModalOpen] = useState(false);
  const [openDeleteModal, setOpenDeleteModal] = useState(false);
  const [selectedGoal, setSelectedGoal] = useState<Goal | null>(null); // Used for AddEditGoalModal
  const [deleteResult, setDeleteResult] = useState<boolean | null>(null);
  const [selectedGoalId, setSelectedGoalId] = useState<number | null>(null); // Used for DeleteGoalModal

  const totalGoals = (completedGoalsCount ?? 0) + (pendingGoalsCount ?? 0);

  useEffect(() => {
    let ignore = false;

    async function loadCompletedGoalsCount() {
      if (currentUserId === null || currentUserId === undefined) return;

      const result = await getCompletedGoalsCount(currentUserId);

      if (!ignore && typeof result !== "string") {
        setCompletedGoalsCount(result);
      }
    }

    loadCompletedGoalsCount();

    return () => {
      ignore = true;
    };
  }, [currentUserId]);

  useEffect(() => {
    let ignore = false;

    async function loadPendingGoalsCount() {
      if (currentUserId === null || currentUserId === undefined) return;

      const result = await getPendingGoalsCount(currentUserId);

      if (!ignore && typeof result !== "string") {
        setPendingGoalsCount(result);
      }
    }

    loadPendingGoalsCount();

    return () => {
      ignore = true;
    };
  }, [currentUserId]);

  useEffect(() => {
    let ignore = false;

    async function loadUserMission() {
      if (currentUserId === null || currentUserId === undefined) return;

      const result = await getUserMission(currentUserId);

      if (!ignore) {
        setMission(result);
      }
    }

    loadUserMission();

    return () => {
      ignore = true;
    };
  }, [currentUserId]);

  async function loadUserGoals() {
    if (currentUserId === null || currentUserId === undefined) return;

    const result = await getUserGoals(currentUserId);

    if (typeof result !== "string") {
      setGoals(result);
    }
  }

  function handleAdd() {
    setSelectedGoal(null);
    setGoalModalOpen(true);
  }

  function handleEdit(goal: Goal) {
    setSelectedGoal(goal);
    setGoalModalOpen(true);
  }

  function handleCloseModal() {
    setGoalModalOpen(false);
    setSelectedGoal(null);
  }

  function handleDeleteClick(goalId: number) {
    setSelectedGoalId(goalId);
    setDeleteResult(null);
    setOpenDeleteModal(true);
  }

  async function handleDeleteGoal() {
    if (selectedGoalId === null) return;

    const response = await deleteGoal(selectedGoalId);

    if (typeof response === "string" || response === false) {
      setDeleteResult(false);
      return;
    }

    if (response === true) {
      setDeleteResult(true);
      await loadUserGoals();
    }
  }

  useEffect(() => {
    loadUserGoals();
  }, [currentUserId]);

  return (
    <div>
      <div className="min-h-screen rounded-2xl border border-gray-200 bg-white px-5 py-7 xl:py-5 dark:border-gray-800 dark:bg-white/3">
        <div className="mx-auto w-full max-w-157.5">
          <ComponentCard title="My Goals">
            <div className="flex justify-between">
              <h3 className="mb-4 text-theme-xl font-semibold text-gray-800 sm:text-2xl dark:text-white/90">
                Overview
              </h3>

              <Button size="sm" onClick={handleAdd}>
                Add Goal
              </Button>
            </div>

            <div className="grid grid-cols-3 grid-rows-2 gap-2">
              <Card
                title="My Mission"
                statNum={mission ?? ""}
                Icon={SparklesIcon}
                cardClassName="col-span-3"
              />

              <Card title="Total Goals" statNum={totalGoals} Icon={TotalIcon} />

              <Card
                title="Completed Goals"
                statNum={completedGoalsCount ?? 0}
                Icon={DoneIcon}
              />

              <Card
                title="Pending Goals"
                statNum={pendingGoalsCount ?? 0}
                Icon={PendingIcon}
              />
            </div>

            <GoalsTable
              tableData={goals ?? []}
              headers={[
                "title",
                "category",
                "time frame",
                "deadline",
                "status",
                "actions",
              ]}
              onEdit={handleEdit}
              onDelete={handleDeleteClick}
            />
          </ComponentCard>

          <AddEditGoalModal
            isOpen={goalModalOpen}
            onClose={handleCloseModal}
            onGoalChanged={loadUserGoals}
            goal={selectedGoal}
          />
          <DeleteGoalModal
            isOpen={openDeleteModal}
            onCancel={() => {
              setOpenDeleteModal(false);
              setSelectedGoalId(null);
            }}
            onSave={handleDeleteGoal}
            error={deleteResult === false ? "Failed to delete goal" : null}
            success={deleteResult === true}
          />
        </div>
      </div>
    </div>
  );
}
