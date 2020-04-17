using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskManager.DAL.Entities;
using TaskManager.DAL.EntityFramework;

namespace TaskManager.DAL.Identity
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> store)
            : base(store)
        {

        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            ///It is based on the same context as the ApplicationUserManager
            return new AppRoleManager(new 
                RoleStore<AppRole>(context.Get<DBContext>()));

        }
    }
}
