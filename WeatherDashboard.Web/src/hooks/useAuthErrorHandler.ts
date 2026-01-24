import { useEffect } from "react";
import { useIsAuthenticated, useMsal } from "@azure/msal-react";

export function useAuthErrorHandler() {
  const isAuthenticated = useIsAuthenticated();
  const { instance, accounts } = useMsal();

  useEffect(() => {
    if (!isAuthenticated) return;

    const handleUnauthorized = async (event: Event) => {
      if (event instanceof ErrorEvent && event.message.includes("401")) {
        try {
          const account = accounts[0];
          if (account) {
            await instance.acquireTokenSilent({
              scopes: ["openid", "profile", "email"],
              account,
              forceRefresh: true,
            });
          }
        } catch (error) {
          console.error("Token refresh failed, redirecting to login:", error);
          instance.loginRedirect();
        }
      }
    };

    window.addEventListener("error", handleUnauthorized);
    return () => window.removeEventListener("error", handleUnauthorized);
  }, [isAuthenticated, instance, accounts]);
}
