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
import type { WeatherResponse } from "../types";
import { Tag } from "./Tag.tsx";
import { useIsAuthenticated } from "@azure/msal-react";

interface LocationWeatherProps {
  currentWeather: WeatherResponse;
  isLoading: boolean;
}

export function LocationWeather({
  currentWeather,
  isLoading,
}: LocationWeatherProps) {
  const isAuthenticated = useIsAuthenticated();

  if (isLoading) {
    return (
      <div className="w-full mx-auto">
        <Card>
          <div className="animate-pulse flex flex-col gap-4">
            <div className="h-8 bg-gray-300 rounded w-1/3"></div>
            <div className="h-6 bg-gray-300 rounded w-1/2"></div>
            <div className="h-20 bg-gray-300 rounded w-full"></div>
          </div>
        </Card>
      </div>
    );
  }

  return (
    <div className="w-full mx-auto">
      <Card>
        <div className="flex justify-between relative">
          <button
            className="absolute top-0 right-0 cursor-pointer hover:brightness-90 transition disabled:cursor-not-allowed"
            aria-label={`Add ${currentWeather.city} to your favourites`}
            disabled={!isAuthenticated}
            // Add a proper tooltip and maybe a lock icon at some point
            title={
              isAuthenticated
                ? `Add ${currentWeather.city} to your favourites`
                : "Login to favourite locations"
            }
          >
            <Star size={40} />
          </button>
          <div>
            <h2 className="text-4xl font-semibold mb-2">
              {currentWeather.city}
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
            {currentWeather.condition === "Clear" && <Sun size={100} />}
            {currentWeather.condition === "Clouds" && <Cloud size={100} />}
            {currentWeather.condition === "Rain" && <CloudRain size={100} />}
            {currentWeather.condition === "Snow" && <CloudSnow size={100} />}
            <div>{currentWeather.temperature.toFixed(0)}°C</div>
          </div>
        </div>
      </Card>
    </div>
  );
}
