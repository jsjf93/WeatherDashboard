import { useState } from "react";
import { Sparkles } from "lucide-react";
import { Card } from "./Card";

export function Insights() {
  const [showFullInsights, setShowFullInsights] = useState(false);

  return (
    <div className="flex-1">
      <Card>
        <div className="flex flex-col min-h-36 items-start">
          <h2 className="text-xs font-semibold mb-2 uppercase">
            <span className="flex items-center gap-2">
              <Sparkles /> AI Insights
            </span>
          </h2>
          <p
            className={`w-full text-lg ${!showFullInsights ? "line-clamp-3" : ""}`}
          >
            Get personalized weather insights powered by AI. This is a test to
            check that it wraps correctly. Enjoy tailored advice for your daily
            activities based on the latest weather data. Stay ahead of changing
            conditions with our smart recommendations. Whether you're planning
            outdoor adventures or daily commutes, our AI insights have got you
            covered.
          </p>
          <button
            onClick={() => setShowFullInsights(!showFullInsights)}
            className="text-sm mt-2 underline hover:no-underline"
          >
            {showFullInsights ? "Show less" : "Show more"}
          </button>
        </div>
      </Card>
    </div>
  );
}
