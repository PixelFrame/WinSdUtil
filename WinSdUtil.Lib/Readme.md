# WinSdUtil.Lib
Provide offline Windows security descriptor translation
No Windows runtime platform dependency

Current roadmap:
Binary <-> Managed object <-> SDDL

## Data Source
By default, this library looks up local SQLite database for well known SID information and Active Directory attribute/class GUID.
But in some cases, it is not very convenient to use a SQLite db (e.g. Blazor WASM), so a JSON data provider was added. To use the JSON data provider, you need to read the JSON files under Data directory. And then provide the JSON data to `WinSdConverter` constructor.

``` CSharp
var trusteeJsonData = await httpClient.GetStringAsync("data/WinSdTrustee.json");
var adGuidJsonData = await httpClient.GetStringAsync("data/WinSdAdGuid.json");
var sdConverter = new WinSdConverter(true, trusteeJsonData, adGuidJsonData);
var acl = sdConverter.FromSddlToAcl("O:SYG:SYD:NO_ACCESS_CONTROL");
```

The AD Schema GUID data is acquired from a fresh installed Windows Server 2025 domain controller.