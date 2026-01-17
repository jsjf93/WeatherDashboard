import { useState } from "react";
import { Search } from "lucide-react";

export function SearchBar() {
  const [query, setQuery] = useState("");

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    console.log("Searching for:", query);
  }

  return (
    <form className="w-full max-w-md mx-auto" onSubmit={handleSubmit}>
      <div className="rounded-lg flex items-center gap-2 relative">
        <Search className="absolute z-10 text-white/70 left-3" />
        <label htmlFor="search-input" className="sr-only">
          Search for a city
        </label>
        <input
          id="search-input"
          type="text"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder="Search for a city..."
          className="w-full px-4 py-2 pl-10 bg-black/40 backdrop-blur-md border border-white/20 rounded-full text-white placeholder:text-white/70 focus:outline-none focus:ring-2 focus:ring-white/40"
        />
        <button type="submit" className="sr-only">
          Search
        </button>
      </div>
    </form>
  );
}
