import {
  Star,
  Wind,
  Droplet,
  Sun,
  Cloud,
  CloudRain,
  CloudSnow,
  Moon,
  ArrowUp,
  ArrowDown,
  Lock,
  MapPin,
} from "lucide-react";
import { Card } from "./Card";
import type { WeatherResponse } from "../types";
import { Tag } from "./Tag.tsx";
import { useIsAuthenticated } from "@azure/msal-react";
import { useCurrentTime } from "../hooks/useCurrentTime";

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
  const currentTime = useCurrentTime();

  function onClick() {
    if (!currentWeather) return;

    if (isFavourited) {
      onRemoveFavourite();
    } else {
      onAddFavourite(currentWeather.city);
    }
  }

  const isNight = (() => {
    if (!currentWeather) return false;
    
    // All values are Unix timestamps in UTC, so we can compare directly
    return currentTime < currentWeather.sunrise || currentTime >= currentWeather.sunset;
  })();

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
              <span className="flex items-center gap-2">
                <MapPin /> Current Weather
              </span>
            </h2>
            <p className="w-full text-lg text-gray-500">
              Search for a city to show the current weather
            </p>
          </div>
        ) : (
          <div className="flex items-center justify-between relative">
            <h2 className="sr-only">Current Weather</h2>
            <button
              className="absolute top-0 right-0 cursor-pointer hover:brightness-90 transition disabled:cursor-not-allowed"
              aria-label={`Add ${currentWeather.city} to your favourites`}
              disabled={!isAuthenticated}
              title={
                isAuthenticated
                  ? isFavourited
                    ? `Remove ${currentWeather.city} from your favourites`
                    : `Add ${currentWeather.city} to your favourites`
                  : "You must be signed in to favourite a location"
              }
              onClick={onClick}
            >
              <div className="relative">
                <Star size={40} fill={isFavourited ? "yellow" : "none"} />
                {!isAuthenticated && (
                  <Lock size={20} className="absolute -top-2 -right-2 p-0.5" />
                )}
              </div>
            </button>
            <div>
              <h2 className="text-4xl font-semibold mb-2">
                {currentWeather.city}
              </h2>
              <p className="text-lg mb-4">{currentWeather.description}</p>
              <div className="flex gap-4">
                <div className="flex flex-col md:flex-row gap-2 flex-wrap items-start">
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
                        <span className="text-blue-400">
                          <Droplet />
                        </span>{" "}
                        {currentWeather.humidity}%
                      </div>
                    }
                  />
                  <Tag
                    label={
                      <div className="flex gap-2 items-center font-bold">
                        <span className="flex gap-1 items-center">
                          <span className="text-blue-400">
                            <ArrowDown />
                          </span>{" "}
                          {Math.round(currentWeather.minTemperature)}°C
                        </span>
                        <span className="flex gap-1 items-center">
                          <span className="text-red-400">
                            <ArrowUp />
                          </span>{" "}
                          {Math.round(currentWeather.maxTemperature)}
                          °C
                        </span>
                      </div>
                    }
                  />
                </div>
              </div>
            </div>

            <div className="pr-10 text-5xl sm:text-6xl font-bold flex flex-col items-center justify-center gap-1 sm:gap-2">
              {currentWeather.condition === "Clear" && isNight && <Moon size={100} />}
              {currentWeather.condition === "Clear" && !isNight && <Sun size={100} />}
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
