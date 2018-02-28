rmdir /S /Q "build" 2>nul

mkdir build\package\lib\netcoreapp1.0
mkdir build\package\lib\netcoreapp1.1
mkdir build\package\lib\netcoreapp2.0
mkdir build\package\lib\netstandard1.1
mkdir build\package\lib\net45
mkdir build\package\lib\net46
mkdir build\package\lib\net47

copy WebPush.nuspec build\package\

dotnet restore
dotnet build --configuration=Release WebPush\WebPush.csproj

copy WebPush\bin\Release\netcoreapp1.0\WebPush.dll build\package\lib\netcoreapp1.0\WebPush.dll
copy WebPush\bin\Release\netcoreapp1.1\WebPush.dll build\package\lib\netcoreapp1.1\WebPush.dll
copy WebPush\bin\Release\netcoreapp2.0\WebPush.dll build\package\lib\netcoreapp2.0\WebPush.dll
copy WebPush\bin\Release\netstandard1.1\WebPush.dll build\package\lib\netstandard1.1\WebPush.dll
copy WebPush\bin\Release\net45\WebPush.dll build\package\lib\net45\WebPush.dll
copy WebPush\bin\Release\net46\WebPush.dll build\package\lib\net46\WebPush.dll
copy WebPush\bin\Release\net47\WebPush.dll build\package\lib\net47\WebPush.dll

nuget pack build\package\WebPush.nuspec