using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DAL.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string DName { get; set; }
        public virtual ICollection<AppUser> Users { get; set; }
    }
}
