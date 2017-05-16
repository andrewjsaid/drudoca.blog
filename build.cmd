cd src\Drudoca.Blog

echo Building for ubuntu.16.04-x64
dotnet restore -r ubuntu.16.04-x64
dotnet build -c Release -r ubuntu.16.04-x64
dotnet publish -f netcoreapp1.1 -c Release -r ubuntu.16.04-x64

echo Building for win10-x64
dotnet restore -r win10-x64
dotnet build -c Release -r win10-x64
dotnet publish -f netcoreapp1.1 -c Release -r win10-x64

cd ..\..\