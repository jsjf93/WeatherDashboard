import { useEffect } from "react";
import { SearchBar } from "./components/SearchBar";
import { Insights } from "./components/Insights";
import { Forecast } from "./components/Forecast";
import { LocationWeather } from "./components/LocationWeather.tsx";
import { Header } from "./components/Header.tsx";
import { Favourites } from "./components/Favourites.tsx";
import { ToastContainer } from "./components/ToastContainer.tsx";
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
import { useCurrentTime } from "./hooks/useCurrentTime";

const THEME_CLASSES = ['theme-Clear', 'theme-Rain', 'theme-Clouds', 'theme-Snow', 'theme-night'] as const;
const DEFAULT_THEME = 'theme-Clouds';
const VALID_CONDITIONS = ['Clear', 'Rain', 'Clouds', 'Snow'] as const;

function App() {
  const isAuthenticated = useIsAuthenticated();
  const location = useAppSelector(selectCurrentLocation);
  const dispatch = useAppDispatch();
  const currentTime = useCurrentTime();

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

  const { data: favouritesData, isLoading: isFavouritesLoading } =
    useGetFavouritesQuery(undefined, {
      skip: !isAuthenticated,
    });
  const [addFavourite] = useAddFavouriteMutation();
  const [removeFavourite] = useRemoveFavouriteMutation();

  const handleAddFavourite = async (location: string) => {
    if (!location) return;

    try {
      await addFavourite({ city: location }).unwrap();
    } catch {
      // Error handled by apiErrorMiddleware
    }
  };

  async function handleRemoveFavourite(favouriteId?: string) {
    if (!favouriteId) return;

    try {
      await removeFavourite(favouriteId).unwrap();
    } catch {
      // Error handled by apiErrorMiddleware
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

  // Apply theme class to body element so CSS variables are available to html/body/#root
  useEffect(() => {
    // Remove all theme classes
    document.body.classList.remove(...THEME_CLASSES);

    // Determine and add the appropriate theme class
    if (!currentWeather) {
      document.body.classList.add(DEFAULT_THEME);
    } else {
      // Determine if it's nighttime based on sunrise and sunset
      // All values are Unix timestamps in UTC, so we can compare directly
      const isNight =
        currentTime < currentWeather.sunrise ||
        currentTime >= currentWeather.sunset;

      if (isNight) {
        document.body.classList.add("theme-night");
      } else {
        // Validate condition against known theme classes
        const condition = currentWeather.condition;
        const themeClass = VALID_CONDITIONS.includes(condition as typeof VALID_CONDITIONS[number])
          ? `theme-${condition}`
          : DEFAULT_THEME;
        document.body.classList.add(themeClass);
      }
    }

    // Cleanup function to remove all theme classes on unmount or before next effect
    return () => {
      document.body.classList.remove(...THEME_CLASSES);
    };
  }, [currentWeather, currentTime]);

  return (
    <>
      <ToastContainer />
      <div className="min-h-screen flex flex-col p-4 md:p-10 grow">
        <Header />

        <main className="flex flex-col gap-4 md:gap-5">
          <SearchBar />

          <Favourites
            favouritedLocations={favouritesData?.favourites}
            onClick={(location) => dispatch(setCurrentLocation(location))}
            isLoading={isFavouritesLoading && isAuthenticated}
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
                showLoading={isFetching || isLoading}
              />
              <Forecast
                forecastData={forecastWeather?.dailySummaries}
                isLoading={isForecastFetching && !!location}
              />
            </div>
          </div>
        </main>
      </div>
    </>
  );
}

export default App;
