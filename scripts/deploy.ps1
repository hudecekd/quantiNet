﻿. .\configuration.ps1

[string] $nexusUrl = "https://repository.quanti.cz/repository/QuantiNET/"

# get api key for current user. Each user has his own api key stored in apiKey.txt file which is not pushed go GIT but saved locally!
[string] $apiKeyPath = "c:\nuget\nugetApiKey.txt"
[string] $apiKey = [System.IO.File]::ReadAllText($apiKeyPath)

# go to root directory
cd ..

# TODO: is there a better way to locate nupkg files? Somehow specify exact path without '*'?
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey  $outputDirectory\Quanti.Utils.0.0.1.nupkg"
if ($LastExitCode -ne -0)
{
  echo "Deploy failed."
  exit $LastExitCode
}

Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey  $outputDirectory\Quanti.WPF.Utils.0.0.1.nupkg"
if ($LastExitCode -ne -0)
{
  echo "Deploy failed."
  exit $LastExitCode
}

# When defualt bin folder is not changed msbuild adds netstandard folder by default. When it is changed we need to delete it so it behaves correctly
# and nuget packages is in "$configuration" folder.
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey $outputDirectory\Quanti.Utils.S20.0.0.1.nupkg"
if ($LastExitCode -ne -0)
{
  echo "Deploy failed."
  exit $LastExitCode
}