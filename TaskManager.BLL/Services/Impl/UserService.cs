using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;
using TaskManager.DAL.Entities;
using TaskManager.DAL.Identity;
using TaskManager.DAL.Repo;

namespace TaskManager.BLL.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepo db;
        public UserService(IUserRepo userRepo)
        {
            db = userRepo;
        }

        public async Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto)
        {
            ClaimsIdentity claim = null;

            AppUser user = await db.UserManager.FindAsync
                (userDto.UserName, userDto.Password);

            if (user != null)
            {
                claim = await db.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public async Task<OperationDetails> CreateAsync(UserDTO userDto)
        {
            AppUser user = await db.UserManager.FindByNameAsync(userDto.UserName);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = userDto.UserName,
                    ProfileImage = userDto.ProfileImage,
                    CreatedAt = userDto.CreatedAt,
                    Surname = userDto.Surname,
                    RealName = userDto.RealName,
                    DepartmentId = userDto.DepartmentId,
                    Job = userDto.Job,
                    Email = userDto.Email,
                    PhoneNumber = userDto.PhoneNumber
                };


                var result =
                    await db.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                await db.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                await db.SaveChangesAsync();

                return new OperationDetails(true, "Регистрация прошла успешно", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким именем уже зарегистрирован", "Username");
            }

        }

        public async Task<OperationDetails> DeleteByIdAsync(string id)
        {
            AppUser user = await db.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await db.UserManager.DeleteAsync(user);

                await db.SaveChangesAsync();
                return new OperationDetails(true, "Пользователь удален", "");
            }
            else
            {
                return new OperationDetails(false, "Произошла ошибка при удалении пользователя", "UserId");
            }
        }
        public UserDTO FindById(string id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            AppUser user = db.UserManager.FindById(id);
            if (user != null)
            {
                UserDTO userDto = mapper.Map<AppUser, UserDTO>(user);
                return userDto;
            }
            else return null;
        }
        public async Task<UserDTO> FindByIdAsync(string id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            AppUser user = await db.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                UserDTO userDto = mapper.Map<AppUser, UserDTO>(user);
                userDto.Role = db.UserManager.GetRoles(userDto.Id).FirstOrDefault();
                return userDto;
            }
            else return null;
        }

        public async Task<UserDTO> FindByNameAsync(string userName)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            AppUser user = await db.UserManager.FindByIdAsync(userName);

            if (user != null)
            {
                UserDTO userDto = mapper.Map<AppUser, UserDTO>(user);
                return userDto;
            }
            else return null;
        }

        public IQueryable<UserDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            IQueryable<AppUser> users = db.UserManager.Users;
            List<UserDTO> usersDto = new List<UserDTO>();

            foreach (var user in users)
            {
                UserDTO userDto = mapper.Map<AppUser, UserDTO>(user);
                usersDto.Add(userDto);
            }

            return usersDto.AsQueryable();
        }
        public IQueryable<UserDTO> GetAllUsersWithRoles()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            IQueryable<AppUser> users = db.UserManager.Users;
            List<UserDTO> usersDto = new List<UserDTO>();

            foreach (var user in users)
            {
                UserDTO userDto = mapper.Map<AppUser, UserDTO>(user);
                usersDto.Add(userDto);
            }
            usersDto.Select(x => x.Role = db.UserManager.GetRoles(x.Id).FirstOrDefault()).ToList();
            return usersDto.AsQueryable();
        }
        public List<string> GetAllRoles()
        {
            var roles = db.RoleManager.Roles;
            return roles.Select(x => x.Name).ToList();
        }

        public string GetUserFullNameById(string id)
        {
            AppUser user = db.UserManager.FindById(id);
            if (user != null)
            {
                return String.Format(user.RealName + " " + user.Surname);
            }
            return "Не найдено";

        }
        public async Task<OperationDetails> UpdateAsync(UserDTO userDto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            AppUser user = await db.UserManager.FindByIdAsync(userDto.Id);
            user = mapper.Map(userDto, user);

            if (user != null)
            {
                var result = await db.UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await db.SaveChangesAsync();
                    return new OperationDetails(true, "Данные успешно обновлены", "");
                }
                else
                {
                    return new OperationDetails(false, "Произошла ошибка при обновлении данных", "UserId");
                }
            }
            else
            {
                return new OperationDetails(false, "Произошла ошибка при обновлении данных", "UserId");
            }

        }

        public async Task<OperationDetails> UpdateByIdAsync(string id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            UserDTO userDto = await FindByIdAsync(id);
            AppUser user = mapper.Map<UserDTO, AppUser>(userDto);

            if (user != null)
            {
                var result = await db.UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await db.SaveChangesAsync();
                    return new OperationDetails(true, "Данные успешно обновлены", "");
                }
                else
                {
                    return new OperationDetails(false, "Произошла ошибка при обновлении данных", "UserId");
                }
            }
            else
            {
                return new OperationDetails(false, "Произошла ошибка при обновлении данных", "UserId");
            }
        }
    }
}
