import { AuthButton } from "./AuthButton";

export function Header() {
  return (
    <header className="w-full">
      <div className="px-4 md:px-10 py-4 flex flex-col md:grid md:grid-cols-3 items-center gap-4">
        <div className="hidden md:block" />
        <div className="flex flex-col items-center">
          <h1 className="text-xl font-semibold">Weather Dashboard</h1>
          <p className="text-sm">Stay updated with the latest weather</p>
        </div>

        <div className="flex items-center justify-end gap-4">
          <AuthButton />
        </div>
      </div>
    </header>
  );
}
