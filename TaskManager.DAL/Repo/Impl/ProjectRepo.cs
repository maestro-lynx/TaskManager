using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Entities;
using TaskManager.DAL.EntityFramework;
using System.Data.Entity;

namespace TaskManager.DAL.Repo.Impl
{
    public class ProjectRepo : IProjectRepo
    {
        public DBContext db;
        public ProjectRepo()
        {
            db = new DBContext();
        }
     
        public void Create(Project item)
        {
            db.Projects.Add(item);
        }

        public void Delete(Project item)
        {
            db.Projects.Remove(item);
        }
        public void Update(Project item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public Project Get(int id)
        {
            Project item = db.Projects.Find(id);
            return item;
        }

        public async Task<Project> GetAsync(int id)
        {
            Project item = await db.Projects.FindAsync(id);
            return item;
        }

        public async Task<Project> GetByNameAsync(string name)
        {
            Project item = await db.Projects.Where(x => x.Title == name)
                .SingleOrDefaultAsync();
            return item;
        }

        public IQueryable<Project> List()
        {
            return db.Projects.AsQueryable();
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
