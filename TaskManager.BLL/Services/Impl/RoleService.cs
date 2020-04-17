using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;
using TaskManager.DAL.Entities;
using TaskManager.DAL.Repo;

namespace TaskManager.BLL.Services.Impl
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo db;
        public RoleService(IRoleRepo roleRepo)
        {
            db = roleRepo;
        }
        public async Task<OperationDetails> CreateAsync(RoleDTO roleDto)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            
            AppRole role = await db.RoleManager.FindByNameAsync(roleDto.RoleName);

            if (role == null)
            {
                role = mapper.Map<RoleDTO, AppRole>(roleDto);

                var result = await db.RoleManager.CreateAsync(role);

                if (result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                await db.SaveChangesAsync();

                return new OperationDetails(true, "Роль создано успешно", "");
            }
            else
            {
                return new OperationDetails(false, "Роль с таким именем уже есть", "RoleName");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(string id)
        {
            AppRole role = await db.RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                await db.RoleManager.DeleteAsync(role);
                await db.SaveChangesAsync();
                return new OperationDetails(true, "Роль удален", "");
            }
            else
            {
                return new OperationDetails(true, "Произошла ошибка при удалении", "RoleId");
            }
        }


        public async Task<RoleDTO> FindByIdAsync(string id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            AppRole role = await db.RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                RoleDTO roleDto = mapper.Map<AppRole, RoleDTO>(role);
                return roleDto;
            }
            else return null;
        }

        public IQueryable<RoleDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            IQueryable<AppRole> roles = db.RoleManager.Roles;
            List<RoleDTO> rolesDto = new List<RoleDTO>();

            foreach (var role in roles)
            {
                RoleDTO roleDto = mapper.Map<AppRole, RoleDTO>(role);
                rolesDto.Add(roleDto);
            }

            return rolesDto.AsQueryable();
        }
    }
}
