import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import Env from "../../Env";
import { msalInstance } from "../config/auth";
import type {
  ForecastResponse,
  ForecastSummaryResponse,
  WeatherResponse,
} from "../types";
import { apiScope } from "../config/auth";

const baseQuery = fetchBaseQuery({
  baseUrl: Env.API_BASE_URL + "/api/",
  prepareHeaders: async (headers) => {
    try {
      const accounts = msalInstance.getAllAccounts();
      if (accounts.length > 0) {
        const account = accounts[0];
        try {
          const tokenResponse = await msalInstance.acquireTokenSilent({
            scopes: [apiScope],
            account,
          });
          headers.set("Authorization", `Bearer ${tokenResponse.accessToken}`);
        } catch (error) {
          console.warn(
            "Silent token acquisition failed, attempting popup:",
            error,
          );
          const tokenResponse = await msalInstance.acquireTokenPopup({
            scopes: [apiScope],
            account,
          });
          headers.set("Authorization", `Bearer ${tokenResponse.accessToken}`);
        }
      }
    } catch (error) {
      console.error("Failed to acquire token:", error);
    }
    return headers;
  },
});

export const weatherApi = createApi({
  reducerPath: "weatherApi",
  baseQuery,
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
