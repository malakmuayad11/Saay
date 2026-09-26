import type { ReactNode } from "react";

import { cn } from "~/utils";

// Props for Table
interface TableProps {
  children: ReactNode;
  className?: string;
}

// Props for TableHeader
interface TableHeaderProps {
  children: ReactNode;
  className?: string;
}

// Props for TableBody
interface TableBodyProps {
  children: ReactNode;
  className?: string;
}

// Props for TableRow
interface TableRowProps {
  children: ReactNode;
  className?: string;
}

// Props for TableCell
interface TableCellProps {
  children?: ReactNode;
  isHeader?: boolean;
  className?: string;
}

// Table Component
const TableLayout: React.FC<TableProps> = ({ children, className }) => {
  return (
    <div className="w-full overflow-x-auto">
      <table className={cn("min-w-full", className)}>{children}</table>
    </div>
  );
};

// TableHeader Component
const TableHeader: React.FC<TableHeaderProps> = ({ children, className }) => {
  return <thead className={className}>{children}</thead>;
};

// TableBody Component
const TableBody: React.FC<TableBodyProps> = ({ children, className }) => {
  return <tbody className={className}>{children}</tbody>;
};

// TableRow Component
const TableRow: React.FC<TableRowProps> = ({ children, className }) => {
  return <tr className={className}>{children}</tr>;
};

// TableCell Component
const TableCell: React.FC<TableCellProps> = ({
  children,
  isHeader = false,
  className,
}) => {
  const CellTag = isHeader ? "th" : "td";

  return <CellTag className={cn(className)}>{children}</CellTag>;
};

export { TableLayout, TableBody, TableCell, TableHeader, TableRow };
