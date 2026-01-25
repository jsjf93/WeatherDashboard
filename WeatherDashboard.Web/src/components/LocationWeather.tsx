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
  currentWeather?: WeatherResponse;
  isFavourited?: boolean;
  isLoading: boolean;
  onAddFavourite: (city: string) => void;
  onRemoveFavourite: () => void;
}

export function LocationWeather({
  currentWeather,
  isFavourited,
  isLoading,
  onAddFavourite,
  onRemoveFavourite,
}: LocationWeatherProps) {
  const isAuthenticated = useIsAuthenticated();

  function onClick() {
    if (!currentWeather) return;

    if (isFavourited) {
      onRemoveFavourite();
    } else {
      onAddFavourite(currentWeather.city);
    }
  }

  return (
    <div className="w-full mx-auto">
      <Card>
        {isLoading ? (
          <div className="animate-pulse flex flex-col gap-4">
            <h2 className="sr-only">Current Weather</h2>
            <div className="h-8 bg-gray-300 rounded w-1/3"></div>
            <div className="h-6 bg-gray-300 rounded w-1/2"></div>
            <div className="h-20 bg-gray-300 rounded w-full"></div>
          </div>
        ) : !currentWeather ? (
          <div className="flex flex-col min-h-36">
            <h2 className="text-xs font-semibold mb-2 uppercase">
              Current Weather
            </h2>
            <p className="w-full text-lg text-gray-500">
              Search for a city to show the current weather
            </p>
          </div>
        ) : (
          <div className="flex justify-between relative">
            <h2 className="sr-only">Current Weather</h2>
            <button
              className="absolute top-0 right-0 cursor-pointer hover:brightness-90 transition disabled:cursor-not-allowed"
              aria-label={`Add ${currentWeather.city} to your favourites`}
              disabled={!isAuthenticated}
              // Add a proper tooltip and maybe a lock icon at some point
              title={
                isAuthenticated
                  ? isFavourited
                    ? `Remove ${currentWeather.city} from your favourites`
                    : `Add ${currentWeather.city} to your favourites`
                  : "Login to favourite locations"
              }
              onClick={onClick}
            >
              <Star size={40} fill={isFavourited ? "yellow" : "none"} />
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
                        <Wind /> {currentWeather.wind} km/h
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
              <div>{Math.round(currentWeather.temperature)}°C</div>
            </div>
          </div>
        )}
      </Card>
    </div>
  );
}
