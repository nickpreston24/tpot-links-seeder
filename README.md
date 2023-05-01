## Quickstart

If you wanna do some shenanigans to understand how easy C# & it's API's are, you can run `dotnet new webapi` to generate the basic scaffold, then copy this inside the `profiles:` section of `launchSettings.json`:

```json
"hotreloadprofile": {
      "commandName": "Project",

      "launchBrowser": true,

      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",

        "Key": "Value"
      },

      "applicationUrl": "https://localhost:5002;http://localhost:5003",

      "hotReloadProfile": "aspnetcore"
    },
```

Then run it in Hot Reload Mode by running `dotnet watch -p .`.  

If you navigate to `localhost:5002/weatherforecast`, you'll see results from your controller. (mine was 5002 b/c of Docker, sorry!)

Observe the random Weather JSON that comes back.

Then navigate to the `WeatherForecastController.cs` and change the following:

```cs
Enumerable.Range(1, 5)
```
To this:


```cs
Enumerable.Range(1, 50)
```


You should see dramatically more weather forecasts automatically load!

> NOTE: If you try changing any variables marked `static`, you won't induce a change.  Why?  Because static variables are cached on the heap and it's my suspicion that only stack memory can be Hot Reloaded.  This is ok because it's predictable and makes sense from the language design.