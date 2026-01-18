import { PublicClientApplication } from "@azure/msal-browser";
import Env from "../../Env";

const msalConfig = {
  auth: {
    clientId: Env.AZURE_CLIENT_ID,
    authority: `https://login.microsoftonline.com/common`,
    redirectUri: window.location.origin,
  },
  cache: {
    cacheLocation: "sessionStorage",
    storeAuthStateInCookie: false,
  },
};

const msalInstance = new PublicClientApplication(msalConfig);

const scopes = ["openid", "profile", "email"];

export { msalInstance, scopes };
