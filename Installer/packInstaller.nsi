; Creates the installer.
; NOTE: Do not run on its own, use PackRelease.ps1 instead.


!define APPNAME "Monofoxe"
!define APPVERSION "v2-dev"
!define NPL_APPNAME "NoPipeline"
!define INSTALLERVERSION "2.0.0.0-dev"

!define MUI_ICON "pics\icon.ico"
!define MUI_UNICON "pics\icon.ico"

!define NOPIPELINEROOT "..\NoPipeline\NoPipeline\NoPipeline\bin\Release"
!define TEMPLATES_DIRECTORY "Templates\ProjectTemplates\Visual C#\${APPNAME} ${APPVERSION}"

!include "Sections.nsh"
!include "MUI2.nsh"
!include "InstallOptions.nsh"


!define MUI_WELCOMEFINISHPAGE_BITMAP "pics\panel.bmp"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

Name '${APPNAME} ${INSTALLERVERSION}'
OutFile '..\Release\MonofoxeSetup.exe'
InstallDir '$PROGRAMFILES\${APPNAME} Engine\${APPVERSION}\' ; Main install directory.

VIProductVersion "${INSTALLERVERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "${APPNAME}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "Chai Foxes"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "${INSTALLERVERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductVersion" "${INSTALLERVERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription" "${APPNAME} Installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright © Chai Foxes"


; Request application privileges.
RequestExecutionLevel admin


; UI stuff.
!define MUI_HEADERIMAGE "pics\Monofoxe.bmp"
!define MUI_HEADERIMAGE_BITMAP "pics\Monofoxe.bmp"
!define MUI_ABORTWARNING
; UI stuff.

; Stuff to install.
Section "Monofoxe" Monofoxe
	SectionIn RO
	RMDir /r '$INSTDIR'
	SetOutPath '$INSTDIR\lib'
	File /r '..\Monofoxe\bin\Release\*.dll'
	File /r '..\Monofoxe\bin\Release\*.xml'

	SetOutPath '$INSTDIR\lib\Pipeline'
	File /r '..\Monofoxe\bin\Pipeline\Release\*.dll'

	SetOutPath '$INSTDIR'
	WriteUninstaller "uninstall.exe"

	# NoPipeline.
	SetOutPath '$INSTDIR\NoPipeline'
	File /r "${NOPIPELINEROOT}\*.exe"
	File /r "${NOPIPELINEROOT}\*.dll"
	File /r "..\Common\Monofoxe.props"

	File /r "Externals\Monofoxe.NoPipeline.targets"
	# NoPipeline.
SectionEnd

Section "MonoGame" Monogame
	SetOutPath '$INSTDIR'
	File "Externals\MonoGameSetup.exe"
	ExecWait "$INSTDIR\MonoGameSetup.exe"
	Delete "$INSTDIR\MonoGameSetup.exe"
SectionEnd

Section "Visual Studio 2015 Templates/" VS2015
	SetOutPath "$DOCUMENTS\Visual Studio 2015\${TEMPLATES_DIRECTORY}"
	File /r '..\Release\*.zip'
SectionEnd

Section "Visual Studio 2017 Templates/" VS2017
	SetOutPath "$DOCUMENTS\Visual Studio 2017\${TEMPLATES_DIRECTORY}"
	File /r '..\Release\*.zip'
SectionEnd

Section "Visual Studio 2019 Templates/" VS2019
	SetOutPath "$DOCUMENTS\Visual Studio 2019\${TEMPLATES_DIRECTORY}"
	File /r '..\Release\*.zip'
SectionEnd

!define OldMonofoxeInstallationDir '$PROGRAMFILES\Monofoxe\'
Section "Remove old versions." RemoveOldVersions
  RMDir /r "$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\Monofoxe"
  RMDir /r "$DOCUMENTS\Visual Studio 2017\Templates\ProjectTemplates\Visual C#\Monofoxe"
  RMDir /r "$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\Visual C#\Monofoxe"
  Delete "${OldMonofoxeInstallationDir}\Uninstall.exe"
  RMDir /r "${OldMonofoxeInstallationDir}"
SectionEnd

; Stuff to install.


; Component menu.
LangString MonofoxeDesc ${LANG_ENGLISH} "Install ${APPNAME}!"
LangString MonogameDesc ${LANG_ENGLISH} "Install MonoGame 3.7.1."
LangString VS2015Desc ${LANG_ENGLISH} "Install project templates for Visual Studio 2015. Templates are required to create new projects."
LangString VS2017Desc ${LANG_ENGLISH} "Install project templates for Visual Studio 2017. Templates are required to create new projects."
LangString VS2019Desc ${LANG_ENGLISH} "Install project templates for Visual Studio 2019. Templates are required to create new projects."
LangString RemoveOldVersionsDesc ${LANG_ENGLISH} "Remove all previous Monofoxe versions."


!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${Monofoxe} $(MonofoxeDesc)
	!insertmacro MUI_DESCRIPTION_TEXT ${Monogame} $(MonogameDesc)
	!insertmacro MUI_DESCRIPTION_TEXT ${VS2015} $(VS2015Desc)
	!insertmacro MUI_DESCRIPTION_TEXT ${VS2017} $(VS2017Desc)
	!insertmacro MUI_DESCRIPTION_TEXT ${VS2019} $(VS2019Desc)
	!insertmacro MUI_DESCRIPTION_TEXT ${RemoveOldVersions} $(RemoveOldVersionsDesc)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Function checkMonogame
IfFileExists `$PROGRAMFILES\MonoGame\v3.0\*.*` disable end
	disable:
		SectionSetFlags ${Monogame} $1
	end:
FunctionEnd

Function checkVS2015
IfFileExists `$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\*.*` end disable
	disable:
		SectionSetFlags ${VS2015} $1
	end:
FunctionEnd

Function checkVS2017
IfFileExists `$DOCUMENTS\Visual Studio 2017\Templates\ProjectTemplates\*.*` end disable
	disable:
		SectionSetFlags ${VS2017} $1
	end:
FunctionEnd

Function checkVS2019
IfFileExists `$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\*.*` end disable
	disable:
		SectionSetFlags ${VS2019} $1
	end:
FunctionEnd
; Component menu.

Function .onInit
	IntOp $0 $0 | ${SF_RO}
	Call checkMonogame
	Call checkVS2015
	Call checkVS2017
	Call checkVS2019
	IntOp $0 ${SF_SELECTED} | ${SF_RO}
FunctionEnd

; Uninstaller Section

Section "Uninstall"
	RMDir /r "$DOCUMENTS\Visual Studio 2015\${TEMPLATES_DIRECTORY}"
	RMDir /r "$DOCUMENTS\Visual Studio 2017\${TEMPLATES_DIRECTORY}"
	RMDir /r "$DOCUMENTS\Visual Studio 2019\${TEMPLATES_DIRECTORY}"
	Delete "$INSTDIR\Uninstall.exe"
	RMDir /r "$INSTDIR"
SectionEnd



