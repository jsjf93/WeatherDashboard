import {
  Star,
  Wind,
  Droplet,
  Sun,
  Cloud,
  CloudRain,
  CloudSnow,
} from "lucide-react";
import { Card } from "./Card";
import type { WeatherData } from "../types.ts";
import { Tag } from "./Tag.tsx";
import { useIsAuthenticated } from "@azure/msal-react";

interface LocationWeatherProps {
  currentWeather: WeatherData;
}

export function LocationWeather({ currentWeather }: LocationWeatherProps) {
  const isAuthenticated = useIsAuthenticated();

  return (
    <div className="w-full mx-auto">
      <Card>
        <div className="flex justify-between relative">
          <button
            className="absolute top-0 right-0 cursor-pointer hover:brightness-90 transition disabled:cursor-not-allowed"
            aria-label={`Add ${currentWeather.location} to your favourites`}
            disabled={!isAuthenticated}
            // Add a proper tooltip and maybe a lock icon at some point
            title={
              isAuthenticated ? "Favourite" : "Login to favourite locations"
            }
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
            {currentWeather.condition === "cloudy" && <Cloud size={100} />}
            {currentWeather.condition === "rainy" && <CloudRain size={100} />}
            {currentWeather.condition === "snowy" && <CloudSnow size={100} />}
            <div>{currentWeather.temperature}°C</div>
          </div>
        </div>
      </Card>
    </div>
  );
}
