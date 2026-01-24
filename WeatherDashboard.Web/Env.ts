const Env = {
  API_BASE_URL: import.meta.env.VITE_API_URL || "",
  AZURE_CLIENT_ID: import.meta.env.VITE_AZURE_CLIENT_ID || "",
  AZURE_TENANT_ID: import.meta.env.VITE_AZURE_TENANT_ID || "",
  AZURE_API_SCOPE_ID: import.meta.env.VITE_AZURE_API_SCOPE_ID || "",
};

export default Env;
