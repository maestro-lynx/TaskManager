using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Entities;
using TaskManager.DAL.EntityFramework;

namespace TaskManager.DAL.Repo.Impl
{
    public class DepartmentRepo : IDepartmentRepo
    {
        public DBContext db;
        public DepartmentRepo()
        {
            db = new DBContext();
        }

        public void Create(Department item)
        {
            db.Departments.Add(item);
        }

        public void Delete(Department item)
        {
            db.Departments.Remove(item);
        }
        public void Update(Department item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public Department Get(int id)
        {
            Department item = db.Departments.Find(id);
            return item;
        }

        public async Task<Department> GetAsync(int id)
        {
            Department item = await db.Departments.FindAsync(id);
            return item;
        }

        public async Task<Department> GetByNameAsync(string name)
        {
            Department item = await db.Departments.Where(x => x.DName == name)
                .SingleOrDefaultAsync();
            return item;
        }

        public IQueryable<Department> List()
        {
            return db.Departments.AsQueryable();
        }


        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
