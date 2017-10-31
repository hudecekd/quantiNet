. .\configuration.ps1

# nuget properties
[string] $licenseUrl = "http://www.quanti.cz"
[string] $outputDirectory = ".\output\$configuration";

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

  # create output directory where nuget packages will be saved only when it does not exists.
  if (!(Test-Path -Path $outputDirectory))
  {
    New-Item -ItemType directory $outputDirectory
  }

  # msbuild of .NET Standard uses output directory of project.
  # Do not know whether we can set path only for nuget package so we need to copy it from build folder
  Copy-Item QuantiNET\Quanti.Utils\bin.S20\$configuration\Quanti.Utils.S20.0.0.1.nupkg $outputDirectory

  Invoke-Expression "$nuget pack QuantiNET\Quanti.Utils\Quanti.Utils.csproj -OutputDirectory $outputDirectory -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"
  Invoke-Expression "$nuget pack QuantiNET\Quanti.WPF.Utils\Quanti.WPF.Utils.csproj -OutputDirectory $outputDirectory -Prop Configuration=$configuration -Properties licenseUrl=$licenseUrl"
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