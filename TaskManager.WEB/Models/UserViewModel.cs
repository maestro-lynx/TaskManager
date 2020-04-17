using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.WEB.Models
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
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Отделение")]
        public int DepartmentId { get; set; }
        [Display(Name = "Полное имя")]
        public string FullName { get; set; }
        [Display(Name = "Отделение")]
        public string DepartmentName { get; set; }

    }
    public class UserMenuViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Surname { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? CreatedAt { get; set; }
        public string DepartmentName { get; set; }
        public string Job { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вы должны ввести ваше имя")]
        public string RealName { get; set; }
        [Required(ErrorMessage = "Вы должны ввести вашу фамилию")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Вы должны ввести имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Вы должны ввести почту")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Введите корректный почтовый адрес")]
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Вы должны ввести корректный пароль")]
        [StringLength(16, MinimumLength =6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        public HttpPostedFileBase ProfileImage { get; set; }
    }
}