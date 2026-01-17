import { Component } from "react";

interface TagProps {
  label: string;
  onClick?: () => void;
}

export function Tag({ label, onClick }: TagProps) {
  return (
    <Component
      className="rounded-full"
      onClick={onClick}
      as={onClick ? "button" : "span"}
    >
      {label}
    </Component>
  );
}
