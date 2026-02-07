import type { Favourite } from "../types";
import { Tag } from "./Tag";

type FavouritesProps = {
  favouritedLocations?: Favourite[];
  onClick: (location: string) => void;
};

export function Favourites({ favouritedLocations, onClick }: FavouritesProps) {
  return (
    <div className="flex flex-wrap gap-2 justify-center">
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
