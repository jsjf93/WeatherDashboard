export interface WeatherResponse {
  city: string;
  temperature: number;
  humidity: number;
  windSpeed: number;
  condition: "Clear" | "Clouds" | "Rain" | "Snow";
  description: string;
}
