import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import Env from "../../Env";
import { msalInstance } from "../config/auth";
import type {
  ForecastResponse,
  ForecastSummaryResponse,
  WeatherResponse,
  GetFavouritesResponse,
  AddFavouriteRequest,
  AddFavouriteResponse,
  SetDefaultFavouriteRequest,
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
  tagTypes: ["Favourites"],
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
    getFavourites: builder.query<GetFavouritesResponse, void>({
      query: () => "favourites",
      providesTags: ["Favourites"],
    }),
    addFavourite: builder.mutation<AddFavouriteResponse, AddFavouriteRequest>({
      query: (body) => ({
        url: "favourites",
        method: "POST",
        body,
      }),
      async onQueryStarted(_arg, { dispatch, queryFulfilled }) {
        try {
          const { data: newFavourite } = await queryFulfilled;
          dispatch(
            weatherApi.util.updateQueryData(
              "getFavourites",
              undefined,
              (draft) => {
                draft.favourites.push(newFavourite);
              },
            ),
          );
        } catch {
          // Cache should hopefully rollback automatically
        }
      },
    }),
    setDefaultFavourite: builder.mutation<void, SetDefaultFavouriteRequest>({
      query: (body) => ({
        url: `favourites/${body.id}/set-default`,
        method: "PUT",
        body,
      }),
      invalidatesTags: ["Favourites"],
    }),
    removeFavourite: builder.mutation<void, string>({
      query: (id) => ({
        url: `favourites/${id}`,
        method: "DELETE",
      }),
      async onQueryStarted(id, { dispatch, queryFulfilled }) {
        const patchResult = dispatch(
          weatherApi.util.updateQueryData(
            "getFavourites",
            undefined,
            (draft) => {
              draft.favourites = draft.favourites.filter((f) => f.id !== id);
            },
          ),
        );
        try {
          await queryFulfilled;
        } catch {
          patchResult.undo();
        }
      },
    }),
  }),
});

export const {
  useGetWeatherByCityQuery,
  useGetForecastByCityQuery,
  useGetForecastSummaryQuery,
  useGetFavouritesQuery,
  useAddFavouriteMutation,
  useSetDefaultFavouriteMutation,
  useRemoveFavouriteMutation,
} = weatherApi;
