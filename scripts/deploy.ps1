# go to root directory
cd ..

# TODO: is there a better way to locate nupkg files? Somehow specify exact path without '*'?
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey  Quanti.Utils.*.nupkg"
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey  Quanti.WPF.Utils.*.nupkg"

# When defualt bin folder is not changed msbuild adds netstandard folder by default. When it is changed we need to delete it so it behaves correctly
# and nuget packages is in "$configuration" folder.
Invoke-Expression "$nuget push -Source $nexusUrl -ApiKey $apiKey QuantiNET\Quanti.Utils\bin.S20\$configuration\Quanti.Utils.S20.*.nupkg"