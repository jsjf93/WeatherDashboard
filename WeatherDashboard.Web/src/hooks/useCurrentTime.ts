import { useState, useEffect } from "react";

/**
 * Hook that provides the current time as a Unix timestamp (seconds since epoch)
 * Updates every minute to keep the time current
 */
export function useCurrentTime(): number {
  // Initialize with current time using lazy initializer
  const [currentTime, setCurrentTime] = useState(() => Math.floor(Date.now() / 1000));

  useEffect(() => {
    const updateTime = () => {
      setCurrentTime(Math.floor(Date.now() / 1000));
    };
    // Update every minute to keep time current
    const interval = setInterval(updateTime, 60000);
    return () => clearInterval(interval);
  }, []);

  return currentTime;
}
