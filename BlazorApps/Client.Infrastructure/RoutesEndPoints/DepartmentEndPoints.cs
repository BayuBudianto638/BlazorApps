using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.RoutesEndPoints
{
    public static class DepartmentEndPoints
    {
        public static string GetAll = "api/Departments/search";
        public static string Get = "api/Departments";
        public static string Create = "api/Departments";
        public static string Edit = "api/Departments/";
        public static string Delete = "api/Departments";
    }
}