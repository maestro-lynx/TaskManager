using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TaskManager.DAL.Entities;
using TaskManager.DAL.Utility;
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

namespace TaskManager.DAL.EntityFramework
{
    public class DBContext : IdentityDbContext<AppUser>
    {
        public DBContext() : base(MyConstants.CONNECTION_STRING)
        {
            Database.SetInitializer(new DbInit());
        }

        public static DBContext Create()
        {
            return new DBContext();
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }





        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                 .HasRequired(m => m.FromUser)
                .WithMany(t => t.Received)
                .HasForeignKey(m => m.FromUserId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasRequired(m => m.ToUser)
                .WithMany(t => t.Delivered)
                .HasForeignKey(m => m.ToUserId)
                .WillCascadeOnDelete(false);
        }
    }
}
