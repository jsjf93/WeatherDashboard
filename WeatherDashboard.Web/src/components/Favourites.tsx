import { Tag } from "./Tag";

type FavouritesProps = {
  favouritedLocations: string[];
  onClick: (location: string) => void;
};

export function Favourites({ favouritedLocations, onClick }: FavouritesProps) {
  return (
    <div className="flex gap-2 justify-center mt-2 mb-4 md:mb-10">
      {favouritedLocations.map((location) => (
        <Tag
          key={location}
          label={location}
          onClick={() => onClick(location)}
        />
      ))}
    </div>
  );
}
