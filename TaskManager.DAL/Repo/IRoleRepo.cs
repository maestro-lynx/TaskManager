using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Identity;

namespace TaskManager.DAL.Repo
{
    public interface IRoleRepo
    {
        AppRoleManager RoleManager { get; }
        Task SaveChangesAsync();
    }
}
