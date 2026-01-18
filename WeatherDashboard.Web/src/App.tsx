import { SearchBar } from "./components/SearchBar";
import { Insights } from "./components/Insights";
import { Forecast } from "./components/Forecast";
import { LocationWeather } from "./components/LocationWeather.tsx";
import { Header } from "./components/Header.tsx";
import { useGetWeatherByCityQuery } from "./services/weatherApi.ts";
import { useAppSelector } from "./hooks/useRedux";
import { selectCurrentLocation } from "./features/location/locationSlice";

function App() {
  const location = useAppSelector(selectCurrentLocation);

  const { data: currentWeather } = useGetWeatherByCityQuery(location!, {
    skip: !location,
  });

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

        {/* Removed until the favourites feature is implemented in the backend */}
        {/* <Favourites
          favouritedLocations={favouritedLocations}
          onClick={(location) => dispatch(setCurrentLocation(location))}
        /> */}

        <div className="flex flex-col gap-2 md:gap-4 w-full max-w-3xl mx-auto">
          {currentWeather && (
            <LocationWeather currentWeather={currentWeather} />
          )}

          <div className="w-full max-w-3xl mx-auto flex flex-col md:flex-row gap-2 md:gap-4">
            <Insights />
            <Forecast />
          </div>
        </div>
      </main>
    </div>
  );
}

export default App;
