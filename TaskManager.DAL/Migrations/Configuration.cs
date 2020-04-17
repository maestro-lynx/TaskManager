namespace TaskManager.DAL.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TaskManager.DAL.Entities;
    using TaskManager.DAL.Identity;
    using TaskManager.DAL.Utility;

    internal sealed class Configuration : DbMigrationsConfiguration<TaskManager.DAL.EntityFramework.DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaskManager.DAL.EntityFramework.DBContext context)
        {
            context.Departments.Add(new Department { DName = "Не распределены" });
            PerformInitialSetup(context);
            context.SaveChanges();
        }
        public void PerformInitialSetup(TaskManager.DAL.EntityFramework.DBContext context)
        {

            AppUserManager userManager =
                   new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleManager =
                new AppRoleManager(new RoleStore<AppRole>(context));

            string roleNameAdmin = MyConstants.ROLE_NAME_ADMIN;
            string roleNameUser = MyConstants.ROLE_NAME_USER;
            string userName = MyConstants.USERNAME;
            string password = MyConstants.PASSWORD;

            if (!roleManager.RoleExists(roleNameAdmin))
            {
                roleManager.Create(new AppRole(roleNameAdmin));
            }

            if (!roleManager.RoleExists(roleNameUser))
            {
                roleManager.Create(new AppRole(roleNameUser));
            }

            AppUser user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new AppUser()
                {
                    RealName ="",
                    Surname="",
                    Email="",
                    Job="Администратор",
                    UserName = userName,
                    CreatedAt = DateTime.Now,
                    DepartmentId = 1                   
                };
                var userResult = userManager.Create(user, password);
                if (userResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, roleNameAdmin);
                }
            }
        }
    }
}
