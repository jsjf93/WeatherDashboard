import { useState } from "react";
import { Sparkles } from "lucide-react";
import { Card } from "./Card";
import { useGetForecastSummaryQuery } from "../services/weatherApi";

interface InsightsProps {
  city?: string;
  isForecastReady?: boolean;
  showLoading?: boolean;
}

export function Insights({
  city,
  isForecastReady,
  showLoading,
}: InsightsProps) {
  const [showFullInsights, setShowFullInsights] = useState(false);

  const {
    data: summary,
    isFetching,
    error,
  } = useGetForecastSummaryQuery(city!, {
    skip: !city || !isForecastReady,
  });

  return (
    <div className="flex-1">
      <Card>
        <div className="flex flex-col min-h-36 items-start">
          <h2 className="text-xs font-semibold mb-2 uppercase">
            <span className="flex items-center gap-2">
              <Sparkles /> AI Insights
            </span>
          </h2>

          {showLoading && <InsightsSkeleton />}

          {!showLoading && !city ? (
            <p className="w-full text-lg text-gray-500">
              Search for a city to see AI-powered weather insights.
            </p>
          ) : isFetching ? (
            <InsightsSkeleton />
          ) : error ? (
            <p className="w-full text-lg text-red-500">
              Unable to generate insights. Please try again later.
            </p>
          ) : summary ? (
            <>
              <p
                className={`w-full text-lg ${!showFullInsights ? "line-clamp-3" : ""}`}
              >
                {summary.summary}
              </p>
              {summary.summary.length > 150 && (
                <button
                  onClick={() => setShowFullInsights(!showFullInsights)}
                  className="text-sm mt-2 underline hover:no-underline"
                >
                  {showFullInsights ? "Show less" : "Show more"}
                </button>
              )}
            </>
          ) : null}
        </div>
      </Card>
    </div>
  );
}

function InsightsSkeleton() {
  return (
    <div className="w-full text-lg">
      <div className="animate-pulse space-y-2">
        <div className="h-4 bg-gray-300 rounded w-full"></div>
        <div className="h-4 bg-gray-300 rounded w-5/6"></div>
        <div className="h-4 bg-gray-300 rounded w-4/6"></div>
      </div>
    </div>
  );
}
