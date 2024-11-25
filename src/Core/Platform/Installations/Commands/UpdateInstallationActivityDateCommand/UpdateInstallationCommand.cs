namespace Bit.Core.Platform;

/// <summary>
/// Commands responsible for updating an installation from
/// `InstallationRepository`.
/// </summary>
/// <remarks>
/// If referencing: you probably want the interface
/// `IUpdateInstallationCommand` instead of directly calling this class.
/// </remarks>
/// <seealso cref="IUpdateInstallationCommand"/>
public class UpdateInstallationCommand : IUpdateInstallationCommand
{
    private readonly IGetInstallationQuery _getInstallationQuery;
    private readonly IInstallationRepository _installationRepository;

    public UpdateInstallationCommand(
        IGetInstallationQuery getInstallationQuery,
        IInstallationRepository installationRepository
    )
    {
        _getInstallationQuery = getInstallationQuery;
        _installationRepository = installationRepository;
    }

    public async Task UpdateLastActivityDateAsync(Guid installationId)
    {
        var installation = await _getInstallationQuery.GetByIdAsync(installationId);
        /*TODO: Update `LastActivityDate`*/
        await _installationRepository.UpsertAsync(installation);
    }
}
