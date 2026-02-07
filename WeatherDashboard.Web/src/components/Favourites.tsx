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
  if (isLoading) {
    return (
      <div className="flex flex-wrap gap-2 justify-center">
        {Array.from({ length: 3 }).map((_, index) => (
          <SkeletonTag key={index} />
        ))}
      </div>
    );
  }

  if (!favouritedLocations || favouritedLocations.length === 0) {
    return null;
  }

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
