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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo db;
        public DepartmentService(IDepartmentRepo repo)
        {
            db = repo;
        }
        public async Task<OperationDetails> CreateAsync(DepartmentDTO item)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            Department department = mapper.Map<DepartmentDTO, Department>(item);

            if (department != null)
            {
                db.Create(department);

                await db.SaveChangesAsync();

                return new OperationDetails(true, "Отделение успешно создан", "");
            }
            else
            {
                return new OperationDetails(true, "Произошла ошибка при создании", "DepartmentId");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Department department = db.Get(id);
            if (department != null)
            {
                db.Delete(department);
                await db.SaveChangesAsync();
                return new OperationDetails(true, "Отделение успешно удален", "");
            }
            else
            {
                return new OperationDetails(true, "Произошла ошибка при удалении", "DepartmentId");
            }
        }

        public async Task<DepartmentDTO> FindByIdAsync(int id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            Department department = await db.GetAsync(id);
            if (department != null)
            {
                DepartmentDTO DepartmentDTO = mapper.Map<Department, DepartmentDTO>(department);
                return DepartmentDTO;
            }
            else return null;
        }
        public string GetNameById(int id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            Department department = db.Get(id);
            if (department != null)
            {
                DepartmentDTO departmentDTO = mapper.Map<Department, DepartmentDTO>(department);
                if (departmentDTO.Id==1)
                {
                    return "Отделение не указано";
                }
                return departmentDTO.DName;
            }
            else return null;
        }
        public IQueryable<DepartmentDTO> FindByName(string name)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            var departments = db.List().Where(x => x.DName == name);
            ICollection<DepartmentDTO> departmentsDTO = new List<DepartmentDTO>();
            if (departments != null)
            {
                foreach (var department in departments)
                {
                    DepartmentDTO DepartmentDTO = mapper.Map<Department, DepartmentDTO>(department);
                    departmentsDTO.Add(DepartmentDTO);
                }
            }
            return departmentsDTO.AsQueryable();
        }

        public IQueryable<DepartmentDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            ICollection<DepartmentDTO> result = new List<DepartmentDTO>();
            var departments = db.List();

            if (departments != null)
            {
                foreach (var department in departments)
                {
                    DepartmentDTO DepartmentDTO = mapper.Map<Department, DepartmentDTO>(department);
                    result.Add(DepartmentDTO);
                }
            }
            return result.AsQueryable();
        }

        public async Task<OperationDetails> UpdateByIdAsync(DepartmentDTO DepartmentDTO)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);

            Department department = mapper.Map<DepartmentDTO, Department>(DepartmentDTO);

            if (department != null)
            {
                db.Update(department);
                await db.SaveChangesAsync();
                return new OperationDetails(true, "Отделение успешно переименован", "");
            }
            else
            {
                return new OperationDetails(true, "Произошла ошибка при переименовании", "DepartmentId");
            }
        }
    }
}
