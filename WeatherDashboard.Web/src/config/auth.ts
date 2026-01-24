import { PublicClientApplication } from "@azure/msal-browser";
import Env from "../../Env";

const msalConfig = {
  auth: {
    clientId: Env.AZURE_CLIENT_ID,
    authority: `https://login.microsoftonline.com/${Env.AZURE_TENANT_ID}`,
    redirectUri: window.location.origin,
  },
  cache: {
    cacheLocation: "sessionStorage",
    storeAuthStateInCookie: false,
  },
};

const msalInstance = new PublicClientApplication(msalConfig);

const scopes = ["openid", "profile", "email"];
const apiScope = `api://${Env.AZURE_API_SCOPE_ID}/api.access`;

export { msalInstance, scopes, apiScope };
