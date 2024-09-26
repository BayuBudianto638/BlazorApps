using Client.Infrastructure.Auth;
using Client.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Authorization;

namespace Client.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;

        private string? _hangfireUrl;
        private bool _canViewHangfire;
        private bool _canViewDashboard;
        private bool _canViewRoles;
        private bool _canViewUsers;
        private bool _canViewProducts;
        private bool _canViewBrands;
        private bool _canViewTenants;
        private bool _canViewDepartments;
        private bool CanViewAdministrationGroup => _canViewUsers || _canViewRoles || _canViewTenants || _canViewDepartments;

        protected override async Task OnParametersSetAsync()
        {
            _hangfireUrl = Config[ConfigNames.ApiBaseUrl] + "jobs";
            var user = (await AuthState).User;
            _canViewHangfire = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Hangfire);
            _canViewDashboard = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Dashboard);
            _canViewRoles = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Roles);
            _canViewUsers = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Users);
            _canViewProducts = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Products);
            _canViewBrands = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Brands);
            _canViewTenants = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Tenants);
            _canViewDepartments = await AuthService.HasPermissionAsync(user, FSHAction.View, FSHResource.Departments);
        }
    }
}