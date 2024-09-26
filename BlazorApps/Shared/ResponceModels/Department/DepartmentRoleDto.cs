using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ResponceModels.Department
{
    public class DepartmentRoleDto
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid RoleId { get; set; }
    }
}