using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;

namespace TaskManager.BLL.Services
{
    public interface IRoleService
    {
        Task<OperationDetails> CreateAsync(RoleDTO roleDto);
        Task<RoleDTO> FindByIdAsync(string id);
        IQueryable<RoleDTO> GetAll();
        Task<OperationDetails> DeleteByIdAsync(string id);
    }
}
