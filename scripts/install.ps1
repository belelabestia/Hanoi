echo '$_: dotnet pack'
dotnet pack

echo '$_: dotnet tool install --global --add-source nupkg Hanoi'
dotnet tool install --global --add-source nupkg Hanoi