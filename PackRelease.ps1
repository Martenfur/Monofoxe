# Packs templates and installer.
# NOTE: To create an installer you'll need a NSIS installed,
# and nsis.exe added to PATH.
# Maybe rewrite someday using this https://cakebuild.net.

[xml]$XmlDocument = Get-Content -Path "$PWD\Packages.props"
$monofoxeVersion = $XmlDocument.Project.PropertyGroup.MonofoxeVersion
$nopipelineVersion = $XmlDocument.Project.PropertyGroup.NopipelineVersion

$projectTemplatesPath = "$PWD\Templates\ProjectTemplates\";
$itemTemplatesPath = "$PWD\Templates\ItemTemplates\";


Function PackItemTemplate([string] $item)
{
	"Packing $item..."
	[IO.Compression.ZipFile]::CreateFromDirectory("$itemTemplatesPath$item", "$destItemTemplatesDir$item.zip")
}

Function ReplaceParameters([string] $platform)
{
	"Assembling parameters for $platform..."

	$csprojPath = "$destProjectTemplatesDir\Crossplatform\$platform\$platform.csproj"

	if (Test-Path -Path $csprojPath)
	{
		(Get-Content -Path $csprojPath) -replace '\$\(MonofoxeVersion\)', $monofoxeVersion | Set-Content -Path $csprojPath
		(Get-Content -Path $csprojPath) -replace '\$\(NopipelineVersion\)', $nopipelineVersion | Set-Content -Path $csprojPath
	}
}
Add-Type -A System.IO.Compression.FileSystem

$destReleaseDir = "$PWD\Release\"
$destProjectTemplatesDir = "$destReleaseDir\ProjectTemplates\"
$destItemTemplatesDir = "$destReleaseDir\ItemTemplates\"

$crossplatform = "Crossplatform"

"Compiling shaders..."
dotnet tool install -g dotnet-mgfxc
dotnet tool update -g dotnet-mgfxc
mgfxc Monofoxe/Resources/AlphaBlend.fx Monofoxe/Resources/AlphaBlend_dx.mgfxo /Profile:DirectX_11
mgfxc Monofoxe/Resources/AlphaBlend.fx Monofoxe/Resources/AlphaBlend_gl.mgfxo /Profile:OpenGL


"Building solution..."

Function Build([string] $proj)
{
	dotnet build ("$PWD\Monofoxe\$proj\$proj.csproj" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build', '/p:NoWarn=1591')
}

Build "Monofoxe.Engine"
Build "Monofoxe.Engine.DesktopGL"
Build "Monofoxe.Engine.WindowsDX"
Build "Monofoxe.Tiled"
Build "Monofoxe.Pipeline"


"Cleaning output directory at $destReleaseDir..."
if (Test-Path "$destReleaseDir" -PathType Container)
{
	Remove-Item "$destReleaseDir" -Force -Recurse
}
New-Item -ItemType Directory -Force -Path "$destReleaseDir" > $null
New-Item -ItemType Directory -Force -Path "$destProjectTemplatesDir" > $null
New-Item -ItemType Directory -Force -Path "$destItemTemplatesDir" > $null


Copy-Item -path "$projectTemplatesPath$crossplatform\" -Destination "$destProjectTemplatesDir" -Recurse -Container

PackItemTemplate "Entity"
PackItemTemplate "Component"
PackItemTemplate "TiledEntityFactory"

ReplaceParameters "DX"
ReplaceParameters "GL"
ReplaceParameters "Library"
ReplaceParameters "Content"

[IO.Compression.ZipFile]::CreateFromDirectory("$destProjectTemplatesDir$crossplatform", "$destProjectTemplatesDir$crossplatform.zip")


"Making installer..."
&makensis Installer/packInstaller.nsi

$debug = $FALSE

"Cleaning..."
if (!$debug)
{
	Remove-Item "$destProjectTemplatesDir" -Force -Recurse
	Remove-Item "$destItemTemplatesDir" -Force -Recurse
}
