; Creates the installer.
; NOTE: Do not run on its own, use PackRelease.ps1 instead.


!define APPNAME "Monofoxe"
!define APPVERSION "v2-dev"
!define INSTALLERVERSION "2.0.0.0-dev"

!define MUI_ICON "pics\icon.ico"
!define MUI_UNICON "pics\icon.ico"

!define PROJECT_TEMPLATES_DIRECTORY "Templates\ProjectTemplates\Visual C#\${APPNAME} ${APPVERSION}"
!define ITEM_TEMPLATES_DIRECTORY "Templates\ItemTemplates\Visual C#\${APPNAME} ${APPVERSION}"


!define REGISTRY_DIRECTORY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME} ${APPVERSION}"

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

Section "Visual Studio 2017 Templates/" VS2017
	SetOutPath "$DOCUMENTS\Visual Studio 2017\${PROJECT_TEMPLATES_DIRECTORY}"
	File /r '..\Release\ProjectTemplates\*.zip'
	SetOutPath "$DOCUMENTS\Visual Studio 2017\${ITEM_TEMPLATES_DIRECTORY}"
	File /r '..\Release\ItemTemplates\*.zip'
SectionEnd

Section "Visual Studio 2019 Templates/" VS2019
	SetOutPath "$DOCUMENTS\Visual Studio 2019\${PROJECT_TEMPLATES_DIRECTORY}"
	File /r '..\Release\ProjectTemplates\*.zip'
	SetOutPath "$DOCUMENTS\Visual Studio 2019\${ITEM_TEMPLATES_DIRECTORY}"
	File /r '..\Release\ItemTemplates\*.zip'
SectionEnd

!define OldMonofoxeInstallationDir '$PROGRAMFILES\Monofoxe\'
Section "Remove old versions." RemoveOldVersions
  RMDir /r "$DOCUMENTS\Visual Studio 2017\Templates\ProjectTemplates\Visual C#\Monofoxe"
  RMDir /r "$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\Visual C#\Monofoxe"
  Delete "${OldMonofoxeInstallationDir}\Uninstall.exe"
  RMDir /r "${OldMonofoxeInstallationDir}"
SectionEnd

; Stuff to install.


; Component menu.
LangString VS2017Desc ${LANG_ENGLISH} "Install project templates for Visual Studio 2017. Templates are required to create new projects."
LangString VS2019Desc ${LANG_ENGLISH} "Install project templates for Visual Studio 2019. Templates are required to create new projects."
LangString RemoveOldVersionsDesc ${LANG_ENGLISH} "Remove all previous Monofoxe versions."


!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${VS2017} $(VS2017Desc)
	!insertmacro MUI_DESCRIPTION_TEXT ${VS2019} $(VS2019Desc)
	!insertmacro MUI_DESCRIPTION_TEXT ${RemoveOldVersions} $(RemoveOldVersionsDesc)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

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
	Call checkVS2017
	Call checkVS2019
	IntOp $0 ${SF_SELECTED} | ${SF_RO}
FunctionEnd

