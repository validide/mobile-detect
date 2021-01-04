param(
  [Parameter(Mandatory = $false)]
  [string] $Rebuild = 'true',
  [Parameter(Mandatory = $false)]
  [string] $Configuration = 'Debug',
  [Parameter(Mandatory = $false)]
  [string] $SourceLink = 'true',
  [Parameter(Mandatory = $false)]
  [string] $GenerateReport = 'true'
)

$coveragePath = [System.IO.Path]::GetFullPath("$($PSScriptRoot)/../coverage/")
$params = New-Object System.Collections.ArrayList

if ($Rebuild -inotmatch 'true') {
  [void] $params.Add('--no-build')
}

if ($Configuration -imatch 'Debug') { # Check coverage only on Debug BUILD (this will be non deterministic)
  [void] $params.Add('/p:CollectCoverage=true')
  [void] $params.Add('/p:CoverletOutputFormat=\`"lcov,opencover\`"')
  [void] $params.Add("/p:CoverletOutput='$coveragePath'")
  # [void] $params.Add('/p:Include="[MobileDetect.*.]*"')
  [void] $params.Add('/p:Threshold=95')
  if ($Rebuild -imatch 'true') {
    [void] $params.Add('/p:UseSourceLink=true')
  }
}

if (Test-Path -Path $coveragePath -PathType Container) {
  Remove-Item -Path $coveragePath -Recurse -Force
}

$expresionParams = $params -join ' '
# Write-Host $expresionParams
Invoke-Expression "dotnet test --configuration $Configuration $expresionParams"
if ($LASTEXITCODE -ne 0) {
  exit $LASTEXITCODE
}

if ($Configuration -imatch 'Debug' -and $GenerateReport -imatch 'true') {
  # Generate the HTML report
  $testsProps = Get-Content -Raw -Path "$($PSScriptRoot)/../test/Directory.Build.props"

  if ($testsProps -imatch '.*Include="ReportGenerator"\s*Version="(.*)".*') {
    $reportGeneratorVersion = $matches[1]
    # Write-Host "reportGeneratorVersion: $reportGeneratorVersion"
    if ([System.String]::IsNullOrEmpty($userProfile)) {
      $userProfile = '~'
    }
    $userProfile = (Get-Item -Path $userProfile).FullName
    $exp = "dotnet $userProfile/.nuget/packages/reportgenerator/$reportGeneratorVersion/tools/net5.0/ReportGenerator.dll `"-reports:$coveragePath/**/*.opencover.xml`" `"-targetdir:$($coveragePath)report-generator`" `"-reporttypes:Html;HtmlSummary`""
    # Write-Host $exp
    Invoke-Expression $exp
  }
}
