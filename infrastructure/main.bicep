param location string = resourceGroup().location

module api 'api.bicep' = {
  name: 'WeatherDashboardApi'
  params: {
    location: location
  }
}

module ui 'ui.bicep' = {
  name: 'WeatherDashboardUI'
  params: {
    location: location
  }
}