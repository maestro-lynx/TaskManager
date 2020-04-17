using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using TaskManager.DAL.Entities;
using TaskManager.DAL.EntityFramework;
using TaskManager.DAL.Identity;

namespace TaskManager.DAL.Repo.Impl
{
    public class UserRepo : IUserRepo
    {
        private readonly DBContext db;
        private AppUserManager userManager;
        private AppRoleManager roleManager;
        public UserRepo()
        {
            db = new DBContext();
        }
        public AppUserManager UserManager
        {
            get
            {
                if (userManager == null)
                    userManager = new AppUserManager(
                new UserStore<AppUser>(db));
                return userManager;
            }
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
