using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class DepartmentRole
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid RoleId { get; set; }

        public DepartmentRole(Guid departmentId, Guid roleId)
        {
            DepartmentId = departmentId;
            RoleId = roleId;
        }
    }
}