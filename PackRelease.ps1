# NOTE: Script doesn't build projects. You need to build them into release yourself before running it.
# Maybe rewrite using this https://cakebuild.net someday.

# Credit: https://alastaircrabtree.com/how-to-find-latest-version-of-msbuild-in-powershell/
Function Find-MsBuild([int] $MaxVersion = 2017)
{
    $agentPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe"
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
    $fallbackPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319"
		
    If ((2017 -le $MaxVersion) -And (Test-Path $agentPath)) { return $agentPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $proPath)) { return $proPath } 
    If ((2017 -le $MaxVersion) -And (Test-Path $communityPath)) { return $communityPath } 
    If ((2015 -le $MaxVersion) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $MaxVersion) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
    If (Test-Path $fallbackPath) { return $fallbackPath } 
        
    throw "Yikes - Unable to find msbuild"
}



Add-Type -A System.IO.Compression.FileSystem

$msbuild = Find-MsBuild

$srcLibDir = "$PWD\Monofoxe\bin\Release"
$srcPipelineLibDir = "$PWD\Monofoxe\bin\Pipeline\Release"
$srcFMODLibDir = "$PWD\Monofoxe\bin\Release\FMOD"


$destReleaseDir = "$PWD\Release\"
$destLibDir = "$destReleaseDir\RawLibraries"
$desktopGL = "MonofoxeDesktopGL"
$desktopGLTemplate = "$PWD\Templates\$desktopGL\"


"Building solution $msbuild..."
&$msbuild ("$PWD\Monofoxe\Monofoxe.sln" ,'/verbosity:q','/p:configuration=Debug','/t:Clean,Build') > $null
&$msbuild ("$PWD\Monofoxe\Monofoxe.sln" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build') > $null


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
Copy-Item -path "$srcLibDir\*" -Filter "*.xml" -Destination "$destReleaseDir$desktopGL\References\"
# Copying deafult shader into the content directory.
New-Item -ItemType Directory -Force -Path "$destReleaseDir$desktopGL\Content\Effects\" > $null
Copy-Item -path "$srcLibDir\*" -Filter "*.fx" -Destination "$destReleaseDir$desktopGL\Content\Effects\"
New-Item -ItemType Directory -Force -Path "$destReleaseDir$desktopGL\Content\References\" > $null
Copy-Item -path "$srcPipelineLibDir\*" -Filter "*.dll" -Destination "$destReleaseDir$desktopGL\Content\References\"
Copy-Item -path "$srcFMODLibDir" -Destination "$destReleaseDir$desktopGL\" -Recurse


"Copying raw libraries..."
New-Item -ItemType Directory -Force -Path "$destLibDir" > $null
Copy-Item -path "$srcLibDir\*" -Filter "*.dll" -Destination "$destLibDir"
Copy-Item -path "$srcLibDir\*" -Filter "*.xml" -Destination "$destLibDir"
Copy-Item -path "$srcLibDir\*" -Filter "*.fx" -Destination "$destLibDir"
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



