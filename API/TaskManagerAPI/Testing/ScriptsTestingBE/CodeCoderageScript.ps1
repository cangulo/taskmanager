# List all the csproj files
$fullPathsFiles = Get-ChildItem -Path ./ -Recurse -Filter "*Test.csproj" | ForEach-Object -Process {$_.FullName}

foreach ($projectFilePath in $fullPathsFiles) {
    $projectFileName = Split-Path -Path $projectFilePath -Leaf -Resolve
    $projectName = $projectFileName -replace ".Test.csproj"

    Write-Host("#######     " + $projectName + "    Executing Test and creating .coverage file" );
    dotnet test $projectFilePath -r ./Testing/Results/$projectName --collect "Code Coverage" 

    Write-Host("#######     " + $projectName + "    Converting .coverage to .coveragexml" );
    $coverageFullPath = Get-ChildItem -Path ./Testing/Results/$projectName -Recurse -Filter "*.coverage" | ForEach-Object -Process {$_.FullName}
    C:\Users\carlo\.nuget\packages\microsoft.codecoverage\16.4.0\build\netstandard1.0\CodeCoverage\CodeCoverage.exe analyze /output:.\Testing\Results\$projectName.coveragexml $coverageFullPath

    Remove-Item -Path .\Testing\Results\$projectName -Recurse
    
    dotnet C:\Users\carlo\.nuget\packages\reportgenerator\4.4.6\tools\netcoreapp3.0\ReportGenerator.dll -reports:.\Testing\Results\$projectName.coveragexml -targetdir:.\Testing\Results\$projectName\

    Remove-Item -Path .\Testing\Results\$projectName.coveragexml
}

# # Create default report files (.coverage)
# dotnet test .\Testing\TaskManagerAPI.CQRS.Test\TaskManagerAPI.CQRS.Test.csproj -r ./Testing/Results/ --collect "Code Coverage"

# # Convert reports files from '.coverage' to '.coveragexml'
# C:\Users\carlo\.nuget\packages\microsoft.codecoverage\16.4.0\build\netstandard1.0\CodeCoverage\CodeCoverage.exe analyze /output:.\Testing\Results\CodeCoverage.coveragexml .\Testing\Results\97c3e003-56b9-41a8-b9f3-b976fc56fcda\carlo_DESKTOP-M7J1NMP_2020-02-02.17_52_02.coverage

# # Convert reports files from '.coveragexml' to '.htm'
# dotnet C:\Users\carlo\.nuget\packages\reportgenerator\4.4.6\tools\netcoreapp3.0\ReportGenerator.dll "-reports:.\Testing\Results\CodeCoverage.coveragexml" "-targetdir:.\Testing\Results\CodeCoverage-02-02-20"
