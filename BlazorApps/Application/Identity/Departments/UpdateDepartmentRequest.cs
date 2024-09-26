


namespace Application.Identity.Departments
{
    public class UpdateDepartmentRequest : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }

    public class UpdateDepartmentRequestValidator : CustomValidator<UpdateDepartmentRequest>
    {
        public UpdateDepartmentRequestValidator(IRepository<Department> repository, IStringLocalizer<UpdateDepartmentRequestValidator> localizer) =>
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (department, name, ct) =>
                        await repository.GetBySpecAsync(new DepartmentByNameSpec(name), ct)
                            is not Department existingBrand || existingBrand.Id == department.Id)
                    .WithMessage((_, name) => string.Format(localizer["department.alreadyexists"], name));
    }

    public class UpdateDepartmentRequestHandler : IRequestHandler<UpdateDepartmentRequest, Result<Guid>>
    {
        // Add Domain Events automatically by using IRepositoryWithEvents
        private readonly IRepositoryWithEvents<Department> _repository;
        private readonly IStringLocalizer<UpdateDepartmentRequestHandler> _localizer;

        public UpdateDepartmentRequestHandler(IRepositoryWithEvents<Department> repository, IStringLocalizer<UpdateDepartmentRequestHandler> localizer) =>
            (_repository, _localizer) = (repository, localizer);

        public async Task<Result<Guid>> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var department = await _repository.GetByIdAsync(request.Id, cancellationToken);

            _ = department ?? throw new NotFoundException(string.Format(_localizer["department.notfound"], request.Id));

            department.Update(request.Name, request.Description, request.Status);

            await _repository.UpdateAsync(department, cancellationToken);
            return await Result<Guid>.SuccessAsync(request.Id, "Updated Successfully");

        }
    }
}