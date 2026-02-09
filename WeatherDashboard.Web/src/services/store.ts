import { configureStore } from "@reduxjs/toolkit";
import { weatherApi } from "./weatherApi";
import locationReducer from "../features/location/locationSlice";
import toastReducer from "../features/toast/toastSlice";
import { apiErrorMiddleware } from "../middleware/apiErrorMiddleware";

export const store = configureStore({
  reducer: {
    [weatherApi.reducerPath]: weatherApi.reducer,
    location: locationReducer,
    toast: toastReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware()
      .concat(weatherApi.middleware)
      .concat(apiErrorMiddleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
