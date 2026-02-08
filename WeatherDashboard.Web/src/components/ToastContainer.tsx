import { useEffect } from "react";
import { Toast } from "./Toast";
import { useAppSelector, useAppDispatch } from "../hooks/useRedux";
import { selectToasts, removeToast } from "../features/toast/toastSlice";

const AUTO_DISMISS_DURATION = 5000;
const MAX_TOASTS = 3;

export function ToastContainer() {
  const toasts = useAppSelector(selectToasts);
  const dispatch = useAppDispatch();

  const visibleToasts = toasts.slice(0, MAX_TOASTS);

  useEffect(() => {
    const timers = visibleToasts.map((toast) => {
      const timeElapsed = Date.now() - toast.timestamp;
      const remainingTime = Math.max(0, AUTO_DISMISS_DURATION - timeElapsed);

      return setTimeout(() => {
        dispatch(removeToast(toast.id));
      }, remainingTime);
    });

    return () => {
      timers.forEach((timer) => clearTimeout(timer));
    };
  }, [visibleToasts, dispatch]);

  const handleClose = (toastId: string) => {
    dispatch(removeToast(toastId));
  };

  if (visibleToasts.length === 0) {
    return null;
  }

  return (
    <div
      className="fixed top-4 right-4 z-50 flex flex-col gap-2 pointer-events-none"
      aria-live="polite"
      aria-relevant="additions"
    >
      {visibleToasts.map((toast) => (
        <div key={toast.id} className="pointer-events-auto">
          <Toast toast={toast} onClose={() => handleClose(toast.id)} />
        </div>
      ))}
    </div>
  );
}
