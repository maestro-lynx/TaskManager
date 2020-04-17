using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskManager.DAL.Entities;
using TaskManager.DAL.EntityFramework;

namespace TaskManager.DAL.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            ///Calling the non-default constructor of the UserStore class
            return new AppUserManager(new 
                UserStore<AppUser>(context.Get<DBContext>()));

        }
    }
}
