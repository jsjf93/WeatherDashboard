export interface WeatherResponse {
  city: string;
  temperature: number;
  humidity: number;
  wind: number;
  condition: "Clear" | "Clouds" | "Rain" | "Snow";
  description: string;
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
  condition: "Clear" | "Clouds" | "Rain" | "Snow";
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
