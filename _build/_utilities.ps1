#
# A set o utilities used to build the project
#

function Patch-File([string] $filePath, [string] $match, [string] $replace) {
	(Get-Content $filePath) -replace $match, $replace | Out-File $filePath
}

function Patch-Nuspec-Version([string] $filePath, [string] $version) {
	$path = $filePath
	$match = '<version>.*<\/version>'
	$replace = "<version>$version</version>"
	
	Patch-File $path $match $replace
}

function Patch-ProjectJson-Version([string] $filePath, [string] $version) {
	$path = $filePath
	$match = '"version": ".*"'
	$replace = '"version": "' + $version + '"'
	
	Patch-File $path $match $replace
}


function Resolve-HeaderName-FromPhp([string] $phpHeader) {
    $header = $phpHeader.ToLowerInvariant();
    if ($header.StartsWith('http_')) {
        $length = 'http_'.Length; 
        $header = $header.Substring($length, $header.Length - $length)
    }

    $header = $header -replace '_', ' ' 
    $header = [cultureinfo]::InvariantCulture.TextInfo.ToTitleCase($header)
    $header = $header -replace ' ', '-'

    return $header
}