namespace Application.Identity.Departments
{

    public class DepartmentsByUserSpec : Specification<ApplicationUser>
    {
        public DepartmentsByUserSpec(Guid departmentId) =>
            Query.Where(p => p.DepartmentId == departmentId);
    }
}