SetCompressor /SOLID /FINAL lzma

!define APPNAME "Monofoxe"
!define NPL_APPNAME "NoPipeline"
!define INSTALLERVERSION "1.0.0.0"

!define MUI_ICON "pics\icon.ico"
!define MUI_UNICON "pics\icon.ico"


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
InstallDir '$PROGRAMFILES\${APPNAME}\' ; Main install directory.

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
	SetOutPath '$INSTDIR'
  ; Uninstaller.
  WriteUninstaller "uninstall.exe"
SectionEnd

Section "MonoGame" Monogame
	SetOutPath '$INSTDIR'
	File "Externals\MonoGameSetup.exe"
	ExecWait "$INSTDIR\MonoGameSetup.exe"
	Delete "$INSTDIR\MonoGameSetup.exe"
SectionEnd

Section "NoPipeline" NoPipeline
	SetOutPath '$INSTDIR'
	File "Externals\NoPipelineSetup.exe"
	ExecWait "$INSTDIR\NoPipelineSetup.exe"
	Delete "$INSTDIR\NoPipelineSetup.exe"
SectionEnd

Section "Visual Studio 2015 Templates" VS2015

  IfFileExists `$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\*.*` InstallTemplates CannotInstallTemplates
  InstallTemplates:
    SetOutPath "$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\Monofoxe"
    File /r '..\Release\MonofoxeDesktopGL.zip'
    GOTO EndTemplates
  CannotInstallTemplates:
    DetailPrint "Visual Studio 2015 not found"
  EndTemplates:

SectionEnd

Section "Visual Studio 2017 Templates" VS2017

  IfFileExists `$DOCUMENTS\Visual Studio 2017\Templates\ProjectTemplates\*.*` InstallTemplates CannotInstallTemplates
  InstallTemplates:
    SetOutPath "$DOCUMENTS\Visual Studio 2017\Templates\ProjectTemplates\Visual C#\Monofoxe"
    File /r '..\Release\MonofoxeDesktopGL.zip'
    GOTO EndTemplates
  CannotInstallTemplates:
    DetailPrint "Visual Studio 2017 not found"
  EndTemplates:

SectionEnd

Section "Visual Studio 2019 Templates" VS2019

  IfFileExists `$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\*.*` InstallTemplates CannotInstallTemplates
  InstallTemplates:
    SetOutPath "$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\Visual C#\Monofoxe"
    File /r '..\Release\MonofoxeDesktopGL.zip'
    GOTO EndTemplates
  CannotInstallTemplates:
    DetailPrint "Visual Studio 2019 not found"
  EndTemplates:

SectionEnd
; Stuff to install.



; Component menu.
LangString NoPipelineDesc ${LANG_ENGLISH} "Install Monofoxe!"
LangString NoPipelineDesc ${LANG_ENGLISH} "Install NoPipeline."
LangString MonogameDesc ${LANG_ENGLISH} "Install MonoGame 3.7.1. "
LangString VS2015Desc ${LANG_ENGLISH} "Install the project templates for Visual Studio 2015"
LangString VS2017Desc ${LANG_ENGLISH} "Install the project templates for Visual Studio 2017"
LangString VS2019Desc ${LANG_ENGLISH} "Install the project templates for Visual Studio 2019"

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${Monofoxe} $(MonofoxeDesc)
  !insertmacro MUI_DESCRIPTION_TEXT ${NoPipeline} $(NoPipelineDesc)
  !insertmacro MUI_DESCRIPTION_TEXT ${Monogame} $(MonogameDesc)
  !insertmacro MUI_DESCRIPTION_TEXT ${VS2015} $(VS2015Desc)
  !insertmacro MUI_DESCRIPTION_TEXT ${VS2017} $(VS2017Desc)
  !insertmacro MUI_DESCRIPTION_TEXT ${VS2019} $(VS2019Desc)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Function checkMonogame
IfFileExists `$PROGRAMFILES\MonoGame\v3.0\*.*` disable end
  disable:
	 SectionSetFlags ${Monogame} $1
  end:
FunctionEnd

Function checkNoPipeline
IfFileExists `$PROGRAMFILES\NoPipeline\*.*` disable end
  disable:
	 SectionSetFlags ${NoPipeline} $1
  end:
FunctionEnd

Function checkVS2015
IfFileExists `$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\*.*` end disable
  disable:
	 SectionSetFlags ${VS2015} $0
  end:
FunctionEnd

Function checkVS2017
IfFileExists `$DOCUMENTS\Visual Studio 2017\Templates\ProjectTemplates\*.*` end disable
  disable:
	 SectionSetFlags ${VS2017} $0
  end:
FunctionEnd

Function checkVS2019
IfFileExists `$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\*.*` end disable
  disable:
	 SectionSetFlags ${VS2019} $0
  end:
FunctionEnd
; Component menu.

Function .onInit
  IntOp $0 $0 | ${SF_RO}
  Call checkMonogame
  Call checkNoPipeline
  Call checkVS2015
  Call checkVS2017
  Call checkVS2019
  IntOp $0 ${SF_SELECTED} | ${SF_RO}
FunctionEnd

; Uninstaller Section

Section "Uninstall"
  RMDir /r "$DOCUMENTS\Visual Studio 2015\Templates\ProjectTemplates\Visual C#\Monofoxe"
  RMDir /r "$DOCUMENTS\Visual Studio 2017\Templates\ProjectTemplates\Visual C#\Monofoxe"
  RMDir /r "$DOCUMENTS\Visual Studio 2019\Templates\ProjectTemplates\Visual C#\Monofoxe"
  Delete "$INSTDIR\Uninstall.exe"
  RMDir /r "$INSTDIR"
SectionEnd



