# go to root folder
cd ..

msbuild /t:Clean QuantiNET\QuantiNET.S20.sln
msbuild /t:Clean QuantiNET\QuantiNET.sln

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