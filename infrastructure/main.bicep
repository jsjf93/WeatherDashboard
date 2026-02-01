param location string = resourceGroup().location
param sqlServerAdminLogin string
@secure()
param sqlServerAdminPassword string

module api 'api.bicep' = {
  name: 'WeatherDashboardApi'
  params: {
    location: location
    sqlServerAdminLogin: sqlServerAdminLogin
    sqlServerAdminPassword: sqlServerAdminPassword
    allowedOrigins: [
      'https://${ui.outputs.staticWebAppUrl}'
    ]
  }
}

module ui 'ui.bicep' = {
  name: 'WeatherDashboardUI'
  params: {
    location: location
  }
}