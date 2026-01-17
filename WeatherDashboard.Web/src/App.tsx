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
  Sparkles,
} from "lucide-react";

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
  const [showFullInsights, setShowFullInsights] = useState(false);

  const getThemeColors = () => {
    if (!currentWeather) return "theme-cloudy";
    return `theme-${currentWeather.condition}`;
  };

  return (
    <div
      className={`min-h-screen p-4 md:p-10 ${getThemeColors()}`}
      style={{ background: "var(--bg-gradient)" }}
    >
      <div className="flex flex-col items-center py-10">
        <h1 className="text-2xl text-shadow-subtle">Weather Dashboard</h1>
        <p className="text-sm text-shadow-subtle">
          Accurate forecasts, wherever you are
        </p>
      </div>

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
              <button className="absolute top-0 right-0">
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
                {currentWeather.condition === "cloudy" && <Cloud size={100} />}
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

        <div className="w-full max-w-3xl mx-auto flex gap-2">
          <div className="flex-1">
            <Card>
              <div className="flex flex-col min-h-36 items-start">
                <h2 className="text-xs font-semibold mb-2 uppercase">
                  <span className="flex items-center gap-2">
                    <Sparkles /> AI Insights
                  </span>
                </h2>
                <p
                  className={`w-full text-lg ${!showFullInsights ? "line-clamp-3" : ""}`}
                >
                  Get personalized weather insights powered by AI. This is a
                  test to check that it wraps correctly. Enjoy tailored advice
                  for your daily activities based on the latest weather data.
                  Stay ahead of changing conditions with our smart
                  recommendations. Whether you're planning outdoor adventures or
                  daily commutes, our AI insights have got you covered.
                </p>
                <button
                  onClick={() => setShowFullInsights(!showFullInsights)}
                  className="text-sm mt-2 underline hover:no-underline"
                >
                  {showFullInsights ? "Show less" : "Show more"}
                </button>
              </div>
            </Card>
          </div>

          <div className="flex-none w-55">
            <Card>
              <div className="flex flex-col gap-3 min-h-36">
                <h2 className="text-xs font-semibold uppercase">Forecast</h2>
                <div className="flex items-center gap-4 whitespace-nowrap justify-between">
                  <span className="w-20">Today</span>
                  <Sun size={24} className="shrink-0" />
                  <span className="font-semibold">17°C</span>
                </div>
                <div className="flex items-center gap-4 whitespace-nowrap justify-between">
                  <span className="w-20">Tomorrow</span>
                  <Cloud size={24} className="shrink-0" />
                  <span className="font-semibold">19°C</span>
                </div>
                <div className="flex items-center gap-4 whitespace-nowrap justify-between">
                  <span className="w-20">Wednesday</span>
                  <Sun size={24} className="shrink-0" />
                  <span className="font-semibold">20°C</span>
                </div>
              </div>
            </Card>
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
