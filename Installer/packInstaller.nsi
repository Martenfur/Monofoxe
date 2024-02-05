; Creates the installer.
; NOTE: Do not run on its own, use PackRelease.ps1 instead.


!define APPNAME "Monofoxe"
!define APPVERSION "v3-dev"
!define INSTALLERVERSION "3.0.0.0-dev.1"

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

Section "Visual Studio 2022 Templates/" VS2022
	SetOutPath "$DOCUMENTS\Visual Studio 2022\${PROJECT_TEMPLATES_DIRECTORY}"
	File /r '..\Release\ProjectTemplates\*.zip'
	SetOutPath "$DOCUMENTS\Visual Studio 2022\${ITEM_TEMPLATES_DIRECTORY}"
	File /r '..\Release\ItemTemplates\*.zip'
SectionEnd

!define OldMonofoxeInstallationDir '$PROGRAMFILES\Monofoxe\'
!define OldMonofoxeInstallationDirV2 '$PROGRAMFILES\Monofoxe Engine\'
Section "Remove old versions." RemoveOldVersions
  RMDir /r "$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\Monofoxe"
  RMDir /r "$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\Visual C#\Monofoxe"
  RMDir /r "$DOCUMENTS\Visual Studio 2022\Templates\ProjectTemplates\Visual C#\Monofoxe"
	RMDir /r "$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\Monofoxe v2-dev"
	RMDir /r "$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\Visual C#\Monofoxe v2-dev"
  RMDir /r "$DOCUMENTS\Visual Studio 2022\Templates\ProjectTemplates\Visual C#\Monofoxe v2-dev"
	
  Delete "${OldMonofoxeInstallationDir}\Uninstall.exe"
  RMDir /r "${OldMonofoxeInstallationDir}"
	RMDir /r "${OldMonofoxeInstallationDirV2}"
SectionEnd

; Stuff to install.


; Component menu.
LangString VS2022Desc ${LANG_ENGLISH} "Install project templates for Visual Studio 2022. Templates are required to create new projects."
LangString RemoveOldVersionsDesc ${LANG_ENGLISH} "Remove all previous Monofoxe versions."


!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${VS2022} $(VS2022Desc)
	!insertmacro MUI_DESCRIPTION_TEXT ${RemoveOldVersions} $(RemoveOldVersionsDesc)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Function checkVS2022
IfFileExists `$DOCUMENTS\Visual Studio 2022\Templates\ProjectTemplates\*.*` end disable
	disable:
		SectionSetFlags ${VS2022} $1
	end:
FunctionEnd
; Component menu.

Function .onInit
	IntOp $0 $0 | ${SF_RO}
	Call checkVS2022
	IntOp $0 ${SF_SELECTED} | ${SF_RO}
FunctionEnd

