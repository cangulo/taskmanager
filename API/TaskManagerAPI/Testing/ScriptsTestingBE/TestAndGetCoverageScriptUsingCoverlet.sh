#!/bin/bash
#   FileTitle
#   FileDescription

# TaskManagerAPI.BL.Test
# TaskManagerAPI.CQRS.Test        
# TaskManagerAPI.Models.Test      
# TaskManagerAPI.Repositories.Test
# TaskManagerAPI.Test
# TaskManagerAPI.Test.Common 

test_projects=$(ls ./Testing/ | grep TaskManagerAPI.*.Test)

for i in ${test_projects[@]}; do
    echo "Testing project "$i
    dotnet test ./Testing/$i/$i.csproj -p:CollectCoverage=true -p:CoverletOutputFormat=lcov -p:CoverletOutput=./$i/TestResultsCoverlet/result.info
    reportgenerator.exe "-reports:./Testing/$i/TestResults/result.info" "-targetdir:./Testing/Results_coverlet" -reporttypes:"Html;lcov"
    rm -rf 
done

for d in ./Testing/*/TestResultsCoverlet;do rm -rf $d;done