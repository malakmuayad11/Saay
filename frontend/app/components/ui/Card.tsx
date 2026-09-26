import type { ElementType } from "react";

export type CardProps = {
  title: string;
  statNum: number | string;
  Icon?: ElementType;
  cardClassName?: string;
};

export default function Card({
  title,
  statNum,
  Icon,
  cardClassName,
}: CardProps) {
  return (
    <div
      className={`${cardClassName && cardClassName} rounded-2xl border border-gray-200 bg-white p-5 md:p-6 dark:border-gray-800 dark:bg-white/3`}
    >
      {/* Metric Item Start */}
      <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-gray-100 dark:bg-gray-800">
        {Icon && <Icon className="size-6 text-gray-800 dark:text-white/90" />}
      </div>

      <div className="mt-5 flex items-end justify-between">
        <div>
          <span className="text-sm text-gray-500 dark:text-gray-400">
            {title}
          </span>

          <p className="mt-2 text-title-sm font-bold text-gray-800 dark:text-white/90">
            {statNum}
          </p>
        </div>
      </div>
    </div>
  );
}
