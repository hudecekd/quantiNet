. .\configuration.ps1

# go to root folder
cd ..

[string] $configuration = "Release"

mstest /testcontainer:QuantiNET\Quanti.Utils.UnitTests\bin\$configuration\Quanti.Utils.UnitTests.dll