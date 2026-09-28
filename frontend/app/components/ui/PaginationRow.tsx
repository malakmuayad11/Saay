import { t } from "i18next";
import Button from "./button/Button";

type PaginationRowProps = {
  from: number;
  to: number;
  total: number;
  onPreviousClick: () => void;
  onNextClick: () => void;
};

export function PaginationRow({
  from,
  to,
  total,
  onPreviousClick,
  onNextClick,
}: PaginationRowProps) {
  function handleNextClick() {
    if (to === total) return;
    onNextClick();
  }
  return (
    <div className="flex items-center justify-between gap-3 border-t border-gray-100 p-3 dark:border-gray-800">
      <p className="text-theme-sm text-gray-500 dark:text-gray-400">
        {t("common.showing")} {from}-{to} {t("common.of")} {total}
      </p>

      <div className="flex items-center gap-2">
        <Button
          disabled={from === 1}
          size="sm"
          variant="outline"
          onClick={onPreviousClick}
          aria-label={t("common.previous")}
        >
          &lt;
        </Button>

        <Button
          disabled={to >= total}
          size="sm"
          variant="outline"
          onClick={handleNextClick}
          aria-label={t("common.next")}
        >
          {t("common.next")} &gt;
        </Button>
      </div>
    </div>
  );
}
