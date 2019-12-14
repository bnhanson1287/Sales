using eRaceSystem.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace eRaceWebApp.Admin.Security
{
    public static class Settings
    {
        public static string AdminRole => ConfigurationManager.AppSettings["adminRole"];

        //public static string EmployeeRoles => ConfigurationManager.AppSettings["employeeRole"];

    }
}