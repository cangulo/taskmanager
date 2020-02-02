#!/bin/bash
#   FileTitle
#   FileDescription

dotnet test ./Testing/TaskManagerAPI.CQRS.Test/*.csproj -p:CollectCoverage=true -p:CoverletOutputFormat=lcov -p:CoverletOutput='./TestResults/result.info' -p:ExcludeByFile=\"./**/Migrations/*.cs\"
reportgenerator.exe "-reports:./Testing/*.Test/TestResults/*.info" "-targetdir:./Testing/Results" -reporttypes:"Html;lcov"
for d in ./Testing/*.Test/TestResults;do rm -rf $d;done