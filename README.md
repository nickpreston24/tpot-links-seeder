# TPOT Seeder :seedling:

## Quickstart :spiral_note_pad:

To install everything to run this API, run the following from GitBash, Bash or `sh`ell:

`bash install-packages.sh`

To ensure the most up to date packages, run [`nupu`](https://github.com/ThomasArdal/NuPU), which can be installed using the dotnet cli tool helper: `dotnet tool install --global NuPU`.

Then run it in Hot Reload Mode by running `dotnet watch -p .`.  This is similar to `nodemon`, but much, much faster. :wink:

Swagger will generate a test UI for you and will auto regenerate on changes.

> NOTE: If you try changing any variables marked `static`, you won't induce a change.  Why?  Because static variables are cached on the heap and it's my suspicion that only stack memory can be Hot Reloaded.  This is ok because it's predictable and makes sense from the language design


# Nuget Registry Instructions 

## Setting up MyGet registry on your local

```bash
a*******-*f**-****-b*bb-d**d**d****c  ### <= api key (primary)
```


```bash

### Adding a source:
nuget sources add -name [name] -source [url] -user [username] -pass [pwd]

### Example:
nuget sources add -name code-mechanic -source https://www.myget.org/F/code-mechanic/api/v3/index.json -user nickpreston17 -pass [pwd]



### pushing a package (release mode)
dotnet pack -c Release  #you must be in the same folder as your csproj

nuget push ./bin/Release/my-awesome-nupkg.1.0.0.nupkg {{api_key}} -Source https://www.myget.org/F/code-mechanic/api/v2/package

### installing (long)
dotnet add package my-awesome-nupkg --version 1.0.0 --source https://www.myget.org/F/code-mechanic/api/v3/index.json

### installing (short)
dotnet add package my-awesome-nupkg

```

Here's how to add your private [feed](https://docs.myget.org/docs/walkthrough/getting-started-with-nuget): 

```xml (csproj)
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="MyGet" value="https://www.myget.org/F/code-mechanic/api/v3/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <MyGet>
      <add key="Username" value="username-here" />
      <add key="ClearTextPassword" value="password-here" />
    </MyGet>
  </packageSourceCredentials>
  <activePackageSource>
    <add key="All" value="(Aggregate source)" />
  </activePackageSource>
</configuration>

```

### MyGet Troubleshooting:

https://docs.myget.org/docs/how-to/nuget-configuration-inheritance


## Productivity Scripts

Here's a sample script I use to quickly bump a version (`nurelease`), then push straight to my Nuget Package registry host (using `nupush`).  My registry service is MyGet, but others may be used. 

These are here for me to be able to go top speed when maintaining both CodeMechanic as a monorepo and anything that depends on it.

```bashrc

function nurelease(){
dotnet clean;
rm -rf obj/ bin/;
dotnet build;
dotnet pack -p:PackageVersion="$1" -c Release;

}

function nupush()
{
    ## Copy your api key to your .bashrc
    nuget push "$1" {{api-key}} -Source https://www.myget.org/F/code-mechanic/api/v2/package
}


function numechanic() {


# !bin/bash
## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex

}

function nurazor() {

dotnet new classlib

# !bin/bash
## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex
dotnet add package CodeMechanic.RazorHAT

}


function nuapi() {

dotnet new webapi

# !bin/bash
## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex


}



function nuclasslib() {

dotnet new classlib

# !bin/bash
## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex


}

```