using Application.Identity.Departments;
using BlazorApps.Server.Controllers;

namespace BlazorApps.Server.Controllers.Identity
{
    public class DepartmentsController : VersionNeutralApiController
    {
        [HttpPost("search")]
        [MustHavePermission(FSHAction.Search, FSHResource.Departments)]
        [OpenApiOperation("Search departments using available Filters.", "")]
        public async Task<IActionResult> SearchAsync(SearchDepartmentsRequest request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("{id:guid}")]
        [MustHavePermission(FSHAction.View, FSHResource.Departments)]
        [OpenApiOperation("Get department details.", "")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(await Mediator.Send(new GetDepartmentRequest(id)));
        }

        [HttpPost]
        [MustHavePermission(FSHAction.Create, FSHResource.Departments)]
        [OpenApiOperation("Create a new department.", "")]
        public async Task<IActionResult> CreateAsync(CreateDepartmentRequest request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(FSHAction.Update, FSHResource.Departments)]
        [OpenApiOperation("Update a department.", "")]
        public async Task<IActionResult> UpdateAsync(UpdateDepartmentRequest request, Guid id)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:guid}")]
        [MustHavePermission(FSHAction.Delete, FSHResource.Departments)]
        [OpenApiOperation("Delete a department.", "")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteDepartmentRequest(id)));
        }

    }
}