import { Sun, Cloud } from "lucide-react";
import { Card } from "./Card";

export function Forecast() {
  return (
    <div className="flex-none w-full md:w-55">
      <Card>
        <div className="flex flex-col gap-3 md:min-h-36">
          <h2 className="text-xs font-semibold uppercase">Forecast</h2>
          <div className="flex items-center whitespace-nowrap justify-between">
            <span className="w-20">Today</span>
            <div className="flex items-center gap-2">
              <Sun size={24} className="shrink-0" />
              <span className="font-semibold w-12">17°C</span>
            </div>
          </div>
          <div className="flex items-center whitespace-nowrap justify-between">
            <span className="w-20">Tomorrow</span>
            <div className="flex items-center gap-2">
              <Cloud size={24} className="shrink-0" />
              <span className="font-semibold w-12">19°C</span>
            </div>
          </div>
          <div className="flex items-center whitespace-nowrap justify-between">
            <span className="w-20">Wednesday</span>
            <div className="flex items-center gap-2">
              <Sun size={24} className="shrink-0" />
              <span className="font-semibold w-12">20°C</span>
            </div>
          </div>
        </div>
      </Card>
    </div>
  );
}
