#!/bin/bash
#   FileTitle
#   FileDescription

# dotnet test ./Testing/TaskManagerAPI.CQRS.Test/*.csproj -p:CollectCoverage=true -p:CoverletOutputFormat=lcov -p:CoverletOutput='./TestResults/result.info' -p:ExcludeByFile=\"./**/Migrations/*.cs\"
# for d in ./Testing/*.Test/*.csproj;do dotnet add $d package AltCover;done
dotnet test -p:AltCover=true  -p:AltCoverAssemblyExcludeFilter='^(?!(TaskManager))||((.Test)).*$'
reportgenerator.exe "-reports:./Testing/*.Test/coverage.xml" "-targetdir:./Testing/Results_altcover" -reporttypes:"Html;lcov"
for d in ./Testing/*.Test/coverage.xml;do rm -rf $d;done