import { useState } from "react";
import { SearchBar } from "./components/SearchBar";
import { Insights } from "./components/Insights";
import { Forecast } from "./components/Forecast";
import type { WeatherData } from "./types.ts";
import { LocationWeather } from "./components/LocationWeather.tsx";
import { Favourites } from "./components/Favourites.tsx";

const MOCK_WEATHER_DATA = {
  London: {
    condition: "sunny",
    description: "Clear sky",
    windSpeed: 10,
    humidity: 60,
    temperature: 15,
  },
  "New York": {
    condition: "cloudy",
    description: "Overcast clouds",
    windSpeed: 8,
    humidity: 70,
    temperature: 20,
  },
  Lisbon: {
    condition: "rainy",
    description: "Light rain",
    windSpeed: 12,
    humidity: 80,
    temperature: 18,
  },
  Madrid: {
    condition: "snowy",
    description: "Snow showers",
    windSpeed: 5,
    humidity: 90,
    temperature: 2,
  },
};

const favouritedLocations = Object.keys(MOCK_WEATHER_DATA);

function App() {
  const [currentWeather, setCurrentWeather] = useState<WeatherData>({
    location: "London",
    ...MOCK_WEATHER_DATA["London"],
  });

  const getThemeColors = () => {
    if (!currentWeather) return "theme-cloudy";
    return `theme-${currentWeather.condition}`;
  };

  return (
    <div
      className={`min-h-screen p-4 md:p-10 ${getThemeColors()}`}
      style={{ background: "var(--bg-gradient)" }}
    >
      <header className="flex flex-col items-center py-10">
        <h1 className="text-2xl text-shadow-subtle">Weather Dashboard</h1>
        <p className="text-sm text-shadow-subtle">
          Accurate forecasts, wherever you are
        </p>
      </header>

      <main>
        <SearchBar />

        <Favourites
          favouritedLocations={favouritedLocations}
          onClick={(location) =>
            setCurrentWeather({
              location,
              ...MOCK_WEATHER_DATA[location as keyof typeof MOCK_WEATHER_DATA],
            })
          }
        />

        <div className="flex flex-col gap-2 md:gap-4 w-full max-w-3xl mx-auto">
          <LocationWeather currentWeather={currentWeather} />

          <div className="w-full max-w-3xl mx-auto flex flex-col md:flex-row gap-2 md:gap-4">
            <Insights />
            <Forecast />
          </div>
        </div>
      </main>
    </div>
  );
}

export default App;
