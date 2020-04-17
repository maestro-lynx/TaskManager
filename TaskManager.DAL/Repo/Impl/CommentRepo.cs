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
    public class CommentRepo : ICommentRepo
    {
        private DBContext db;
        public CommentRepo(DBContext context)
        {
            db = new DBContext();
        }
        public void Create(Comment item)
        {
            db.Comments.Add(item);
        }

        public void Delete(Comment item)
        {
            db.Comments.Remove(item);
        }
        public void Update(Comment item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public Comment Get(int id)
        {
            Comment item = db.Comments.Find(id);
            return item;
        }

        public async Task<Comment> GetAsync(int id)
        {
            Comment item = await db.Comments.FindAsync(id);
            return item;
        }


        public IQueryable<Comment> List()
        {
            return db.Comments.AsQueryable();
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
