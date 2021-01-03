$coveragePath = [System.IO.Path]::GetFullPath("$($PSScriptRoot)/../coverage/")
$params = New-Object System.Collections.ArrayList
[void] $params.Add('--no-build')
[void] $params.Add('/p:CollectCoverage=true')
[void] $params.Add('/p:CoverletOutputFormat=\`"lcov,opencover\`"')
[void] $params.Add("/p:CoverletOutput='$coveragePath'")
[void] $params.Add('/p:Threshold=95')

if (Test-Path -Path $coveragePath -PathType Container) {
  Remove-Item -Path $coveragePath -Recurse -Force
}

$expresionParams = $params -join ' '
# Write-Host $expresionParams
Invoke-Expression "dotnet test $expresionParams"
if ($LASTEXITCODE -ne 0) {
  exit $LASTEXITCODE
}
