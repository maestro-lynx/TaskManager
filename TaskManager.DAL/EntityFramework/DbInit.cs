using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using TaskManager.DAL.Entities;
using TaskManager.DAL.Identity;
using TaskManager.DAL.Utility;

namespace TaskManager.DAL.EntityFramework
{
    public class DbInit : DropCreateDatabaseIfModelChanges<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            PerformInitialSetup(context);        
            base.Seed(context);
            context.SaveChanges();
        }
        public void PerformInitialSetup(DBContext context)
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
                    RealName = "",
                    Surname = "",
                    Email = "",
                    Job = "Администратор",
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
