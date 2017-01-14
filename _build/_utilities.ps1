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
