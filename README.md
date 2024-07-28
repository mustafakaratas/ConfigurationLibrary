# ConfigurationLibrary
***
### This repository includes 3 projects;
>* **ConfigurationReader:** Class library for create nuget package.
>* **MainApi:** Web api for import ConfigurationReader nuget package and test it.
>* **ConfigurationApi:** Crud operations for configurations.

run `dotnet pack --configuration Release` command in ConfigurationReader project directory.

Copy nuget package directory.

Open ide (Rider or Visual Studio).

Add new nuget package source via **Tools > Nuget > Show Nuget Sources** for Rider.

Sample NuGet.Config file;

>       <?xml version="1.0" encoding="utf-8"?>
>           <configuration>
>           <packageSources>
>           <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
>           <add key="local" value="/Users/mustafakaratas/Projects/ConfigurationLibrary/ConfigurationReader/bin/Release/" />
>           </packageSources>
>       </configuration>

Configure connection string to your database connection string in appSettings.Development file;

>       "ConnectionStrings": {
>           "ConfigurationDb": "Server=localhost;Database=master;User Id=SA;Password=Mustafa190;TrustServerCertificate=true;"
>       }

Here is scripts for testing;

>       CREATE TABLE Configurations(
>           Id int primary key identity,
>           Name nvarchar(255) not null,
>           Type nvarchar(255) not null,
>           Value nvarchar(255) not null,
>           IsActive bit not null,
>           ApplicationName nvarchar(255) not null,
>           Timest timestamp not null
>       );
> 
>       INSERT INTO Configurations (Name, Type, Value, IsActive, ApplicationName)
>       Values('SiteName','string','boyner.com.tr',1,'MainApi');
>
>       INSERT INTO Configurations (Name, Type, Value, IsActive, ApplicationName)
>       Values('IsBasketEnabled','bool','true',1,'MainApi2');
>
>       INSERT INTO Configurations (Name, Type, Value, IsActive, ApplicationName)
>       Values('MaxItemCount','int','50',1,'MainApi');