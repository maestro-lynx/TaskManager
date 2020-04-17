using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.WEB.Models
{
    public class JobViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }       
    }
}