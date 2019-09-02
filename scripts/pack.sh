rm ./pkgs/Chronicle*.nupkg
nuget pack ./src/Chronicle -Build -Version $1 -OutputDirectory ./pkgs  -Properties Configuration=Release;AssemblyVersion=$1