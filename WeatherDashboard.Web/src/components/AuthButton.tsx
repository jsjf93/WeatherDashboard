import { useMsal } from "@azure/msal-react";
import { LogIn, LogOut } from "lucide-react";
import { scopes } from "../config/auth";

export function AuthButton() {
  const { instance, accounts } = useMsal();

  const handleLogin = () => {
    instance.loginRedirect({
      scopes,
    });
  };

  const isLoggedIn = accounts.length > 0;

  function handleClick() {
    if (isLoggedIn) {
      instance.logoutRedirect();
    } else {
      handleLogin();
    }
  }

  return (
    <button
      onClick={handleClick}
      className="flex gap-2 items-center px-5 py-1 bg-white text-black rounded-lg hover:bg-gray-100 cursor-pointer transition-colors duration-200 font-medium"
    >
      {isLoggedIn ? <LogOut size={20} /> : <LogIn size={20} />}
      {isLoggedIn ? "Logout" : "Login"}
    </button>
  );
}
