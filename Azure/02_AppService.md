# Erstellen eines App Service

## Erstellen einer webapi von der Konsole aus

Neue WebAPI Projekte können auch von der Konsole aus erstellt werden, indem ein neuer Ordner (z. B. *AzureDemo*)
erstellt wird. In diesem Ordner werden dann folgende Befehle ausgeführt. Der Connection String muss
allerdings noch an die eigenen Einstellungen angepasst werden.

### Erstellen einer WebAPI mit SQL Server in Azure

```text
dotnet new webapi
dotnet tool update --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet ef dbcontext scaffold "Server=aaaaa.database.windows.net;Database=bbbbb;User id=ccccc;Password=ddddd" Microsoft.EntityFrameworkCore.SqlServer --output-dir Model --use-database-names --force --data-annotations
```

Beim Verbindungsstring von scaffold sind folgende Dinge anzupassen:

- *aaaaa.database.windows.net*: Durch den Servernamen des SQL Servers auf Azure zu ersetzen
- *Database=bbbbb*: Durch den Datenbanknamen auf Azure zu ersetzen
- *User id=ccccc*:  Benutzername unseres App Users, der im vorigen Kapitel erstellt wurde.
- *Password=ddddd*: Passwort des App Users, der im vorigen Kapitel erstellt wurde.

### Erstellen einer WebAPI mit SQLite

```text
dotnet new webapi
dotnet tool update --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet ef dbcontext scaffold "DataSource=aaaa.db" Microsoft.EntityFrameworkCore.Sqlite --output-dir Model --use-database-names --force --data-annotations
```

Beim Verbindungsstring von scaffold sind folgende Dinge anzupassen:

- *DataSource=aaaa.db:* Durch den Datenbanknamen der SQLite Datei zu ersetzen.

## Publishing mit Visual Studio

### launchSettings.json

Folgende Einstellungen hören auf den Standardports für HTTP und HTTPS

```js
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "AzureDemoApp": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://0.0.0.0:443;http://0.0.0.0:80",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

```

### ConfigureServices

Der DbContext wird mit *AddDbContext()* hinzugefügt.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddDbContext<AzureDemoDatabaseContext>();
}
```

### Publishing

Mit der rechten Maustaste kann in Visual Studio beim Projekt der Punkt *Publish* gewählt werden.
Auch hier ist darauf zu achten, dass das niedrigste Paket gewählt wird.

![](vs_publish_settings.png)