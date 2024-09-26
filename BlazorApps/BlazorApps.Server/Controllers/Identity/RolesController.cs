using Application.Identity.Roles;
using Shared.RequestModels.Identity;
using Shared.ResponceModels.Department;

namespace BlazorApps.Server.Controllers.Identity
{
    public class RolesController : VersionNeutralApiController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService) => _roleService = roleService;

        [HttpGet]
        [MustHavePermission(FSHAction.View, FSHResource.Roles)]
        [OpenApiOperation("Get a list of all roles.", "")]
        public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
        {
            return _roleService.GetListAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        [MustHavePermission(FSHAction.View, FSHResource.Roles)]
        [OpenApiOperation("Get role details.", "")]
        public Task<RoleDto> GetByIdAsync(string id)
        {
            return _roleService.GetByIdAsync(id);
        }

        [HttpGet("{id}/permissions")]
        [MustHavePermission(FSHAction.View, FSHResource.RoleClaims)]
        [OpenApiOperation("Get role details with its permissions.", "")]
        public Task<RoleDto> GetByIdWithPermissionsAsync(string id, CancellationToken cancellationToken)
        {
            return _roleService.GetByIdWithPermissionsAsync(id, cancellationToken);
        }

        [HttpPut("{id}/permissions")]
        [MustHavePermission(FSHAction.Update, FSHResource.RoleClaims)]
        [OpenApiOperation("Update a role's permissions.", "")]
        public async Task<ActionResult<string>> UpdatePermissionsAsync(string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
        {
            if (id != request.RoleId)
            {
                return BadRequest();
            }

            return Ok(await _roleService.UpdatePermissionsAsync(request, cancellationToken));
        }

        [HttpPost]
        [MustHavePermission(FSHAction.Create, FSHResource.Roles)]
        [OpenApiOperation("Create or update a role.", "")]
        public Task<string> RegisterRoleAsync(CreateOrUpdateRoleRequest request)
        {
            return _roleService.CreateOrUpdateAsync(request);
        }

        [HttpDelete("{id}")]
        [MustHavePermission(FSHAction.Delete, FSHResource.Roles)]
        [OpenApiOperation("Delete a role.", "")]
        public Task<string> DeleteAsync(string id)
        {
            return _roleService.DeleteAsync(id);
        }


        [HttpPost("{id}/departmentrole")]
        [MustHavePermission(FSHAction.Update, FSHResource.Users)]
        [OpenApiOperation("Update a new user.", "")]
        public Task<string> ChangeDeaprtmentAsync(string id, List<CreateDepartmentRole> request, CancellationToken cancellationToken)
        {
            // TODO: check if registering anonymous users is actually allowed (should probably be an appsetting)
            // and return UnAuthorized when it isn't
            // Also: add other protection to prevent automatic posting (captcha?)
            return _roleService.CreateDepartmentRolesAsync(request, id, cancellationToken);
        }


        [HttpGet("departmentroleslist/{id}")]
        [MustHavePermission(FSHAction.View, FSHResource.Roles)]
        [OpenApiOperation("Get a list of all roles.", "")]
        public Task<List<DepartmentRoleDto>> GetDepartmentRolesListAsync(string id, CancellationToken cancellationToken)
        {
            return _roleService.GetDepartmentRolesListAsync(id, cancellationToken);
        }
    }
}