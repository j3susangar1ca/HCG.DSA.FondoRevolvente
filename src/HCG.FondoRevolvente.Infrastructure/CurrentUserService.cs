using HCG.FondoRevolvente.Application.Interfaces;

namespace HCG.FondoRevolvente.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    public string? UserId => "system-user-id";
    public string? UserName => "System";
    public bool IsAuthenticated => true;
}
