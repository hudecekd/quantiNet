[string] $nexusUrl = "http://localhost:8081/repository/quantiNET/"
[string] $apiKey = "ec177692-4594-3d56-896b-1f3bb95e08ce"
[string] $configuration = "Release"

# nuget properties
[string] $licenseUrl = "http://www.quanti.cz"

# go to root folder
cd ..

# Build of .NET Standard
Invoke-Expression ".\nuget restore QuantiNET\QuantiNET.S20.sln"
msbuild /p:Configuration=$configuration /t:pack QuantiNET\QuantiNET.S20.sln

# Build of .NET Framework
Invoke-Expression ".\nuget restore QuantiNET\QuantiNET.sln"
msbuild /p:Configuration=$configuration QuantiNET\QuantiNET.sln
Invoke-Expression ".\nuget pack QuantiNET\Quanti.Utils\Quanti.Utils.csproj -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"
Invoke-Expression ".\nuget pack QuantiNET\Quanti.WPF.Utils\Quanti.WPF.Utils.csproj -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"

# TODO: is there a better way to delete nupkg files? Somehow specify exact path without '*'?
Invoke-Expression ".\nuget push -Source $nexusUrl -ApiKey $apiKey  Quanti.Utils.*.nupkg"
Invoke-Expression ".\nuget push -Source $nexusUrl -ApiKey $apiKey  Quanti.WPF.Utils.*.nupkg"

# It looks that there is a BUG in msbuild. When default folder is changed from bin to bin.S20 we can see in VS that there are
# two netstandard1.6 folders even when in csproj is only one! So we have to use it to locate nuget package.
Invoke-Expression ".\nuget push -Source $nexusUrl -ApiKey $apiKey QuantiNET\Quanti.Utils\bin.S20\$configuration\netstandard1.6\Quanti.Utils.S20.*.nupkg"

# does not work correctly when called here. It looks that msbuild is still running because it keeps some files and directories there.
# When called manually later it works.
# We want 'clean' script to be able to work alone. We change directory so it looks from the 'clean' script perspective that working directory is the same as script's location
# and then it is up to script to change directory if it is needed.
cd scripts
Invoke-Expression ".\clean.ps1"