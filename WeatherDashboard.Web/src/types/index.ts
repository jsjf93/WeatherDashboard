export interface WeatherResponse {
  city: string;
  temperature: number;
  humidity: number;
  wind: number;
  condition: "Clear" | "Clouds" | "Rain" | "Snow" | "Drizzle" | "Thunderstorm" | "Mist" | "Smoke" | "Haze" | "Dust" | "Fog" | "Sand" | "Ash" | "Squall" | "Tornado";
  description: string;
  minTemperature: number;
  maxTemperature: number;
  sunrise: number;
  sunset: number;
  timezone: number;
}

export interface ForecastResponse {
  dailySummaries: DailyForecast[];
}

export interface DailyForecast {
  date: string;
  icon: string;
  temp: number;
  minTemp: number;
  maxTemp: number;
  condition: "Clear" | "Clouds" | "Rain" | "Snow" | "Drizzle" | "Thunderstorm" | "Mist" | "Smoke" | "Haze" | "Dust" | "Fog" | "Sand" | "Ash" | "Squall" | "Tornado";
  feelsLike: number;
  humidity: number;
  windSpeed: number;
  description: string;
}

export interface ForecastItem {
  dt: number;
  dtTxt: string;
  temp: number;
  feelsLike: number;
  tempMin: number;
  tempMax: number;
  pressure: number;
  humidity: number;
  condition: string;
  description: string;
  icon: string;
  clouds: number;
  windSpeed: number;
  windDeg: number;
  pop: number;
  rain3h?: number;
  snow3h?: number;
}

export interface ForecastSummaryResponse {
  summary: string;
}

export interface Favourite {
  id: string;
  city: string;
  createdAt: string;
}

export interface GetFavouritesResponse {
  favourites: Favourite[];
}

export interface AddFavouriteRequest {
  city: string;
}

export interface AddFavouriteResponse {
  id: string;
  city: string;
  createdAt: string;
}

export interface FastEndpointsError {
  errors: {
    [fieldName: string]: string[];
  };
  statusCode: number;
}

// Toast notification types
export type ToastVariant = "error" | "success" | "info" | "warning";

export interface Toast {
  id: string;
  message: string;
  variant: ToastVariant;
  timestamp: number;
}
