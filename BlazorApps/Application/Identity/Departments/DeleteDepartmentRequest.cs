using Application.Common.Interfaces.UserExistingCheck;

namespace Application.Identity.Departments
{
    public class DeleteDepartmentRequest : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }

        public DeleteDepartmentRequest(Guid id) => Id = id;
    }

    public class DeleteDepartmentRequestHandler : IRequestHandler<DeleteDepartmentRequest, Result<Guid>>
    {
        // Add Domain Events automatically by using IRepositoryWithEvents
        private readonly IRepositoryWithEvents<Department> _departmentRepo;

        private readonly IStringLocalizer<DeleteDepartmentRequestHandler> _localizer;

        private readonly IDepartmentUse _usedDepartment;

        public DeleteDepartmentRequestHandler(IRepositoryWithEvents<Department> departmentRepo,
            IStringLocalizer<DeleteDepartmentRequestHandler> localizer, IDepartmentUse usedDepartment)
        {
            (_departmentRepo, _localizer) = (departmentRepo, localizer);
            _usedDepartment = usedDepartment;
        }


        public async Task<Result<Guid>> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
        {
            if (await _usedDepartment.IsDepartmentUsed(request.Id))
            {
                throw new ConflictException(_localizer["department.cannotbedeleted"]);
            }

            var department = await _departmentRepo.GetByIdAsync(request.Id, cancellationToken);

            _ = department ?? throw new NotFoundException(_localizer["brand.notfound"]);

            await _departmentRepo.DeleteAsync(department, cancellationToken);
            return await Result<Guid>.SuccessAsync(request.Id, "Delete Successfully");
            //return request.Id;
        }
    }
}