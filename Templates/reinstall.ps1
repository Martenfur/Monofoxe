# Development script to quickly reinstall the template.
dotnet pack template.csproj -o ../dist/
dotnet new uninstall Monofoxe.Template
dotnet new install ../dist/Monofoxe.Template.*.nupkg --debug:rebuildcache