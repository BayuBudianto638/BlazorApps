using Microsoft.AspNetCore.Authorization;
using Shared.Authorization;

namespace Client.Infrastructure.Auth
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) =>
            Policy = FSHPermission.NameFor(action, resource);
    }
}