import type { Favourite } from "../types";
import { Tag } from "./Tag";

type FavouritesProps = {
  favouritedLocations?: Favourite[];
  onClick: (location: string) => void;
  isLoading?: boolean;
};

function SkeletonTag() {
  return (
    <div className="rounded-full glass px-3 py-1 h-7 w-24 animate-pulse bg-gray-300"></div>
  );
}

export function Favourites({
  favouritedLocations,
  onClick,
  isLoading,
}: FavouritesProps) {
  // Show skeleton loaders while loading
  if (isLoading) {
    return (
      <div className="flex flex-wrap gap-2 justify-center">
        {Array.from({ length: 3 }).map((_, index) => (
          <SkeletonTag key={index} />
        ))}
      </div>
    );
  }

  // Show empty state message if no favourites
  if (!favouritedLocations || favouritedLocations.length === 0) {
    return (
      <div className="flex flex-wrap gap-2 justify-center">
        <p className="text-gray-500 text-sm">
          Search for a location and favourite it for later
        </p>
      </div>
    );
  }

  // Show favourites
  return (
    <div className="flex flex-wrap gap-2 justify-center">
      {favouritedLocations.map((location) => (
        <Tag
          key={location.id}
          label={location.city}
          onClick={() => onClick(location.city)}
        />
      ))}
    </div>
  );
}
