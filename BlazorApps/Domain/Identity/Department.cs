using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class Department : AuditableEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

        public Department(string name, string? description, bool status)
        {
            Name = name;
            Description = description;
            Status = status;
        }

        public Department Update(string? name, string? description, bool ststus)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (description is not null && Description?.Equals(description) is not true) Description = description;
            Status = ststus;
            return this;
        }
    }
}