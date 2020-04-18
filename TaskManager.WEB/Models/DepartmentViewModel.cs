﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace TaskManager.WEB.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Отделение")]
        public string DName { get; set; }
    }
}