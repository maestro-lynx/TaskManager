using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DAL.Repo
{
    public interface IRepo<T> where T : class
    {
        IQueryable<T> List();
        T Get(int id);
        Task<T> GetAsync(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        void SaveChanges();
        Task SaveChangesAsync();

    }
}
