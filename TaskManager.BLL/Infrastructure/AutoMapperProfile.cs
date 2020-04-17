using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.DAL.Entities;

namespace TaskManager.BLL.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserDTO>().ReverseMap();
            CreateMap<AppRole, RoleDTO>().ReverseMap();
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<AppRole, RoleDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
        }
    }
}
