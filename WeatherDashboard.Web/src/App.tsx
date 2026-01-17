import { useState } from "react";
import { Card } from "./components/Card";
import { SearchBar } from "./components/SearchBar";
import { Tag } from "./components/Tag";
import {
  Wind,
  Droplet,
  Cloud,
  CloudRain,
  CloudSnow,
  Sun,
  Star,
} from "lucide-react";
import { Insights } from "./components/Insights";
import { Forecast } from "./components/Forecast";

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

type WeatherData = {
  location: string;
  condition: string;
  description: string;
  windSpeed: number;
  humidity: number;
  temperature: number;
};

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

        <div className="flex gap-2 justify-center mt-2 mb-10">
          {favouritedLocations.map((location) => (
            <Tag
              key={location}
              label={location}
              onClick={() =>
                setCurrentWeather({
                  location,
                  ...MOCK_WEATHER_DATA[
                    location as keyof typeof MOCK_WEATHER_DATA
                  ],
                })
              }
            />
          ))}
        </div>

        <div className="flex flex-col gap-2 w-full max-w-3xl mx-auto">
          <div className="w-full mx-auto">
            <Card>
              <div className="flex justify-between relative">
                <button
                  className="absolute top-0 right-0"
                  aria-label="Favourite"
                >
                  <Star size={40} />
                </button>
                <div>
                  <h2 className="text-4xl font-semibold mb-2">
                    {currentWeather.location}
                  </h2>
                  <p className="text-lg mb-4">{currentWeather.description}</p>
                  <div className="flex gap-4">
                    <div className="flex flex-col gap-2">
                      <Tag
                        label={
                          <div className="flex gap-2 items-center font-bold">
                            <Wind /> {currentWeather.windSpeed} km/h
                          </div>
                        }
                      />
                      <Tag
                        label={
                          <div className="flex gap-2 items-center font-bold">
                            <Droplet /> {currentWeather.humidity}%
                          </div>
                        }
                      />
                    </div>
                  </div>
                </div>

                <div className="pr-10 text-6xl font-bold flex flex-col items-center justify-center gap-2">
                  {currentWeather.condition === "sunny" && <Sun size={100} />}
                  {currentWeather.condition === "cloudy" && (
                    <Cloud size={100} />
                  )}
                  {currentWeather.condition === "rainy" && (
                    <CloudRain size={100} />
                  )}
                  {currentWeather.condition === "snowy" && (
                    <CloudSnow size={100} />
                  )}
                  <div>{currentWeather.temperature}°C</div>
                </div>
              </div>
            </Card>
          </div>

          <div className="w-full max-w-3xl mx-auto flex flex-col md:flex-row gap-2">
            <Insights />
            <Forecast />
          </div>
        </div>
      </main>
    </div>
  );
}

export default App;
