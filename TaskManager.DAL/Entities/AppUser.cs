using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DAL.Entities
{
    public class AppUser : IdentityUser
    {
        public string RealName { get; set; }
        public string Surname { get; set; }
        public byte[] ProfileImage { get; set; }
        public DateTime? CreatedAt { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public string Job { get; set; }
        public virtual ICollection<Project> Received { get; set; }
        public virtual ICollection<Project> Delivered { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public AppUser() : base()
        {

        }

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity =
                await manager.CreateIdentityAsync
                (this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }
}
