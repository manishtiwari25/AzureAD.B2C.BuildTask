[CmdletBinding()]
param()

# For more information on the Azure DevOps Task SDK:
# https://github.com/Microsoft/vsts-task-lib
Trace-VstsEnteringInvocation $MyInvocation

# Set the working directory.
$B2CDomain = Get-VstsInput -Name b2cDomain -Require
$ClientId = Get-VstsInput -Name clientid -Require
$ClientSecret = Get-VstsInput -Name clientsecret -Require
try {
    Import-Module .\AzureAD.B2C.BuildTask.dll   

    New-CustomizeAndUploadPolicies  -b2CDomain "$B2CDomain" -clientId "$ClientId" -clientSecret "$ClientSecret" -taskType "Release"  -Verbose
}
catch{
    Write-Host $_.Exception.Message; 
}
finally {
    Trace-VstsLeavingInvocation $MyInvocation
}
