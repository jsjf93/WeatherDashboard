param location string

resource apiApp 'Microsoft.Web/sites@2023-01-01' = {
  name: 'weatherdashboard-api'
  location: location
  kind: 'app,linux'
  properties: {
    serverFarmId: apiServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'
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
  name: 'weatherdashboard-plan'
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