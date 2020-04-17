using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Identity;

namespace TaskManager.DAL.Repo
{
    public interface IUserRepo
    {
        AppUserManager UserManager { get; }
        AppRoleManager RoleManager { get; }
        Task SaveChangesAsync();
    }
}
