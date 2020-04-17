using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Progress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Deadline { get; set; }
        public string FromUserId { get; set; }
        public virtual AppUser FromUser { get; set; }
        public string ToUserId { get; set; }
        public virtual AppUser ToUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
