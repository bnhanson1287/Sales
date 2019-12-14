using eRaceSystem.BLL;
using eRaceSystem.DataModels;
using eRaceWebApp.Admin.Security;
using eRaceWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static eRaceWebApp.Admin.Security.Settings;

namespace eRaceWebApp.Admin.SecurityDbContextInitializer
{
    public class SecurityDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            #region Seed Security Roles
            // Administrator Role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                roleManager.Create(new IdentityRole(AdminRole));

            // Employee role
            var controller = new eRaceController();
            var userRoles = controller.ListPositions();
            foreach (var user in userRoles)
            {
                roleManager.Create(new IdentityRole(user.Position));
            }

            

            #endregion

            #region Seed the users
            string adminUser = ConfigurationManager.AppSettings["adminUserName"];
            string adminEmail = ConfigurationManager.AppSettings["adminEmail"];
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var result = userManager.Create(new ApplicationUser
            {
                UserName = adminUser,
                Email = adminEmail,
                EmailConfirmed = true
            }, adminPassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(adminUser).Id, Settings.AdminRole);

            //employee accounts 
            string defaultPassword = ConfigurationManager.AppSettings["defaultPassword"];
            string emailDomain = ConfigurationManager.AppSettings["companyDomain"];
            IEnumerable<EmployeePositions> employees = controller.ListEmployeeAndPosition(emailDomain);
            foreach(var person in employees)
            {
                result = userManager.Create(new ApplicationUser
                {
                    UserName = person.UserName,
                    Email = person.EmailAddress,
                    EmailConfirmed = true,
                    EmployeeId = person.UserID,
                    Position = person.Title

                }, defaultPassword);
                if (result.Succeeded)
                {
                    userManager.AddToRole(userManager.FindByName(person.UserName).Id, person.Title);
                }

            }
            #endregion


            base.Seed(context);
        }
    }
}