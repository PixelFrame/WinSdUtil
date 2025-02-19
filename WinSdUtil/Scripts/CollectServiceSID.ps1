# Windows PowerShell lacks service description in Get-Service, so needs PS7
#requires -PSEdition Core

$Win32 = @"
using System;
using System.Runtime.InteropServices;

public class ADVAPI32
{
    [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)][return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool LookupAccountName(
        string lpSystemName,
        string lpAccountName,
        IntPtr Sid,
        ref uint cbSid,
        IntPtr ReferencedDomainName,
        ref uint cchReferencedDomainName,
        out SID_NAME_USE peUse
    );

    [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)][return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ConvertSidToStringSid(
        IntPtr Sid,
        out string StringSid
    );

    public enum SID_NAME_USE
    {
        SidTypeUser = 1,
        SidTypeGroup,
        SidTypeDomain,
        SidTypeAlias,
        SidTypeWellKnownGroup,
        SidTypeDeletedAccount,
        SidTypeInvalid,
        SidTypeUnknown,
        SidTypeComputer
    }
}
"@

Add-Type -TypeDefinition $Win32 -Language CSharp -ErrorAction SilentlyContinue

function GetSID {
    param(
        [string]$AccountName
    )
    $Sid = [System.Runtime.InteropServices.Marshal]::AllocHGlobal(1024)
    $cbSid = 1024
    $ReferencedDomainName = [System.Runtime.InteropServices.Marshal]::AllocHGlobal(2048)
    $cchReferencedDomainName = 1024
    $peUse = [ADVAPI32+SID_NAME_USE]::SidTypeUser
    $Result = [ADVAPI32]::LookupAccountName(
        $null,
        $AccountName,
        $Sid,
        [ref]$cbSid,
        $ReferencedDomainName,
        [ref]$cchReferencedDomainName,
        [ref]$peUse
    )
    if (!$Result)
    {
        Write-Host "$($AccountName): LookupAccountName failed with error code: $([System.Runtime.InteropServices.Marshal]::GetLastWin32Error())"
    }
    $StringSid = ""
    $Result = [ADVAPI32]::ConvertSidToStringSid($Sid, [ref]$StringSid)
    if (!$Result)
    {
        Write-Error "ConvertSidToStringSid failed with error code: $([System.Runtime.InteropServices.Marshal]::GetLastWin32Error())"
    }

    [System.Runtime.InteropServices.Marshal]::FreeHGlobal($Sid)
    [System.Runtime.InteropServices.Marshal]::FreeHGlobal($ReferencedDomainName)

    return $StringSid
}

$Services = Get-Service -ErrorAction SilentlyContinue
$Result = @()

if(Test-Path $PSScriptRoot\..\Data\ServiceTrustee.json)
{
    # Load existing data
    $Result = Get-Content $PSScriptRoot\..\Data\ServiceTrustee.json | ConvertFrom-Json
}

foreach($svc in $Services)
{
    if(($svc.ServiceType -band 64) -eq 64)
    {
        # Skip per-user services
        Write-Host "Skipping per-user service $($svc.Name)"
        continue
    }
    if($Result | Where-Object { $_.Name -eq $svc.Name })
    {
        # Skip existing services
        Write-Host "Skipping existing service $($svc.Name)"
        continue
    }
    Write-Host "Adding service $($svc.Name)"
    $SID = GetSID "NT Service\$($svc.Name)"
    $Result += @{
        Name = $svc.Name;
        DisplayName = $svc.DisplayName;
        Description = $svc.Description;
        Sid = $SID;
        SddlName = $SID;
        IsLocal = 1;
        Domain = $null;
    }
}

$Result | ConvertTo-Json | Out-File $PSScriptRoot\..\Data\ServiceTrustee.json