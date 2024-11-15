using Bit.Core.Context;
using Microsoft.AspNetCore.Authorization;

namespace Bit.Core.Vault.Authorization.SecurityTasks;

public class SecurityTaskOrganizationAuthorizationHandler : AuthorizationHandler<SecurityTaskOperationRequirement, CurrentContextOrganization>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SecurityTaskOperationRequirement requirement,
        CurrentContextOrganization resource) =>
        throw new NotImplementedException();
}
