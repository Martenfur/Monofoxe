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

Function Assemble-Template([string] $platform)
{
	"Assembling templates for $platform..."
	Copy-Item -path "$PWD\Templates\$platform\" -Destination "$destReleaseDir" -Recurse -Container
	Copy-Item -path "$PWD/Common/*" -Destination "$destReleaseDir$platform" -Recurse -Container

	Copy-Item -path "$destReleaseDir$platform\" -Destination "$destReleaseDir$crossplatform\" -Recurse -Container

	[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$platform", "$destReleaseDir$platform.zip")
}

Add-Type -A System.IO.Compression.FileSystem

$debug = $TRUE

$srcLibDir = "$PWD\Monofoxe\bin\Release"

$destCommonDir = "$PWD\Templates\CommonFiles"
$destReleaseDir = "$PWD\Release\"

$GL = "GL"
$DX = "DX"
$shared = "Shared"
$library = "MonofoxeDotnetStandardLibrary"
$crossplatform = "Crossplatform"

"Building solution $msbuild..."
&$msbuild ("$PWD\Monofoxe\Monofoxe.sln" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build')
&$msbuild ("$PWD\NoPipeline\NoPipeline\NoPipeline.sln" ,'/verbosity:q','/p:configuration=Release','/t:Clean,Build')


"Cleaning output directory at $destReleaseDir..."
if (Test-Path "$destReleaseDir" -PathType Container)
{
	Remove-Item "$destReleaseDir" -Force -Recurse
}
New-Item -ItemType Directory -Force -Path "$destReleaseDir" > $null



Copy-Item -path "$PWD\Templates\$crossplatform\" -Destination "$destReleaseDir" -Recurse -Container

Assemble-Template $GL
Assemble-Template $DX
Assemble-Template $library

Assemble-Template $shared

[IO.Compression.ZipFile]::CreateFromDirectory("$destReleaseDir$crossplatform", "$destReleaseDir$crossplatform.zip")


"Making installer..."
&makensis Installer/packInstaller.nsi

"Cleaning..."
if (!$debug)
{
	Remove-Item "$destReleaseDir$GL" -Force -Recurse
	Remove-Item "$destReleaseDir$DX" -Force -Recurse
	Remove-Item "$destReleaseDir$shared" -Force -Recurse
	Remove-Item "$destReleaseDir$library" -Force -Recurse
	Remove-Item "$destReleaseDir$crossplatform" -Force -Recurse

	Remove-Item "$destReleaseDir$GL.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$DX.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$shared.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$library.zip" -Force -Recurse
	Remove-Item "$destReleaseDir$crossplatform.zip" -Force -Recurse
}

Read-Host -Prompt "Done! Press Enter to exit"



