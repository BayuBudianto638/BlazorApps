namespace Application.Identity.Departments
{
    public class CreateDepartmentRequest : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }

    public class CreateDepartmentRequestValidator : CustomValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentRequestValidator(IReadRepository<Department> repository, IStringLocalizer<CreateDepartmentRequestValidator> localizer) =>
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(75)
                .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new DepartmentByNameSpec(name), ct) is null)
                    .WithMessage((_, name) => string.Format(localizer["Department.alreadyexists"], name));
    }

    public class CreateDepartmentRequestHandler : IRequestHandler<CreateDepartmentRequest, Result<Guid>>
    {
        // Add Domain Events automatically by using IRepositoryWithEvents
        private readonly IRepositoryWithEvents<Department> _repository;

        public CreateDepartmentRequestHandler(IRepositoryWithEvents<Department> repository) => _repository = repository;

        public async Task<Result<Guid>> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var department = new Department(request.Name, request.Description, request.Status);

            await _repository.AddAsync(department, cancellationToken);

            return await Result<Guid>.SuccessAsync(department.Id, "Saved Successfully");
        }
    }
}