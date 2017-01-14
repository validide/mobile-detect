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

$templatesRoot = '.\Templates\'
$rulesRoot = '..\MobileDetect\MatchingRules\'

# update rules version
(Get-Content ($templatesRoot + 'DefaultRules.generated.Version.txt')) -replace '\$version', $rulesVersion | Out-File ($rulesRoot + 'DefaultRules.generated.Version.cs')



#update mobileHeaders
$mobileHeadersData = $rulesData | select -expand headerMatch
$mobileHeaders = ''

$mobileHeadersData.psobject.properties |
ForEach {
    $mobileHeaders += '{"' + $_.Name + '", '

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

(Get-Content ($templatesRoot + 'DefaultRules.generated.MobileHeaders.txt')) -replace '\$mobileHeaders', $mobileHeaders | Out-File ($rulesRoot + 'DefaultRules.generated.MobileHeaders.cs')

#update userAgentHeaders
$userAgentHeadersData = $rulesData | select -expand uaHttpHeaders
$userAgentHeaders = ''

if ($userAgentHeadersData -ne $null) {
        
    foreach($val in $userAgentHeadersData) {
        $userAgentHeaders += '@"' + $val + '",' + "`r`n`t`t`t"
    }

    if ($userAgentHeaders.Length -gt 0) {
        $userAgentHeaders = $userAgentHeaders.Substring(0,$userAgentHeaders.Length-(',' + "`r`n`t`t`t").Length)
    }

    if ($userAgentHeaders.Length -eq 0) {        
        $userAgentHeaders += 'HTTP_USER_AGENT'
    }
} else {
    $userAgentHeaders += 'HTTP_USER_AGENT'
}

(Get-Content ($templatesRoot + 'DefaultRules.generated.UserAgentHeaders.txt')) -replace '\$userAgentHeaders', $userAgentHeaders | Out-File ($rulesRoot + 'DefaultRules.generated.UserAgentHeaders.cs')


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

(Get-Content ($templatesRoot + 'DefaultRules.generated.Phones.txt')) -replace '\$phones', $phones | Out-File ($rulesRoot + 'DefaultRules.generated.Phones.cs')


$tablets = ''
$userAgentMatchesData_tablets.psobject.properties |
ForEach {
    $tablets += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($tablets.Length -gt 0) {
    $tablets = $tablets.Substring(0,$tablets.Length-(',' + "`r`n`t`t`t").Length)
}

(Get-Content ($templatesRoot + 'DefaultRules.generated.Tablets.txt')) -replace '\$tablets', $tablets | Out-File ($rulesRoot + 'DefaultRules.generated.Tablets.cs')


$browsers = ''
$userAgentMatchesData_browsers.psobject.properties |
ForEach {
    $browsers += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($browsers.Length -gt 0) {
    $browsers = $browsers.Substring(0,$browsers.Length-(',' + "`r`n`t`t`t").Length)
}

(Get-Content ($templatesRoot + 'DefaultRules.generated.Browsers.txt')) -replace '\$browsers', $browsers | Out-File ($rulesRoot + 'DefaultRules.generated.Browsers.cs')


$os = ''
$userAgentMatchesData_os.psobject.properties |
ForEach {
    $os += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($os.Length -gt 0) {
    $os = $os.Substring(0,$os.Length-(',' + "`r`n`t`t`t").Length)
}

(Get-Content ($templatesRoot + 'DefaultRules.generated.Os.txt')) -replace '\$os', $os | Out-File ($rulesRoot + 'DefaultRules.generated.Os.cs')

$utilities = ''
$userAgentMatchesData_utilities.psobject.properties |
ForEach {
    $utilities += '{"' + $_.Name + '", @"' + $_.Value + '" },' + "`r`n`t`t`t"
}

if ($utilities.Length -gt 0) {
    $utilities = $utilities.Substring(0,$utilities.Length-(',' + "`r`n`t`t`t").Length)
}

(Get-Content ($templatesRoot + 'DefaultRules.generated.Utilities.txt')) -replace '\$utilities', $utilities | Out-File ($rulesRoot + 'DefaultRules.generated.Utilities.cs')


Write-Host 'Done!'