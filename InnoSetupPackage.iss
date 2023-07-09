; Inno Setup script for building the setup package.  See https://jrsoftware.org/isinfo.php

#define MyAppName "Quartz Windows Service Demo"
#define MyAppExeName "QuartzWindowsServiceApp.exe"

#ifndef BuildConfiguration
  #define BuildConfiguration "Release"
#endif

#ifndef SetupFileVersion
  #define SetupFileVersion "1.0.0.0"
#endif SetupFileVersion

#ifndef SetupProductVersion
  #define SetupProductVersion "1.0.0.0"
#endif SetupProductVersion

[Setup]
WizardStyle=modern

AppName={#MyAppName}

; VersionInfoVersion must be in the format of #.#.#.#.  This will be set as the "File version" for the setup exe.
VersionInfoVersion={#SetupFileVersion}

; AppVersion can contain alpha-numeric characters.  It's used in wizard dialogs, and set as the "Product version" for the setup exe.
AppVersion={#SetupProductVersion}

DefaultDirName={autopf}\QuartzWindowsServiceDemo

OutputDir=InnoSetupOutput
OutputBaseFileName=SetupQuartzWindowsServiceDemo

ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
UninstallDisplayName={#MyAppName} {#SetupFileVersion}

SolidCompression=yes
SetupLogging=yes
DisableDirPage=yes
DisableProgramGroupPage=yes

[Messages]
; See Default.isl in the Inno Setup installation folder for all available messages.
ReadyLabel2b=Click Install to continue with the installation.%n%nThis setup assumes .NET 7 Runtime has already been installed.
FinishedLabelNoIcons=Setup has finished installing [name] on your computer.%n%nNote: The setup application does not start {#MyAppName}.  Use the Services Control Panel to start the service.

[Files]
; Note: Don't use "Flags: ignoreversion" on any shared system files
Source: "src\QuartzWindowsServiceApp\bin\{#BuildConfiguration}\net7.0\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Run]
Filename: "sc.exe"; Parameters: "create ""{#MyAppName}"" binPath=""{app}\{#MyAppExeName}"" start=demand DisplayName=""{#MyAppName}"""; Flags: runhidden

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop ""{#MyAppName}"""; RunOnceId: "QuartzWindowsServiceDemoStop1"; Flags: runhidden
Filename: "sc.exe"; Parameters: "delete ""{#MyAppName}"""; RunOnceId: "QuartzWindowsServiceDemoDelete1"; Flags: runhidden
