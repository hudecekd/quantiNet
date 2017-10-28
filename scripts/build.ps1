$ErrorActionPreference = "Stop"

[string] $configuration = "Release"
[string] $nuget = "c:\nuget\4.4.0\nuget.exe"


# nuget properties
[string] $licenseUrl = "http://www.quanti.cz"

# go to root folder
$currentPathBackup = (Get-Location).Path
cd ..

Try
{
  # Build of .NET Standard
  Invoke-Expression "$nuget restore QuantiNET\QuantiNET.S20.sln"
  msbuild /p:Configuration=$configuration /t:pack QuantiNET\QuantiNET.S20.sln
  if ($LastExitCode -ne -0)
  {
    echo "Build of QuantiNET.S20 failed."
    exit $LastExitCode
  }

  # Build of .NET Framework
  Invoke-Expression "$nuget restore QuantiNET\QuantiNET.sln"
  msbuild /p:Configuration=$configuration QuantiNET\QuantiNET.sln
  if ($LastExitCode -ne -0)
  {
    echo "Build of QuantiNET failed."
    exit $LastExitCode
  }
  Invoke-Expression "$nuget pack QuantiNET\Quanti.Utils\Quanti.Utils.csproj -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"
  Invoke-Expression "$nuget pack QuantiNET\Quanti.WPF.Utils\Quanti.WPF.Utils.csproj -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"
}
Catch
{
  # we do not know what failed so simple return code is enough.
  exit 1
}
Finally
{
  cd $currentPathBackup
}