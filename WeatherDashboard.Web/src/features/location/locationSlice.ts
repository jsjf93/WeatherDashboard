import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { RootState } from "../../services/store";

interface LocationState {
  currentLocation: string | null;
}

const initialState: LocationState = {
  currentLocation: null,
};

export const locationSlice = createSlice({
  name: "location",
  initialState,
  reducers: {
    setCurrentLocation: (state, action: PayloadAction<string>) => {
      state.currentLocation = action.payload;
    },
    clearCurrentLocation: (state) => {
      state.currentLocation = null;
    },
  },
});

export const { setCurrentLocation, clearCurrentLocation } =
  locationSlice.actions;

export const selectCurrentLocation = (state: RootState) =>
  state.location.currentLocation;

export default locationSlice.reducer;
