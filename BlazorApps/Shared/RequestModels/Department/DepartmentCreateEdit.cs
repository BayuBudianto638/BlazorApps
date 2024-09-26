using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestModels.Department
{
    public class DepartmentCreateEdit
    {
        public Guid Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; } = true;
    }
}