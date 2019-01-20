Add-Type -A System.IO.Compression.FileSystem
Add-Type -A System


$srcLibDir = "$PWD\Monofoxe\bin\Release"
$srcPipelineLibDir = "$PWD\Monofoxe\bin\Pipeline\Release"
$srcFMODLibDir = "$PWD\Monofoxe\bin\Release\FMOD"


$destReleaseDir = "$PWD\Release\"
$destLibDir = "$destReleaseDir\RawLibraries"
$desktopGL = "MonofoxeDesktopGL"
$desktopGLTemplate = "$PWD\Templates\$desktopGL\"


# Cleaning old dir.
if (Test-Path $destReleaseDir -PathType Container)
{
	Remove-Item $destReleaseDir -Force -Recurse
}
New-Item -ItemType Directory -Force -Path $destReleaseDir
# Cleaning old dir.


# Copying DesktopGL template.
Copy-Item -path $desktopGLTemplate -Destination $destReleaseDir -Recurse -Container
# Copying libraries inside.
Copy-Item -path $srcLibDir\* -Filter "*.dll" -Destination "$destReleaseDir$desktopGL\References\"
Copy-Item -path $srcPipelineLibDir\* -Filter "*.dll" -Destination "$destReleaseDir$desktopGL\Content\References\"
Copy-Item -path $srcFMODLibDir -Destination "$destReleaseDir$desktopGL\" -Recurse



New-Item -ItemType Directory -Force -Path $destLibDir
Copy-Item -path $srcLibDir\* -Filter "*.dll" -Destination $destLibDir
New-Item -ItemType Directory -Force -Path "$destLibDir\Pipeline\"
Copy-Item -path $srcPipelineLibDir\* -Filter "*.dll" -Destination "$destLibDir\Pipeline\"
Copy-Item -path $srcFMODLibDir -Destination $destLibDir -Recurse


[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$desktopGL", "$destReleaseDir$desktopGL.zip")
[IO.Compression.ZipFile]::CreateFromDirectory("$destLibDir", "$destLibDir.zip")

Remove-Item "$destReleaseDir$desktopGL" -Force -Recurse
Remove-Item "$destLibDir" -Force -Recurse

Read-Host -Prompt "Done!"