# Packs templates and installer.
# NOTE: To create an installer you'll need a NSIS installed,
# and nsis.exe added to PATH.
# Maybe rewrite someday using this https://cakebuild.net.

# Credit: https://alastaircrabtree.com/how-to-find-latest-version-of-msbuild-in-powershell/
Function Find-MsBuild([int] $MaxVersion = 2019)
{
	$agentPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe"
	$devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe"
	$proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe"
	$communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe"
	$communityPath2019 = "$Env:programfiles (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe"
	$fallback2015Path = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\MSBuild.exe"
	$fallbackPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319"
	$fallback2013Path = "${Env:ProgramFiles(x86)}\MSBuild\12.0\Bin\MSBuild.exe"
	if ((2017 -le $MaxVersion) -And (Test-Path $agentPath)) { return $agentPath } 
	If ((2017 -le $MaxVersion) -And (Test-Path $devPath)) { return $devPath } 
	If ((2017 -le $MaxVersion) -And (Test-Path $proPath)) { return $proPath } 
	If ((2017 -le $MaxVersion) -And (Test-Path $communityPath)) { return $communityPath } 
	If ((2019 -le $MaxVersion) -And (Test-Path $communityPath2019)) { return $communityPath2019 } 
	If ((2015 -le $MaxVersion) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
	If ((2013 -le $MaxVersion) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
	If (Test-Path $fallbackPath) { return $fallbackPath } 

	throw "Yikes - Unable to find msbuild"
}
$msbuild = Find-MsBuild

Function Assemble-Template([string] $platform, [bool] $copyCommon)
{
	"Assembling templates for $platform..."
	Copy-Item -path "$PWD\Templates\$platform\" -Destination "$destReleaseDir" -Recurse -Container
	if ($copyCommon)
	{
		Copy-Item -path "$destCommonDir/*" -Destination "$destReleaseDir$platform" -Recurse -Container
		New-Item -ItemType Directory -Force -Path "$destReleaseDir$platform\Content\Effects\" > $null
		Copy-Item -path "$srcLibDir\*" -Filter "*.fx" -Destination "$destReleaseDir$platform\Content\Effects\"
	}
	Copy-Item -path "$PWD/Common/*" -Destination "$destReleaseDir$platform" -Recurse -Container
}

Add-Type -A System.IO.Compression.FileSystem

$debug = $TRUE

$srcLibDir = "$PWD\Monofoxe\bin\Release"

$destCommonDir = "$PWD\Templates\CommonFiles"
$destReleaseDir = "$PWD\Release\"

$desktopGL = "MonofoxeDesktopGL"
$blankDesktopGL = "MonofoxeDesktopGLBlank"
$blankWindows = "MonofoxeWindowsBlank"
$shared = "MonofoxeShared"
$library = "MonofoxeDotnetStandardLibrary"

"Building solution $msbuild..."
&$msbuild ("$PWD\Monofoxe\Monofoxe.sln" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build')
&$msbuild ("$PWD\NoPipeline\NoPipeline\NoPipeline.sln" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build')


"Cleaning output directory at $destReleaseDir..."
if (Test-Path "$destReleaseDir" -PathType Container)
{
	Remove-Item "$destReleaseDir" -Force -Recurse
}
New-Item -ItemType Directory -Force -Path "$destReleaseDir" > $null


Assemble-Template $desktopGL $TRUE
Assemble-Template $blankDesktopGL $FALSE
Assemble-Template $blankWindows $FALSE
Assemble-Template $shared $TRUE
Assemble-Template $library $FALSE


"Packing templates..."
[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$desktopGL", "$destReleaseDir$desktopGL.zip")
[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$blankDesktopGL", "$destReleaseDir$blankDesktopGL.zip")
[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$blankWindows", "$destReleaseDir$blankWindows.zip")
[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$shared", "$destReleaseDir$shared.zip")
[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$library", "$destReleaseDir$library.zip")


"Making installer..."
&makensis Installer/packInstaller.nsi

"Cleaning..."
if ($debug)
{
	Remove-Item "$destReleaseDir$desktopGL" -Force -Recurse
	Remove-Item "$destReleaseDir$blankDesktopGL" -Force -Recurse
	Remove-Item "$destReleaseDir$blankWindows" -Force -Recurse
	Remove-Item "$destReleaseDir$shared" -Force -Recurse
	Remove-Item "$destReleaseDir$library" -Force -Recurse

	Remove-Item "$destReleaseDir$desktopGL.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$blankDesktopGL.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$blankWindows.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$shared.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$library.zip" -Force -Recurse
}

Read-Host -Prompt "Done! Press Enter to exit"



