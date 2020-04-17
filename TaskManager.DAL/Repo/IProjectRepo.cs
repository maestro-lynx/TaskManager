using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Entities;

namespace TaskManager.DAL.Repo
{
    public interface IProjectRepo : IRepo<Project>
    {
        Task<Project> GetByNameAsync(string name);
    }
}
