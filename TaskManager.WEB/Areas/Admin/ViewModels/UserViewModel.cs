using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.WEB.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Вы должны ввести ваше имя")]
        public string RealName { get; set; }
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Вы должны ввести вашу фамилию")]
        public string Surname { get; set; }
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Вы должны ввести имя пользователя")]
        public string UserName { get; set; }
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Вы должны ввести корректный пароль")]
        [StringLength(16, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Display(Name = "Роль")]
        public string Role { get; set; }
        [Display(Name = "Должность")]
        public string Job { get; set; }
        [Display(Name = "Рисунок профиля")]
        public byte[] ProfileImage { get; set; }
        [Display(Name = "Дата регистрации")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Отделение")]
        public int DepartmentId { get; set; }
        [Display(Name = "Полное имя")]
        public string FullName { get; set; }
        [Display(Name = "Отделение")]
        public string DepartmentName { get; set; }
    }
}