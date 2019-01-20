# NOTE: Script doesn't build projects. You need to build them into release yourself before running it.

Add-Type -A System.IO.Compression.FileSystem

$srcLibDir = "$PWD\Monofoxe\bin\Release"
$srcPipelineLibDir = "$PWD\Monofoxe\bin\Pipeline\Release"
$srcFMODLibDir = "$PWD\Monofoxe\bin\Release\FMOD"


$destReleaseDir = "$PWD\Release\"
$destLibDir = "$destReleaseDir\RawLibraries"
$desktopGL = "MonofoxeDesktopGL"
$desktopGLTemplate = "$PWD\Templates\$desktopGL\"


"Cleaning output directory at $destReleaseDir..."
if (Test-Path "$destReleaseDir" -PathType Container)
{
	Remove-Item "$destReleaseDir" -Force -Recurse
}
New-Item -ItemType Directory -Force -Path "$destReleaseDir" > $null


"Copying templates from $desktopGLTemplate..."
Copy-Item -path "$desktopGLTemplate" -Destination "$destReleaseDir" -Recurse -Container

"Copying libraries for templates from $desktopGLTemplate..."
New-Item -ItemType Directory -Force -Path "$destReleaseDir$desktopGL\References\" > $null
Copy-Item -path "$srcLibDir\*" -Filter "*.dll" -Destination "$destReleaseDir$desktopGL\References\"
New-Item -ItemType Directory -Force -Path "$destReleaseDir$desktopGL\Content\References\" > $null
Copy-Item -path "$srcPipelineLibDir\*" -Filter "*.dll" -Destination "$destReleaseDir$desktopGL\Content\References\"
Copy-Item -path "$srcFMODLibDir" -Destination "$destReleaseDir$desktopGL\" -Recurse


"Copying raw libraries..."
New-Item -ItemType Directory -Force -Path "$destLibDir" > $null
Copy-Item -path "$srcLibDir\*" -Filter "*.dll" -Destination "$destLibDir"
New-Item -ItemType Directory -Force -Path "$destLibDir\Pipeline\" > $null
Copy-Item -path "$srcPipelineLibDir\*" -Filter "*.dll" -Destination "$destLibDir\Pipeline\"
Copy-Item -path "$srcFMODLibDir" -Destination $destLibDir -Recurse


"Packing templates..."
[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$desktopGL", "$destReleaseDir$desktopGL.zip")
"Packing raw libraries..."
[IO.Compression.ZipFile]::CreateFromDirectory("$destLibDir", "$destLibDir.zip")

"Cleaning..."
Remove-Item "$destReleaseDir$desktopGL" -Force -Recurse
Remove-Item "$destLibDir" -Force -Recurse

Read-Host -Prompt "Done! Press Enter to exit"