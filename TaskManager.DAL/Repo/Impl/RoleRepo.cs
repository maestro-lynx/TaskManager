using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Entities;
using TaskManager.DAL.EntityFramework;
using TaskManager.DAL.Identity;

namespace TaskManager.DAL.Repo.Impl
{
    public class RoleRepo : IRoleRepo
    {
        private readonly DBContext db;
        private AppRoleManager roleManager;
        public RoleRepo()
        {
            db = new DBContext();
        }
        public AppRoleManager RoleManager
        {
            get
            {
                if (roleManager == null)
                    roleManager = new AppRoleManager(
                new RoleStore<AppRole>(db));
                return roleManager;
            }
        }
        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
