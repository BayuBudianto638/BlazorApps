using Application.Common.Interfaces.UserExistingCheck;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class DepartmentUse : IDepartmentUse
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public DepartmentUse(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsDepartmentUsed(Guid departmentId)
        {
            bool isDepartmentUsed = false;
            var user = await _userManager.Users.Where(u => u.DepartmentId == departmentId).FirstOrDefaultAsync();
            if (user != null)
            {
                isDepartmentUsed = true;
            }
            return isDepartmentUsed;
        }

        public async Task<Guid> GetDepartmentId(Guid userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId.ToString()).FirstOrDefaultAsync();
            return user.DepartmentId;
        }
    }
}