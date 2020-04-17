using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.WEB.Models
{
    public class ProjectInfo
    {
        public ProjectViewModel Project { get; set; }
        public CommentViewModel Comment { get; set; }

    }
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Статус")]
        public string Status { get; set; }
        [Display(Name = "Прогресс")]
        public int Progress { get; set; }
        [Display(Name = "Создан")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Display(Name = "Срок выполнения")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Deadline { get; set; }
        [Display(Name = "От кого")]
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        [Display(Name = "Для кого")]
        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public ProjectViewModel()
        {
            Comments = new List<CommentViewModel>();
        }
    }
}