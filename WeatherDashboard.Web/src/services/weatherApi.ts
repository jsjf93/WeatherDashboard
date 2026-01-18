import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import Env from "../../Env";
import type { WeatherResponse } from "../types";

export const weatherApi = createApi({
  reducerPath: "weatherApi",
  baseQuery: fetchBaseQuery({ baseUrl: Env.API_BASE_URL + "/api/" }),
  endpoints: (builder) => ({
    getWeatherByCity: builder.query<WeatherResponse, string>({
      query: (city: string) => `weather/city?city=${encodeURIComponent(city)}`,
    }),
  }),
});

export const { useGetWeatherByCityQuery } = weatherApi;
