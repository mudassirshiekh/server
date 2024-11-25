namespace Bit.Core.Platform;

/// <summary>
/// Command interface responsible for updating data on an `Installation`
/// record.
/// </summary>
/// <remarks>
/// This interface is implemented by `UpdateInstallationCommand`
/// </remarks>
/// <seealso cref="Bit.Core.Platform.Installations.Commands.UpdateInstallationCommand"/>
public interface IUpdateInstallationCommand
{
    Task UpdateLastActivityDateAsync(Guid installationId);
}
