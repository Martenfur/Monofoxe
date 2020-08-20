# Packs templates and installer.
# NOTE: To create an installer you'll need a NSIS installed,
# and nsis.exe added to PATH.
# Maybe rewrite someday using this https://cakebuild.net.

$projectTemplatesPath = "$PWD\Templates\ProjectTemplates\";
$itemTemplatesPath = "$PWD\Templates\ItemTemplates\";


Function Pack-Item-Template([string] $item)
{
	"Packing $item..."
	[IO.Compression.ZipFile]::CreateFromDirectory("$itemTemplatesPath$item", "$destItemTemplatesDir$item.zip")
}

Function Assemble-Template([string] $platform)
{
	"Assembling templates for $platform..."
	Copy-Item -path "$projectTemplatesPath$platform\" -Destination "$destProjectTemplatesDir" -Recurse -Container
	Copy-Item -path "$PWD/Common/*" -Destination "$destProjectTemplatesDir$platform" -Recurse -Container

	Copy-Item -path "$destProjectTemplatesDir$platform\" -Destination "$destProjectTemplatesDir$crossplatform\" -Recurse -Container

	[IO.Compression.ZipFile]::CreateFromDirectory("$destProjectTemplatesDir$platform", "$destProjectTemplatesDir$platform.zip")
}

Add-Type -A System.IO.Compression.FileSystem

$debug = $FALSE

$srcLibDir = "$PWD\Monofoxe\bin\Release"

$destCommonDir = "$PWD\Templates\CommonFiles"
$destReleaseDir = "$PWD\Release\"
$destProjectTemplatesDir = "$destReleaseDir\ProjectTemplates\"
$destItemTemplatesDir = "$destReleaseDir\ItemTemplates\"

$crossplatform = "Crossplatform"


"Building solutions..."
dotnet build ("$PWD\Monofoxe\Monofoxe.sln" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build', '/p:NoWarn=1591')
dotnet build ("$PWD\NoPipeline\NoPipeline\NoPipeline.sln" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build')


"Cleaning output directory at $destReleaseDir..."
if (Test-Path "$destReleaseDir" -PathType Container)
{
	Remove-Item "$destReleaseDir" -Force -Recurse
}
New-Item -ItemType Directory -Force -Path "$destReleaseDir" > $null
New-Item -ItemType Directory -Force -Path "$destProjectTemplatesDir" > $null
New-Item -ItemType Directory -Force -Path "$destItemTemplatesDir" > $null



Copy-Item -path "$projectTemplatesPath$crossplatform\" -Destination "$destProjectTemplatesDir" -Recurse -Container

Pack-Item-Template "Entity"
Pack-Item-Template "Component"
Pack-Item-Template "EntityTemplate"
Pack-Item-Template "TiledEntityFactory"
Pack-Item-Template "ResourceBox"

Assemble-Template "GL"
Assemble-Template "DX"
Assemble-Template "MonofoxeDotnetStandardLibrary"
Assemble-Template "Shared"


[IO.Compression.ZipFile]::CreateFromDirectory("$destProjectTemplatesDir$crossplatform", "$destProjectTemplatesDir$crossplatform.zip")


"Making installer..."
&makensis Installer/packInstaller.nsi

"Cleaning..."
if (!$debug)
{
	Remove-Item "$destProjectTemplatesDir" -Force -Recurse
	Remove-Item "$destItemTemplatesDir" -Force -Recurse
}

Read-Host -Prompt "Done! Press Enter to exit"



