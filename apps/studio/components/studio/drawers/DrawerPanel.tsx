import React from "react";

export type DrawerPanelProps = {
  id: string;
  title: string;
  isActive: boolean;
  onClose: () => void;
  children?: React.ReactNode;
};

const DrawerPanel: React.FC<DrawerPanelProps> = ({ title, isActive, onClose, children }) => {
  if (!isActive) return null;

  return (
    <section
      role="complementary"
      aria-label={title}
      className="flex h-full flex-col gap-4 border-l border-[color:var(--studio-color-border-subtle)] bg-[color:var(--studio-color-bg-elevated)] p-4 shadow-lg"
    >
      <header className="flex items-center justify-between gap-2">
        <h2 className="text-base font-semibold text-[color:var(--studio-color-text-strong)]">{title}</h2>
        <button
          type="button"
          onClick={onClose}
          aria-label="Close panel"
          className="rounded-md p-2 text-sm text-[color:var(--studio-color-text-muted)] hover:bg-[color:var(--studio-color-bg-muted)] hover:text-[color:var(--studio-color-text-strong)]"
        >
          ×
        </button>
      </header>

      <div className="flex-1 overflow-y-auto">{children}</div>
    </section>
  );
};

export default DrawerPanel;


