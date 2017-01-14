param([string]$version=[string]::Empty, [string]$nuGetPath=[string]::Empty)
#
# create_package.ps1
#

. .\_utilities.ps1

if ([string]::IsNullOrEmpty($nuGetPath)) {
	$nuGetPath = 'C:\NuGet\nuget.exe'
}

$packageVersion = $version
if ([string]::IsNullOrEmpty($version)) {
	$packageVersion = Read-Host 'Enter Package Version: '
}

Patch-Nuspec-Version '.\MobileDetect.nuspec' $packageVersion
Patch-ProjectJson-Version '..\MobileDetect\project.json' $packageVersion

$foldersToClean = @(
    '..\MobileDetect\bin'
    '..\MobileDetect\obj'
    '.\lib'
)

$foldersToClean |
Where-Object { Test-Path $_ } |
ForEach-Object { Remove-Item $_ -Recurse -Force -ErrorAction Stop }

dotnet restore ..\MobileDetect
dotnet build ..\MobileDetect --configuration release --build-base-path ..\_build\lib

if( -not $? )
{
    $msg = $Error[0].Exception.Message
    "Error: $msg. Please check."
    exit
}

Invoke-Expression "$nuGetPath pack MobileDetect.nuspec -symbols  -version $packageVersion -verbosity detailed"

$foldersToClean |
Where-Object { Test-Path $_ } |
ForEach-Object { Remove-Item $_ -Recurse -Force -ErrorAction Stop }


Write-Host "Done packing version '$packageVersion'" 