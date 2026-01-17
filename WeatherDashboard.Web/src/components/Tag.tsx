import React from "react";

interface TagProps {
  label: React.ReactNode;
  onClick?: () => void;
}

export function Tag({ label, onClick }: TagProps) {
  const Component = onClick ? "button" : "span";

  return (
    <Component
      className="rounded-full glass px-3 py-1 text-sm data-[isButton='true']:cursor-pointer hover:brightness-95 transition"
      data-isButton={!!onClick}
      onClick={onClick}
    >
      {label}
    </Component>
  );
}
