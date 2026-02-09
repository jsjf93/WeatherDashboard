import { X, XCircle, CheckCircle, Info, AlertTriangle } from "lucide-react";
import type { Toast as ToastType } from "../types";

interface ToastProps {
  toast: ToastType;
  onClose: () => void;
}

const variantStyles = {
  error: {
    icon: XCircle,
    bgColor: "bg-red-900/90",
    borderColor: "border-red-500/50",
    iconColor: "text-red-300",
  },
  success: {
    icon: CheckCircle,
    bgColor: "bg-green-900/90",
    borderColor: "border-green-500/50",
    iconColor: "text-green-300",
  },
  info: {
    icon: Info,
    bgColor: "bg-blue-900/90",
    borderColor: "border-blue-500/50",
    iconColor: "text-blue-300",
  },
  warning: {
    icon: AlertTriangle,
    bgColor: "bg-yellow-900/90",
    borderColor: "border-yellow-500/50",
    iconColor: "text-yellow-300",
  },
};

export function Toast({ toast, onClose }: ToastProps) {
  const style = variantStyles[toast.variant];
  const Icon = style.icon;

  return (
    <div
      role="alert"
      aria-live="assertive"
      aria-atomic="true"
      className={`
        ${style.bgColor} ${style.borderColor}
        backdrop-blur-xl border rounded-lg shadow-2xl
        p-4 pr-12 min-w-[320px] max-w-md
        animate-in slide-in-from-right duration-300
        relative
      `}
    >
      <div className="flex items-start gap-3">
        <Icon
          className={`${style.iconColor} w-5 h-5 shrink-0 mt-0.5`}
          aria-hidden="true"
        />
        <p className="text-sm text-white leading-relaxed flex-1">
          {toast.message}
        </p>
      </div>
      <button
        onClick={onClose}
        aria-label="Dismiss notification"
        className="
          absolute top-3 right-3
          text-white/70 hover:text-white
          transition-colors
          focus:outline-none focus:ring-2 focus:ring-white/50 focus:ring-offset-2 focus:ring-offset-transparent
          rounded-sm
        "
      >
        <X className="w-4 h-4" aria-hidden="true" />
      </button>
    </div>
  );
}
