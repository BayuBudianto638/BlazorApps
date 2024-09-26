using Client.Infrastructure.ApiClient;
using Client.Infrastructure.ApiClientManagers;
using Client.Infrastructure.Auth;
using Client.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Authorization;
using Shared.RequestModels.Identity;
using Shared.ResponceModels.Department;

namespace Client.Pages.Identity.Users
{
    public partial class DepartmentRoles
    {
        [Parameter]
        public string? Id { get; set; }
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;
        [Inject]
        protected IUsersClient UsersClient { get; set; } = default!;

        private List<RoleDto> _rolesList = default!;
        private List<RoleDto> _departmentRolesList = default!;
        [Inject]
        private IRolesClient RolesClient { get; set; } = default!;
        [Inject]
        IDepartmentClient DepartmentClient { get; set; } = default!;
        private string _title = string.Empty;
        private string _description = string.Empty;

        private string _searchString = string.Empty;

        private bool _canEditUsers;
        private bool _canSearchRoles;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {

            var state = await AuthState;
            _canEditUsers = await AuthService.HasPermissionAsync(state.User, FSHAction.Update, FSHResource.Users);
            _canSearchRoles = await AuthService.HasPermissionAsync(state.User, FSHAction.View, FSHResource.UserRoles);
            var departmentDto = await DepartmentClient.DepartmentDtoGetByIdAsync(Guid.Parse(Id));
            if (departmentDto != null)
            {
                _title = string.Format("Department : {0}", departmentDto.Name);
                _description = string.Format(L["Manage {0}'s Roles"], departmentDto.Name);

                if (await ApiHelper.ExecuteCallGuardedAsync(
                        () => RolesClient.GetListAsync(), Snackbar)
                    is ICollection<RoleDto> response)
                {
                    _rolesList = response.ToList();
                    if (await ApiHelper.ExecuteCallGuardedAsync(
                           () => RolesClient.GetDepartmentRoleListAsync(Id), Snackbar)
                       is ICollection<DepartmentRoleDto> addedDepartment)
                    {
                        foreach (var item in _rolesList)
                        {
                            item.Enabled = false;
                            item.Enabled = addedDepartment.Where(x => x.RoleId == Guid.Parse(item.Id)).Any();

                        }

                    }
                }
                _loaded = true;
            }
            else
            {
                _title = string.Empty;
                Console.WriteLine("empty");
            }

        }

        private async Task SaveAsync()
        {
            List<CreateDepartmentRole> _rolesListAdded = new List<CreateDepartmentRole>();
            foreach (RoleDto roleDto in _rolesList)
            {
                if (roleDto.Enabled)
                {
                    CreateDepartmentRole createDepartmentRole = new CreateDepartmentRole()
                    {
                        //Id = Id,
                        DepartmentId = Guid.Parse(Id),
                        RoleId = Guid.Parse(roleDto.Id)
                    };
                    _rolesListAdded.Add(createDepartmentRole);
                }
            }

            if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => RolesClient.CreateDepartmentRoleAsync(Id, _rolesListAdded, CancellationToken.None),
                    Snackbar,
                    successMessage: L["Updated User Roles."])
                is not null)
            {
                Navigation.NavigateTo("/department");
            }
        }

        private bool Search(RoleDto role) =>
            string.IsNullOrWhiteSpace(_searchString)
                || role.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) is true;
    }
}