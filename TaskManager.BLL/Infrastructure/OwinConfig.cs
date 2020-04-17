using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.EntityFramework;
using TaskManager.DAL.Identity;

namespace TaskManager.BLL.Infrastructure
{
    public static class OwinConfig
    {
        public static void Configure(IAppBuilder app)
        {
            app.CreatePerOwinContext(DBContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
            app.CreatePerOwinContext<AppSignInManager>(AppSignInManager.Create);
        }
    }
}
