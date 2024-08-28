@ECHO OFF

dotnet publish -c Release -o publish -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true -p:DebugSymbols=false -p:DebugType=none --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true