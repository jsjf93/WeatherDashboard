import { useMsal } from "@azure/msal-react";
import { AuthButton } from "./AuthButton";

export function Header() {
  const { accounts } = useMsal();

  return (
    <header className="sticky top-0 z-50 w-full bg-black/80 backdrop-blur-md border-b border-white/10">
      <div className="px-4 md:px-10 py-4 flex items-center justify-between">
        <div className="flex items-center gap-2">
          <span className="text-2xl">⛅</span>
          <h1 className="text-xl font-semibold text-white">
            Weather Dashboard
          </h1>
        </div>

        <div className="flex items-center gap-4">
          {accounts.length > 0 && (
            <span className="text-sm text-gray-300">{accounts[0].name}</span>
          )}
          <AuthButton />
        </div>
      </div>
    </header>
  );
}
