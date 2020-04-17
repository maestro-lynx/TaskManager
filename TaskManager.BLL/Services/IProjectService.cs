using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;

namespace TaskManager.BLL.Services
{
    public interface IProjectService : IService<ProjectDTO>
    {
        Task<OperationDetails> UpdateByIdAsync(ProjectDTO projectDto);
        IQueryable<ProjectDTO> GetUserReceivedById(string id);
        IQueryable<ProjectDTO> GetUserDeliveredById(string id);
        IQueryable<ProjectDTO> FindByName(string name);
    }
}
