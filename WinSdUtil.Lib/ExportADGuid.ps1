$DomainDN = 'DC=contoso,DC=com'

Import-Module ActiveDirectory
$attribSchemas = Get-ADObject -SearchBase "CN=Schema,CN=Configuration,$DomainDN" -LdapFilter '(ObjectClass=attributeSchema)' -Properties lDAPDisplayName, schemaIDGUID
$classSchemas = Get-ADObject -SearchBase "CN=Schema,CN=Configuration,$DomainDN" -LdapFilter '(ObjectClass=classSchema)' -Properties lDAPDisplayName, schemaIDGUID
$extendedRights = Get-ADObject -SearchBase "CN=Extended-Rights,CN=Configuration,$DomainDN" -LdapFilter '(ObjectClass=controlAccessRight)' -Properties displayName, rightsGuid
$result = @()
foreach($attribSchema in $attribSchemas)
{
    $result += [PSCustomObject]@{
        DisplayName = $attribSchema.lDAPDisplayName;
        Guid = [Guid]::new($attribSchema.schemaIDGUID);
        Type = 'Attribute'
    }
}
foreach($classSchema in $classSchemas)
{
    $result += [PSCustomObject]@{
        DisplayName = $classSchema.lDAPDisplayName;
        Guid = [Guid]::new($classSchema.schemaIDGUID);
        Type = 'Class'
    }
}
foreach($extendedRight in $extendedRights)
{
    $result += [PSCustomObject]@{
        DisplayName = $extendedRight.displayName;
        Guid = [Guid]::new($extendedRight.rightsGuid);
        Type = 'ExtendedRights'
    }
}
$result | ConvertTo-Json