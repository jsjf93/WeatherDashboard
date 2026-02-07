import { Sun, Cloud, CloudRain, CloudSnow } from "lucide-react";
import { Card } from "./Card";
import type { ForecastResponse, DailyForecast } from "../types";
import dayjs from "dayjs";

interface ForecastProps {
  forecastData?: ForecastResponse["dailySummaries"];
  isLoading?: boolean;
}

const getWeatherIcon = (condition: string) => {
  switch (condition) {
    case "Clear":
      return Sun;
    case "Rain":
      return CloudRain;
    case "Snow":
      return CloudSnow;
    case "Clouds":
    default:
      return Cloud;
  }
};

const getDayLabel = (dateStr: string, index: number): string => {
  if (index === 0) return "Today";
  if (index === 1) return "Tomorrow";
  return dayjs(dateStr).format("dddd");
};

export function Forecast({ forecastData, isLoading }: ForecastProps) {
  const displayData = forecastData?.slice(0, 3) || [];

  return (
    <div className="flex-none w-full md:w-55">
      <Card>
        <div className="flex flex-col gap-3 md:min-h-36">
          <h2 className="text-xs font-semibold uppercase">Forecast</h2>
          {isLoading ? (
            // Skeleton loaders for forecast items
            Array.from({ length: 3 }).map((_, index) => (
              <div
                key={index}
                className="flex items-center justify-between animate-pulse"
              >
                <div className="h-4 bg-gray-300 rounded w-20"></div>
                <div className="flex items-center gap-2">
                  <div className="h-6 w-6 bg-gray-300 rounded"></div>
                  <div className="h-4 bg-gray-300 rounded w-12"></div>
                </div>
              </div>
            ))
          ) : (
            displayData.map((forecast: DailyForecast, index: number) => {
              const Icon = getWeatherIcon(forecast.condition);
              return (
                <div
                  key={forecast.date}
                  className="flex items-center whitespace-nowrap justify-between"
                >
                  <span className="w-20">
                    {getDayLabel(forecast.date, index)}
                  </span>
                  <div className="flex items-center gap-2">
                    <Icon size={24} className="shrink-0" />
                    <span className="font-semibold w-12">
                      {Math.round(forecast.temp)}°C
                    </span>
                  </div>
                </div>
              );
            })
          )}
        </div>
      </Card>
    </div>
  );
}
