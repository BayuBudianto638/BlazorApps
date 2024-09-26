using Domain.Identity;
using Shared.Wrapper;

namespace Application.Identity.Departments
{
    public class SearchDepartmentsRequest : PaginationFilter, IRequest<Result<PaginationResponse<DepartmentDto>>>
    {
    }

    public class SearchDepartmentsRequestSpec : EntitiesByPaginationFilterSpec<Department, DepartmentDto>
    {
        public SearchDepartmentsRequestSpec(SearchDepartmentsRequest request)
            : base(request) =>
            Query.OrderBy(c => c.Name, !request.HasOrderBy());
    }

    public class SearchDepartmentsRequestHandler : IRequestHandler<SearchDepartmentsRequest, Result<PaginationResponse<DepartmentDto>>>
    {
        private readonly IReadRepository<Department> _repository;

        public SearchDepartmentsRequestHandler(IReadRepository<Department> repository) => _repository = repository;

        public async Task<Result<PaginationResponse<DepartmentDto>>> Handle(SearchDepartmentsRequest request, CancellationToken cancellationToken)
        {
            var spec = new SearchDepartmentsRequestSpec(request);

            var list = await _repository.ListAsync(spec, cancellationToken);
            int count = await _repository.CountAsync(spec, cancellationToken);
            var dataList = new PaginationResponse<DepartmentDto>(list, count, request.PageNumber, request.PageSize);
            return await Result<PaginationResponse<DepartmentDto>>.SuccessAsync(dataList, "success");

            // return new Result<PaginationResponse<DepartmentDto>(list, count, request.PageNumber, request.PageSize);
        }
    }
}