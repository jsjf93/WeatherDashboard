import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { MsalProvider } from "@azure/msal-react";
import { msalInstance } from "./config/auth.ts";
import { Provider } from "react-redux";
import { store } from "./services/store.ts";
import "./index.css";
import App from "./App.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Provider store={store}>
      <MsalProvider instance={msalInstance}>
        <App />
      </MsalProvider>
    </Provider>
  </StrictMode>,
);
