# does not work correctly when called here. It looks that msbuild is still running because it keeps some files and directories there.
# When called manually later it works.
# We want 'clean' script to be able to work alone. We change directory so it looks from the 'clean' script perspective that working directory is the same as script's location
# and then it is up to script to change directory if it is needed.

. .\configuration.ps1

# go to root folder
cd ..

msbuild /t:Clean QuantiNET\QuantiNET.S20.sln
if ($LastExitCode -ne -0)
{
  echo "Clean of QuantiNET.S20 failed."
  exit $LastExitCode
}
msbuild /t:Clean QuantiNET\QuantiNET.sln
if ($LastExitCode -ne -0)
{
  echo "Clean of QuantiNET failed."
  exit $LastExitCode
}

# msbuild still keeps bin, obj folders. We remove them here.

# Clean .NET Standard folders
Remove-Item  .\QuantiNET\Quanti.Utils\obj.S20 -Force -Recurse
Remove-Item  .\QuantiNET\Quanti.Utils\bin.S20 -Force -Recurse

# Clean .NET Framework folders
Remove-Item .\QuantiNET\Quanti.Utils\bin -Force -Recurse
Remove-Item .\QuantiNET\Quanti.Utils\obj -Force -Recurse

# Clean .NET Framework folders
Remove-Item .\QuantiNET\Quanti.WPF.Utils\bin -Force -Recurse
Remove-Item .\QuantiNET\Quanti.WPF.Utils\obj -Force -Recurse

# remove all nupkg files in root folder.
# For .NET Framework they are placed here.
# For .NET Standard they are placed in bin/Release folder which is removed above.
Remove-Item *.nupkg -Force