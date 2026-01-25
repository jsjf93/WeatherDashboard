import type { Favourite } from "../types";
import { Tag } from "./Tag";

type FavouritesProps = {
  favouritedLocations?: Favourite[];
  onClick: (location: string) => void;
};

export function Favourites({ favouritedLocations, onClick }: FavouritesProps) {
  return (
    <div className="flex gap-2 justify-center mt-2 mb-4 md:mb-6">
      {favouritedLocations?.map((location) => (
        <Tag
          key={location.id}
          label={location.city}
          onClick={() => onClick(location.city)}
        />
      ))}
    </div>
  );
}
