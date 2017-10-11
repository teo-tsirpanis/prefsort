dotnet restore

dotnet publish prefsort.fsproj -c Release -f net45 --self-contained -r win-x86 -o .\prefsort-win32
Compress-Archive -Path ".\prefsort-win32" -DestinationPath ".\prefsort-win32"

dotnet publish Prefsort.fsproj -c Release -f netcoreapp2.0 --self-contained -r linux-x64 -o .\prefsort-linux
Compress-Archive -path ".\prefsort-linux" -DestinationPath ".\prefsort-linux"
