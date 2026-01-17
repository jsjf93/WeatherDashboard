import { useMsal } from "@azure/msal-react";

export function AuthButton() {
  const { instance, accounts } = useMsal();

  const handleLogin = () => {
    instance.loginRedirect({
      scopes: ["openid", "profile", "email"],
    });
  };

  if (accounts.length > 0) {
    return (
      <button
        onClick={() => instance.logoutRedirect()}
        className="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded-md transition-colors duration-200 text-sm font-medium"
      >
        Logout
      </button>
    );
  }

  return (
    <button
      onClick={handleLogin}
      className="px-6 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md transition-colors duration-200 font-medium"
    >
      Login
    </button>
  );
}
