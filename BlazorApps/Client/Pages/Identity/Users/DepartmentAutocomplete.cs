using Client.Infrastructure.ApiClientManagers;
using Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Shared.Common;
using Shared.ResponceModels.Department;

namespace Client.Pages.Identity.Users
{
    public class DepartmentAutocomplete : MudAutocomplete<Guid>
    {
        [Inject]
        private IStringLocalizer<DepartmentAutocomplete> L { get; set; } = default!;
        [Inject]
        private IDepartmentClient DepartmentClient { get; set; } = default!;
        [Inject]
        private ISnackbar Snackbar { get; set; } = default!;

        private List<DepartmentDto> _departmentDto = new();

        // supply default parameters, but leave the possibility to override them
        public override Task SetParametersAsync(ParameterView parameters)
        {
            var dict = parameters.ToDictionary();
            if (!dict.ContainsKey(nameof(Label)))
                Label = L["Department"];
            if (!dict.ContainsKey(nameof(Variant)))
                Variant = Variant.Filled;
            if (!dict.ContainsKey(nameof(Dense)))
                Dense = true;
            if (!dict.ContainsKey(nameof(Margin)))
                Margin = Margin.Dense;
            if (!dict.ContainsKey(nameof(ResetValueOnEmptyText)))
                ResetValueOnEmptyText = true;
            if (!dict.ContainsKey(nameof(SearchFunc)))
                SearchFunc = SearchDepartments;
            if (!dict.ContainsKey(nameof(ToStringFunc)))
                ToStringFunc = GetDepartmentName;
            if (!dict.ContainsKey(nameof(Clearable)))
                Clearable = true;
            return base.SetParametersAsync(parameters);
        }

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
                _departmentDto = response.Data.ToList();
            }

            return _departmentDto.Select(x => x.Id);
        }

        private string GetDepartmentName(Guid id) =>
            _departmentDto.Find(b => b.Id == id)?.Name ?? string.Empty;
    }
}