#
# Script used to update the rules used to detect devices
#

. "$($PSScriptRoot)\_utilities.ps1"

# the version with the matching rules from https://github.com/serbanghita/Mobile-Detect
# when updating to a new version make sure the url matches
$jsonVersion = '2.8.34'

$jsonUrl = "https://raw.githubusercontent.com/serbanghita/Mobile-Detect/$jsonVersion/Mobile_Detect.json"
$userAgentTestStringsJsonUrl = "https://raw.githubusercontent.com/serbanghita/Mobile-Detect/$jsonVersion/tests/ualist.json"

$rulesData =  Invoke-WebRequest $jsonUrl |  ConvertFrom-Json
$rulesVersion = $rulesData | Select-Object -expand version

if ($jsonVersion -ne $rulesVersion) {
  Write-Error "Requested version ($jsonVersion) doesn't match response version($rulesVersion)"
  exit
}

Invoke-WebRequest $userAgentTestStringsJsonUrl | Select-Object -expand content | Out-File "$($PSScriptRoot)\..\test\MobileDetectTests\TestData\ua-tests.generated.json"

$templateFile = "$($PSScriptRoot)\Templates\DefaultRules.generated.txt"
$generatedFile = "$($PSScriptRoot)\..\src\MobileDetect\MatchingRules\DefaultRules.generated.cs"


$templateContent = Get-Content $templateFile

# update rules version
$templateContent = $templateContent -replace '\$template_version', $rulesVersion



#update mobileHeaders
$mobileHeadersData = $rulesData | Select-Object -expand headerMatch
$mobileHeaders = ''

$mobileHeadersData.psobject.properties |
ForEach-Object {
    $headerName = Resolve-HeaderName-FromPhp $_.Name
    $mobileHeaders += '{"' + $headerName + '", '

    if ($_.Value -ne $null) {
        
        $values = ''
        

        foreach($val in $_.Value | Select-Object -expand matches) {
            $values += '@"' + $val + '", '
        }

        if ($values.Length -gt 0) {
            $values = $values.Substring(0,$values.Length-(', ').Length)            
        }

        if ($values.Length -gt 0) {
            $mobileHeaders += 'new [] {'
            $mobileHeaders += $values
            $mobileHeaders += '}'
        } else {
            $mobileHeaders += 'null'        
        }
    } else {
        $mobileHeaders += 'null'
    }

    $mobileHeaders += ' },' + "`r`n`t`t`t"
}

if ($mobileHeaders.Length -gt 0) {
    $mobileHeaders = $mobileHeaders.Substring(0,$mobileHeaders.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_mobileHeaders', $mobileHeaders

#update userAgentHeaders
$userAgentHeadersData = $rulesData | Select-Object -expand uaHttpHeaders
$userAgentHeaders = ''

if ($null -ne $userAgentHeadersData) {
        
    foreach($val in $userAgentHeadersData) {
        $headerName = Resolve-HeaderName-FromPhp $val
        $userAgentHeaders += '@"' + $headerName + '",' + "`r`n`t`t`t"
    }

    if ($userAgentHeaders.Length -gt 0) {
        $userAgentHeaders = $userAgentHeaders.Substring(0,$userAgentHeaders.Length-(',' + "`r`n`t`t`t").Length)
    }

    if ($userAgentHeaders.Length -eq 0) {        
        $userAgentHeaders += 'User-Agent'
    }
} else {
    $userAgentHeaders += 'User-Agent'
}

$templateContent = $templateContent -replace '\$template_userAgentHeaders', $userAgentHeaders


#update userAgentMatches
$userAgentMatchesData = $rulesData | Select-Object -expand uaMatch

$userAgentMatchesData_phones = $userAgentMatchesData | Select-Object -expand phones
$userAgentMatchesData_tablets = $userAgentMatchesData | Select-Object -expand tablets
$userAgentMatchesData_browsers = $userAgentMatchesData | Select-Object -expand browsers
$userAgentMatchesData_os = $userAgentMatchesData | Select-Object -expand os
$userAgentMatchesData_utilities = $userAgentMatchesData | Select-Object -expand utilities


$phones = ''
$userAgentMatchesData_phones.psobject.properties |
ForEach-Object {
    $phones += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($phones.Length -gt 0) {
    $phones = $phones.Substring(0,$phones.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_phones', $phones


$tablets = ''
$userAgentMatchesData_tablets.psobject.properties |
ForEach-Object {
    $tablets += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($tablets.Length -gt 0) {
    $tablets = $tablets.Substring(0,$tablets.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_tablets', $tablets

$browsers = ''
$userAgentMatchesData_browsers.psobject.properties |
ForEach-Object {
    $browsers += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($browsers.Length -gt 0) {
    $browsers = $browsers.Substring(0,$browsers.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_browsers', $browsers

$os = ''
$userAgentMatchesData_os.psobject.properties |
ForEach-Object {
    $os += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($os.Length -gt 0) {
    $os = $os.Substring(0,$os.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_os', $os


$utilities = ''
$userAgentMatchesData_utilities.psobject.properties |
ForEach-Object {
    $utilities += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($utilities.Length -gt 0) {
    $utilities = $utilities.Substring(0,$utilities.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_utilities', $utilities
$templateContent | Out-File $generatedFile


Write-Host 'Done!'