#
# Script used to update the rules used to detect devices
#

. .\_utilities.ps1

# the version with the matching rules from https://github.com/serbanghita/Mobile-Detect
# when updating to a new version make sure the url matches
$jsonVersion = '2.8.24'

$jsonUrl = "https://raw.githubusercontent.com/serbanghita/Mobile-Detect/$jsonVersion/Mobile_Detect.json"

$rulesData =  Invoke-WebRequest $jsonUrl | ConvertFrom-Json
$rulesVersion = $rulesData | select -expand version

if ($jsonVersion -ne $rulesVersion) {
  Write-Error "Requested version ($jsonVersion) doesn't match response version($rulesVersion)"
  exit
}

$templateFile = '.\Templates\DefaultRules.generated.txt'
$generatedFile = '..\MobileDetect\MatchingRules\DefaultRules.generated.cs'


$templateContent = Get-Content $templateFile

# update rules version
$templateContent = $templateContent -replace '\$template_version', $rulesVersion



#update mobileHeaders
$mobileHeadersData = $rulesData | select -expand headerMatch
$mobileHeaders = ''

$mobileHeadersData.psobject.properties |
ForEach {
    $headerName = Resolve-HeaderName-FromPhp $_.Name
    $mobileHeaders += '{"' + $headerName + '", '

    if ($_.Value -ne $null) {
        
        $values = ''
        

        foreach($val in $_.Value | select -expand matches) {
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
$userAgentHeadersData = $rulesData | select -expand uaHttpHeaders
$userAgentHeaders = ''

if ($userAgentHeadersData -ne $null) {
        
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
$userAgentMatchesData = $rulesData | select -expand uaMatch

$userAgentMatchesData_phones = $userAgentMatchesData | select -expand phones
$userAgentMatchesData_tablets = $userAgentMatchesData | select -expand tablets
$userAgentMatchesData_browsers = $userAgentMatchesData | select -expand browsers
$userAgentMatchesData_os = $userAgentMatchesData | select -expand os
$userAgentMatchesData_utilities = $userAgentMatchesData | select -expand utilities


$phones = ''
$userAgentMatchesData_phones.psobject.properties |
ForEach {
    $phones += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($phones.Length -gt 0) {
    $phones = $phones.Substring(0,$phones.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_phones', $phones


$tablets = ''
$userAgentMatchesData_tablets.psobject.properties |
ForEach {
    $tablets += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($tablets.Length -gt 0) {
    $tablets = $tablets.Substring(0,$tablets.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_tablets', $tablets

$browsers = ''
$userAgentMatchesData_browsers.psobject.properties |
ForEach {
    $browsers += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($browsers.Length -gt 0) {
    $browsers = $browsers.Substring(0,$browsers.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_browsers', $browsers

$os = ''
$userAgentMatchesData_os.psobject.properties |
ForEach {
    $os += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($os.Length -gt 0) {
    $os = $os.Substring(0,$os.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_os', $os


$utilities = ''
$userAgentMatchesData_utilities.psobject.properties |
ForEach {
    $utilities += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($utilities.Length -gt 0) {
    $utilities = $utilities.Substring(0,$utilities.Length-(',' + "`r`n`t`t`t").Length)
}

$templateContent = $templateContent -replace '\$template_utilities', $utilities
$templateContent | Out-File $generatedFile


Write-Host 'Done!'