import { SearchBar } from "./components/SearchBar";

type WeatherData = {
  condition: string;
};

function App() {
  const currentWeather: WeatherData = {
    condition: "sunny",
  };

  const getThemeColors = () => {
    if (!currentWeather) return "theme-cloudy";
    return `theme-${currentWeather.condition}`;
  };

  return (
    <div
      className={`min-h-screen ${getThemeColors()}`}
      style={{ background: "var(--bg-gradient)" }}
    >
      <div className="flex flex-col items-center py-10">
        <h1 className="text-2xl text-shadow-subtle">Weather Dashboard</h1>
        <p className="text-sm text-shadow-subtle">
          Accurate forecasts, wherever you are
        </p>
      </div>

      <SearchBar />
    </div>
  );
}

export default App;
