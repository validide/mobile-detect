#
# A set o utilities used to build the project
#

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