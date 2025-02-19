# WinSdUtil

Provide offline Windows security descriptor translation without Windows runtime platform dependency

## Data Source

Trustee data is consist of well-known SIDs from documentation below and service SIDs (S-1-5-80) collected from Windows 11 24H2 and Windows Server 2025

https://learn.microsoft.com/en-us/windows/win32/secauthz/well-known-sids

Packaged application trustee (S-1-15-2 and S-1-15-3) data is still under consideration...

The AD Schema GUID data is acquired from a fresh installed Windows Server 2025 domain controller.