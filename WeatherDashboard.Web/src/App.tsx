import { SearchBar } from "./components/SearchBar";
import { Insights } from "./components/Insights";
import { Forecast } from "./components/Forecast";
import { LocationWeather } from "./components/LocationWeather.tsx";
import { Header } from "./components/Header.tsx";
import { Favourites } from "./components/Favourites.tsx";
import {
  useGetForecastByCityQuery,
  useGetWeatherByCityQuery,
  useGetFavouritesQuery,
  useAddFavouriteMutation,
  useRemoveFavouriteMutation,
} from "./services/weatherApi.ts";
import { useAppSelector, useAppDispatch } from "./hooks/useRedux";
import {
  selectCurrentLocation,
  setCurrentLocation,
} from "./features/location/locationSlice";
import { useIsAuthenticated } from "@azure/msal-react";

function App() {
  const isAuthenticated = useIsAuthenticated();
  const location = useAppSelector(selectCurrentLocation);
  const dispatch = useAppDispatch();
  const {
    data: currentWeather,
    isFetching,
    isLoading,
  } = useGetWeatherByCityQuery(location!, {
    skip: !location,
  });

  const {
    data: forecastWeather,
    isSuccess: isForecastSuccess,
    isFetching: isForecastFetching, // We need this so that we don't show the previous search insights
  } = useGetForecastByCityQuery(location!, {
    skip: !location,
  });

  const { data: favouritesData } = useGetFavouritesQuery(undefined, {
    skip: !isAuthenticated,
  });
  const [addFavourite] = useAddFavouriteMutation();
  const [removeFavourite] = useRemoveFavouriteMutation();

  const handleAddFavourite = async (location: string) => {
    if (!location) return;

    try {
      await addFavourite({ city: location }).unwrap();
    } catch (error) {
      console.error("Failed to add favourite:", error);
    }
  };

  async function handleRemoveFavourite(favouriteId?: string) {
    if (!favouriteId) return;

    try {
      await removeFavourite(favouriteId).unwrap();
    } catch (error) {
      console.error("Failed to remove favourite:", error);
    }
  }

  function isLocationFavourited(location?: string) {
    return (
      favouritesData?.favourites?.some(
        (f) => f.city.toLowerCase() === location?.toLowerCase(),
      ) || false
    );
  }

  function getFavouriteId(location?: string) {
    return favouritesData?.favourites?.find(
      (f) => f.city.toLowerCase() === location?.toLowerCase(),
    )?.id;
  }

  const getThemeColors = () => {
    if (!currentWeather) return "theme-Clouds";
    return `theme-${currentWeather.condition}`;
  };

  return (
    <div
      className={`min-h-screen flex flex-col p-4 md:p-10 grow ${getThemeColors()}`}
      style={{ background: "var(--bg-gradient)" }}
    >
      <Header />

      <main className="flex flex-col gap-6 md:gap-10">
        <SearchBar />

        <Favourites
          favouritedLocations={favouritesData?.favourites?.map((f) => f)}
          onClick={(location) => dispatch(setCurrentLocation(location))}
        />

        <div className="flex flex-col gap-2 md:gap-4 w-full max-w-3xl mx-auto">
          <LocationWeather
            currentWeather={currentWeather}
            isFavourited={isLocationFavourited(currentWeather?.city)}
            isLoading={isFetching || isLoading}
            onAddFavourite={handleAddFavourite}
            onRemoveFavourite={() =>
              handleRemoveFavourite(getFavouriteId(currentWeather?.city))
            }
          />

          <div className="w-full max-w-3xl mx-auto flex flex-col md:flex-row gap-2 md:gap-4">
            <Insights
              city={location ?? undefined}
              isForecastReady={isForecastSuccess && !isForecastFetching}
            />
            <Forecast forecastData={forecastWeather?.dailySummaries} />
          </div>
        </div>
      </main>
    </div>
  );
}

export default App;
