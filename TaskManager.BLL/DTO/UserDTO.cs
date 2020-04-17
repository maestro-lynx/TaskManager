using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public byte[] ProfileImage { get; set; }
        public string Role { get; set; }
        public string Job { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int DepartmentId { get; set; }
    }
}
