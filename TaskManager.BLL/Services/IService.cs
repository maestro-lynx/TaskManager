using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.Infrastructure;

namespace TaskManager.BLL.Services
{
    public interface IService<T> where T : class
    {
        Task<OperationDetails> CreateAsync(T item);
        Task<T> FindByIdAsync(int id);
        IQueryable<T> GetAll();
        Task<OperationDetails> DeleteByIdAsync(int id);
    }
}
