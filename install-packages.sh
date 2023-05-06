# !bin/bash
## This installs essential CodeMechanic packages.
dotnet add package CodeMechanic.Types
dotnet add package CodeMechanic.Diagnostics
dotnet add package CodeMechanic.FileSystem
dotnet add package CodeMechanic.Embeds
dotnet add package CodeMechanic.Reflection
dotnet add package CodeMechanic.Regex


dotnet restore;
dotnet build;
