#!/bin/bash
sln_file='./HighLoad App.sln'
dotnet tool restore
dotnet build "$sln_file"
dotnet jb cleanupcode "$sln_file" --settings='../default-codestyle-and-formatting.DotSettings' --profile='Nessi Standard' --verbosity=INFO
