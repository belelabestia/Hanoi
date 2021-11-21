echo '$_: dotnet pack'
dotnet pack

echo '$_: dotnet tool update --global --add-source nuget hanoi'
dotnet tool update --global --add-source nuget hanoi