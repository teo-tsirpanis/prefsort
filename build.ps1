dotnet restore

Remove-Item prefsort-* -recurse

dotnet publish prefsort.fsproj -c Release -f net45 -r win-x86 -o .\prefsort-windows
Compress-Archive -Path ".\prefsort-windows" -DestinationPath ".\prefsort-windows"

dotnet publish prefsort.fsproj -c Release -f netcoreapp2.0 -r linux-x64 -o .\prefsort-linux
Compress-Archive -path ".\prefsort-linux" -DestinationPath ".\prefsort-linux"

dotnet publish prefsort.fsproj -c Release -f netcoreapp2.0 -r osx-x64 -o .\prefsort-mac
Compress-Archive -path ".\prefsort-mac" -DestinationPath ".\prefsort-mac"

dotnet publish prefsort.fsproj -c Release -f netcoreapp2.0 -o .\prefsort-netcore
Compress-Archive -path ".\prefsort-netcore" -DestinationPath ".\prefsort-netcore"
