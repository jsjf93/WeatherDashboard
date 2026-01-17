import React from "react";

interface CardProps {
  children: React.ReactNode;
}

export function Card({ children }: CardProps) {
  return <div className="glass-card rounded-lg p-4 shadow-md">{children}</div>;
}
