# Welcome to CosmosDb.QuickStart

## Topic: Programatically connect to desired location using Cosmos Db .NET SDK for SQL API

Hi All!  
I put together a sample quick start to help you all in customizing your Cosmos Db connection logic using **Microsoft.Azure.Documents.Client.ConnectionPolicy**. 

This example is based on my previous sample souce code about setting Connection Mode and Protocol. You can have a look at source in github: https://github.com/AzureContrib/CosmosDB-DotNet-Quickstart-With-ConnectionPolicy

Changes made on this sample covers: 

1. *Upgrade Microsoft.Azure.DocumentDb nuget package to Microsoft.Azure.DocumentDb version 1.22.0*
2. *Included relavant logic to defined prefered locations as part of connection policy*

## Prerequisites 
1. Windows 7/8.x/10 with Visual Studio 2015/2017 with .NET Framework 4.5
2. Nuget Package Manager 
3. Setup a CosmosDb environment in Azure or use Cosmos Db Emulator (on Windows 10).
## How to use the application?

1. Open **CosmosDb-QuickStart.sln**  
2. Do a Nuget package **restore**.
3. **Update** the Web.config with your cosmosDb URL and authKey. 
4. Launch the solution in **debug** mode. 

## Important Files to Consider 

**Web.config** - All the configuration for the connection policy are driven from Web.config-> appSettings. 

**CosmosDbRepository.cs** - Added the following line of code to Initialize() method, right after connectionPolicy object. 

       //Setting read region selection preference
       connectionPolicy.PreferredLocations.Add(LocationNames.EastUS); // applications first preference
       connectionPolicy.PreferredLocations.Add(LocationNames.WestEurope); // applications second preference

       client = new DocumentClient(new Uri(Constants.endpoint), Constants.authKey, connectionPolicy);

> **ProTip:** Source code is provided as is based on  **CosmosDb Basic ToDo starter application**  and I customized it to showcase the **ConnectionPolicy** implementation.

> **More Reads:** If you want to learn about ConnectionPolicy class, you can read [here](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.client.connectionpolicy?view=azure-dotnet). 