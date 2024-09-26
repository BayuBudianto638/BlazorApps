using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestModels.Identity
{
    public class DepartmentChange
    {
        [Required]
        public string Id { get; set; } = default!;
        [Required]
        public Guid DepartmentId { get; set; }
    }
}