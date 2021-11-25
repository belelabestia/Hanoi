echo '$_: dotnet pack'
dotnet pack

echo '$_: dotnet tool update --global --add-source nupkg hanoi'
dotnet tool update --global --add-source nupkg hanoi