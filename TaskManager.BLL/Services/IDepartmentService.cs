using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;

namespace TaskManager.BLL.Services
{
    public interface IDepartmentService : IService<DepartmentDTO>
    {
        String GetNameById(int id);
        Task<OperationDetails> UpdateByIdAsync(DepartmentDTO DepartmentDTO);
    }
}
