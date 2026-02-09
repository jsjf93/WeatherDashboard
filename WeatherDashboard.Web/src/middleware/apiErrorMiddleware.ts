import { isRejectedWithValue, type Middleware } from "@reduxjs/toolkit";
import { addToast } from "../features/toast/toastSlice";
import type { FastEndpointsError } from "../types";

/**
 * Middleware that listens for RTK Query rejected actions and displays toast notifications
 * for API errors. Handles both FastEndpoints error responses and network failures.
 */
export const apiErrorMiddleware: Middleware = (store) => (next) => (action) => {
  // Check if this is a rejected RTK Query action
  if (isRejectedWithValue(action)) {
    const { payload } = action;

    let errorMessage = "An unexpected error occurred. Please try again.";

    // Handle FastEndpoints error response structure
    if (payload && typeof payload === "object" && "data" in payload) {
      const data = payload.data as Partial<FastEndpointsError>;

      if (data.errors && typeof data.errors === "object") {
        // Extract first error message from FastEndpoints errors object
        const errorMessages = Object.values(data.errors)
          .flat()
          .filter((msg): msg is string => typeof msg === "string");

        if (errorMessages.length > 0) {
          errorMessage = errorMessages[0];
        }
      }
    }
    // Handle network errors or other failures
    else if (payload && typeof payload === "object" && "error" in payload) {
      const error = payload.error as string;
      if (error === "FETCH_ERROR" || error.includes("fetch")) {
        errorMessage =
          "Unable to connect to the server. Please check your connection.";
      } else if (error === "TIMEOUT_ERROR") {
        errorMessage = "Request timed out. Please try again.";
      } else if (error === "PARSING_ERROR") {
        errorMessage = "Unable to process server response.";
      }
    }
    // Handle status-specific messages
    else if (payload && typeof payload === "object" && "status" in payload) {
      const status = payload.status as number;
      if (status === 404) {
        errorMessage = "Resource not found.";
      } else if (status === 401 || status === 403) {
        errorMessage = "You are not authorized to perform this action.";
      } else if (status === 409) {
        errorMessage = "This operation conflicts with existing data.";
      } else if (status >= 500) {
        errorMessage = "Server error. Please try again later.";
      }
    }

    store.dispatch(
      addToast({
        message: errorMessage,
        variant: "error",
      }),
    );
  }

  return next(action);
};
