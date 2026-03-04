using System.Security.Claims;
using DotEmilu.EntityFrameworkCore;

namespace ProductRegistry.Api;

internal sealed class ContextUser(IHttpContextAccessor contextAccessor) : IContextUser<Guid>
{
    public Guid Id
        => Guid.TryParse(contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id
            : Guid.Empty;
}