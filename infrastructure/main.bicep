param location string = resourceGroup().location
param staticWebAppLocation string = 'westeurope'

module api 'api.bicep' = {
  name: 'WeatherDashboardApi'
  params: {
    location: location
  }
}

module ui 'ui.bicep' = {
  name: 'WeatherDashboardUI'
  params: {
    location: staticWebAppLocation
  }
}