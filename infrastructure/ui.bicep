param location string

resource staticWebApp 'Microsoft.Web/staticSites@2023-01-01' = {
  name: 'weatherdashboard-ui'
  location: location
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {
    repositoryUrl: 'https://github.com/jsjf93/WeatherDashboard'
    branch: 'main'
    buildProperties: {
      appLocation: 'WeatherDashboard.Web'
      apiLocation: ''
      outputLocation: 'dist'
      nodeVersion: '22'
    }
  }
}

output staticWebAppUrl string = staticWebApp.properties.defaultHostname