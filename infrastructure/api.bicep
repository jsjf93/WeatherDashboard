param location string
param allowedOrigins array
param sqlServerAdminLogin string
@secure()
param sqlServerAdminPassword string

resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: 'weatherdashboard-sql-${uniqueString(resourceGroup().id)}'
  location: location
  properties: {
    administratorLogin: sqlServerAdminLogin
    administratorLoginPassword: sqlServerAdminPassword
    version: '12.0'
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  parent: sqlServer
  name: 'WeatherDashboard'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
  }
}

resource sqlServerFirewallRule 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAllAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource apiApp 'Microsoft.Web/sites@2023-01-01' = {
  name: 'weatherdashboard-api'
  location: location
  kind: 'app'
  properties: {
    serverFarmId: apiServicePlan.id
    siteConfig: {
      windowsFxVersion: 'DOTNET|10.0'
      cors: {
        allowedOrigins: allowedOrigins
      }
      connectionStrings: [
        {
          name: 'DefaultConnection'
          connectionString: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabase.name};Persist Security Info=False;User ID=${sqlServerAdminLogin};Password=${sqlServerAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
          type: 'SQLServer'
        }
      ]
    }
    httpsOnly: true
  }
}

resource apiServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: 'weatherdashboard-plan'
  location: location
  sku: {
    name: 'F1'
    tier: 'Free'
    capacity: 1
  }
}
