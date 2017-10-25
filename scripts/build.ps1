[string] $nexusUrl = "https://repository.quanti.cz/repository/QuantiNET/"
[string] $configuration = "Release"
[string] $nuget = "c:\nuget\4.4.0\nuget.exe"

# nuget properties
[string] $licenseUrl = "http://www.quanti.cz"

# get api key for current user. Each user has his own api key stored in apiKey.txt file which is not pushed go GIT but saved locally!
[string] $apiKeyPath = "c:\nuget\nugetApiKey.txt"
[string] $apiKey = [System.IO.File]::ReadAllText($apiKeyPath)

# go to root folder
cd ..

# Build of .NET Standard
Invoke-Expression "$nuget restore QuantiNET\QuantiNET.S20.sln"
msbuild /p:Configuration=$configuration /t:pack QuantiNET\QuantiNET.S20.sln

# Build of .NET Framework
Invoke-Expression "$nuget restore QuantiNET\QuantiNET.sln"
msbuild /p:Configuration=$configuration QuantiNET\QuantiNET.sln
Invoke-Expression "$nuget pack QuantiNET\Quanti.Utils\Quanti.Utils.csproj -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"
Invoke-Expression "$nuget pack QuantiNET\Quanti.WPF.Utils\Quanti.WPF.Utils.csproj -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"

# TODO: is there a better way to locate nupkg files? Somehow specify exact path without '*'?
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey  Quanti.Utils.*.nupkg"
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey  Quanti.WPF.Utils.*.nupkg"

# When defualt bin folder is not changed msbuild adds netstandard folder by default. When it is changed we need to delete it so it behaves correctly
# and nuget packages is in "$configuration" folder.
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey QuantiNET\Quanti.Utils\bin.S20\$configuration\Quanti.Utils.S20.*.nupkg"

# does not work correctly when called here. It looks that msbuild is still running because it keeps some files and directories there.
# When called manually later it works.
# We want 'clean' script to be able to work alone. We change directory so it looks from the 'clean' script perspective that working directory is the same as script's location
# and then it is up to script to change directory if it is needed.
cd scripts
Invoke-Expression ".\clean.ps1"