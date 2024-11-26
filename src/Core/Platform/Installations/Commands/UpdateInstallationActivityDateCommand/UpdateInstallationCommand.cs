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
    private readonly TimeProvider _timeProvider;

    public UpdateInstallationCommand(
        IGetInstallationQuery getInstallationQuery,
        IInstallationRepository installationRepository,
        TimeProvider timeProvider
    )
    {
        _getInstallationQuery = getInstallationQuery;
        _installationRepository = installationRepository;
        _timeProvider = timeProvider;
    }

    public async Task UpdateLastActivityDateAsync(Guid installationId)
    {
        var installation = await _getInstallationQuery.GetByIdAsync(installationId);
        installation.LastActivityDate = _timeProvider.GetUtcNow().UtcDateTime;
        await _installationRepository.UpsertAsync(installation);
    }
}
