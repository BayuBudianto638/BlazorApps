using Client.Components.Common;
using Client.Infrastructure.ApiClient;
using Client.Infrastructure.ApiClientManagers;
using Client.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Common;
using Shared.RequestModels.Identity;
using Shared.ResponceModels.Department;
using System.ComponentModel.DataAnnotations;

namespace Client.Pages.Identity.Users
{
    public partial class ChangeDepartments
    {
        [Inject]
        private IDepartmentClient DepartmentClient { get; set; } = default!;
        [Inject]
        protected IUsersClient UsersClient { get; set; } = default!;
        private CustomValidation? _customValidation;

        [Parameter]
        public object? Id { get; set; }
        UserDetailsDto userDetailsDto = new();

        private string _title = string.Empty;
        private string _description = string.Empty;

        private string _searchString = string.Empty;

        private bool _canEditUsers;
        private bool _canSearchRoles;
        private bool _loaded;
        public bool Validate(object request)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), results, true))
            {
                // Convert results to errors
                var errors = new Dictionary<string, ICollection<string>>();
                foreach (var result in results
                    .Where(r => !string.IsNullOrWhiteSpace(r.ErrorMessage)))
                {
                    foreach (string field in result.MemberNames)
                    {
                        if (errors.ContainsKey(field))
                        {
                            errors[field].Add(result.ErrorMessage!);
                        }
                        else
                        {
                            errors.Add(field, new List<string>() { result.ErrorMessage! });
                        }
                    }
                }

                _customValidation?.DisplayErrors(errors);

                return false;
            }

            return true;
        }

        protected override async Task OnInitializedAsync()
        {
            userDetailsDto = await UsersClient.GetByIdAsync(Id.ToString());
            await SearchDepartments(userDetailsDto.DepartmentName);
            _departments.Where(x => x.Id == userDetailsDto.DepartmentId).Select(x => x.Id);
            _loaded = true;
        }


        private async Task SaveAsync()
        {
            DepartmentChange updateUserRequest = new DepartmentChange()
            {
                Id = userDetailsDto.Id.ToString(),
                DepartmentId = userDetailsDto.DepartmentId
            };
            await UsersClient.ChangeDepartmentAsync(updateUserRequest, Id.ToString(), CancellationToken.None);
            Navigation.NavigateTo("/users");
        }

        public List<DepartmentDto> _departments { get; set; } = new();
        private async Task<IEnumerable<Guid>> SearchDepartments(string value)
        {
            var filter = new ClientSearchRequest
            {
                PageSize = 10,
                AdvancedSearch = new() { Fields = new[] { "name" }, Keyword = value }
            };

            if (await ApiHelper.ExecuteCallGuardedAsync(
                    () => DepartmentClient.SearchAsync(filter), Snackbar)
                is PaginationResponseOfModel<DepartmentDto> response)
            {
                _departments = response.Data.ToList();
            }

            return _departments.Select(x => x.Id);
        }
        private string GetDepartmentName(Guid id) =>
           _departments.Find(b => b.Id == id)?.Name ?? string.Empty;

        private void Back() =>
           Navigation.NavigateTo("/users");
    }

}
