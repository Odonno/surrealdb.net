param([string]$targetNetFramework)

$rootDirectory = Split-Path $PSScriptRoot -Parent
$publishOutput = dotnet publish $rootDirectory/SurrealDb.Examples.TodoApi.Aot/SurrealDb.Examples.TodoApi.Aot.csproj -nodeReuse:false /p:UseSharedCompilation=false /p:ExposeExperimentalFeatures=true

$actualWarningCount = 0

foreach ($line in $($publishOutput -split "`r`n"))
{
    if ($line -like "*analysis warning IL*")
    {
        Write-Host $line

        $actualWarningCount += 1
    }
}

pushd $rootDirectory/SurrealDb.Examples.TodoApi.Aot/bin/Release/$targetNetFramework/linux-x64

Write-Host "Executing test App..."
./native/SurrealDb.Examples.TodoApi.Aot 5000
Write-Host "Finished executing test App"

if ($LastExitCode -ne 0)
{
  Write-Host "There was an error while executing AotCompatibility Test App. LastExitCode is:", $LastExitCode
}

popd

Write-Host "Actual warning count is:", $actualWarningCount
$expectedWarningCount = 0

$testPassed = 0
if ($actualWarningCount -ne $expectedWarningCount)
{
    $testPassed = 1
    Write-Host "Actual warning count:", actualWarningCount, "is not as expected. Expected warning count is:", $expectedWarningCount
}

Exit $testPassed