import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import Env from "../../Env";
import type {
  ForecastResponse,
  ForecastSummaryResponse,
  WeatherResponse,
} from "../types";

export const weatherApi = createApi({
  reducerPath: "weatherApi",
  baseQuery: fetchBaseQuery({ baseUrl: Env.API_BASE_URL + "/api/" }),
  endpoints: (builder) => ({
    getWeatherByCity: builder.query<WeatherResponse, string>({
      query: (city: string) => `weather/city?city=${encodeURIComponent(city)}`,
    }),
    getForecastByCity: builder.query<ForecastResponse, string>({
      query: (city: string) => `forecast/city?city=${encodeURIComponent(city)}`,
    }),
    getForecastSummary: builder.query<ForecastSummaryResponse, string>({
      query: (city: string) => `forecast/${encodeURIComponent(city)}/summary`,
    }),
  }),
});

export const {
  useGetWeatherByCityQuery,
  useGetForecastByCityQuery,
  useGetForecastSummaryQuery,
} = weatherApi;
