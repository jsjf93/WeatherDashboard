param location string

resource apiApp 'Microsoft.Web/sites@2023-01-01' = {
  name: 'WeatherDashboardApi'
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: apiServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
      alwaysOn: true
      cors: {
        allowedOrigins: [
          '*'
        ]
      }
    }
    httpsOnly: true
  }
}

resource apiServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: 'WeatherDashboardApiServicePlan'
  location: location
  kind: 'linux'
  sku: {
    name: 'F1'
    tier: 'Free'
    capacity: 1
  }
  properties: {
    reserved: true
  }
}