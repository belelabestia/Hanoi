echo '$_: dotnet pack'
dotnet pack

echo '$_: dotnet tool install --global --add-source nuget Hanoi'
dotnet tool install --global --add-source nuget Hanoi