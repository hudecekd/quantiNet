# We want powershell to stop immediatelly after error occures.
# This handles only "powershell" commands. Native commands has to be handled by checking $LastExitCode variable after they finish.
$ErrorActionPreference = "Stop"

[string] $configuration = "Release"

# output directory where nuget packages will be stored after build
[string] $outputDirectory = ".\output\$configuration";

# For now we expect nuget to be in this location.
# Maybe it should be in GIT or downloaded from internet when build is made?
[string] $nuget = "c:\nuget\4.4.0\nuget.exe"