namespace Bit.Core.Vault.Models.Data;

/// <summary>
/// Data model that represents a Users permissions for a given cipher
/// that belongs to an organization.
/// To be used internally for authorization.
/// </summary>
public class OrganizationCipherPermission
{
    public Guid Id { get; set; }
    public Guid Organization { get; set; }
    public bool Edit { get; set; }
    public bool ViewPassword { get; set; }
    public bool Manage { get; set; }
    public bool Unassigned { get; set; }
}
