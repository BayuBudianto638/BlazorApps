namespace Application.Identity.Departments
{
    public class DepartmentByNameSpec : Specification<Department>, ISingleResultSpecification
    {
        public DepartmentByNameSpec(string name) =>
            Query.Where(b => b.Name == name);
    }
}