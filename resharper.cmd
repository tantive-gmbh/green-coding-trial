@echo off
SET sln-file="./HighLoad App.sln"
dotnet tool restore
dotnet build %sln-file%
dotnet jb cleanupcode %sln-file% --settings="../default-codestyle-and-formatting.DotSettings" --profile="Nessi Standard" --verbosity=INFO
