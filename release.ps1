# Builds nuget packages and templates.

$templatesPath = "$PWD\Templates\";
$destReleaseDir = "$PWD\dist\"

"Cleaning output directory at $destReleaseDir..."
if (Test-Path "$destReleaseDir" -PathType Container)
{
	Remove-Item "$destReleaseDir" -Force -Recurse
}
New-Item -ItemType Directory -Force -Path "$destReleaseDir" > $null

"Compiling shaders..."
dotnet tool install -g dotnet-mgfxc
dotnet tool update -g dotnet-mgfxc
mgfxc Monofoxe/Resources/AlphaBlend.fx Monofoxe/Resources/AlphaBlend_dx.mgfxo /Profile:DirectX_11
mgfxc Monofoxe/Resources/AlphaBlend.fx Monofoxe/Resources/AlphaBlend_gl.mgfxo /Profile:OpenGL

"Cleaning solution..."
dotnet clean "$PWD/Monofoxe/Monofoxe.sln" -v q

"Building packages..."
Function Build([string] $proj)
{
	"Building $proj"
	dotnet build "$PWD\Monofoxe\$proj\$proj.csproj" -v q -c Release -p "NoWarn=1591"
	dotnet pack "$PWD\Monofoxe\$proj\$proj.csproj" -v q -c Release -p "NoWarn=1591" -o "$destReleaseDir"
}

Build "Monofoxe.Engine"
Build "Monofoxe.Engine.WindowsGL"
Build "Monofoxe.Engine.WindowsDX"
Build "Monofoxe.Tiled"
Build "Monofoxe.Pipeline"

"Building templates..."
dotnet pack "$templatesPath\template.csproj" -o  "$destReleaseDir"

"DONE! Output files can be found in $destReleaseDir"

"NOTE: Make sure you have set the appropriate Monofoxe version in Packages.props and bumped Nopipeline version in the templates."