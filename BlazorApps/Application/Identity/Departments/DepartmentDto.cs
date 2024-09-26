namespace Application.Identity.Departments
{
    public class DepartmentDto : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}