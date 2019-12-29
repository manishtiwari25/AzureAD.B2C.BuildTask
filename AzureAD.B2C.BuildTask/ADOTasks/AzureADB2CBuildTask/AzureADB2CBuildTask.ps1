[CmdletBinding()]
param()

# For more information on the Azure DevOps Task SDK:
# https://github.com/Microsoft/vsts-task-lib
Trace-VstsEnteringInvocation $MyInvocation

# Set the working directory.
$DirectoryPath = Get-VstsInput -Name directoryPath -Require
$JSON = Get-VstsInput -Name json -Require
try {
    Import-Module .\AzureAD.B2C.BuildTask.dll   

    New-CustomizeAndUploadPolicies -directoryPath "$DirectoryPath" -JSON "$Json" -taskType "Build"  -Verbose
}
catch{
    Write-Host $_.Exception.Message; 
}
finally {
    Trace-VstsLeavingInvocation $MyInvocation
}
