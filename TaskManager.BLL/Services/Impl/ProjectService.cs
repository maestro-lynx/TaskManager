using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;
using TaskManager.DAL.Entities;
using TaskManager.DAL.Repo;

namespace TaskManager.BLL.Services.Impl
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepo db;
        public ProjectService(IProjectRepo projectRepo)
        {
            db = projectRepo;
        }
        public async Task<OperationDetails> CreateAsync(ProjectDTO item)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            Project project = mapper.Map<ProjectDTO, Project>(item);

            if (project != null)
            {
                db.Create(project);

                await db.SaveChangesAsync();

                return new OperationDetails(true, "Задача успешно создано", "");
            }
            else
            {
                return new OperationDetails(true, "Произошла ошибка при создании задачи", "ProjectId");
            }
        }

        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Project project = db.Get(id);
            if (project != null)
            {
                db.Delete(project);
                await db.SaveChangesAsync();
                return new OperationDetails(true, "Задача успешно удалена", "");
            }
            else
            {
                return new OperationDetails(true, "Произошла ошибка при удалении", "ProjectId");
            }
        }

        public async Task<ProjectDTO> FindByIdAsync(int id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config); 
            Project project = await db.GetAsync(id);
            if (project != null)
            {
                ProjectDTO projectDto = mapper.Map<Project, ProjectDTO>(project);
                return projectDto;
            }
            else return null;
        }

        public IQueryable<ProjectDTO> FindByName(string name)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            var projects = db.List().Where(x => x.Title == name);
            ICollection<ProjectDTO> projectsDto = new List<ProjectDTO>();
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    ProjectDTO projectDto = mapper.Map<Project, ProjectDTO>(project);
                    projectsDto.Add(projectDto);
                }
            }
            return projectsDto.AsQueryable();
        }
        public IQueryable<ProjectDTO> GetUserReceivedById(string id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            ICollection<ProjectDTO> result = new List<ProjectDTO>();
            var projects = db.List().Where(d=>d.ToUserId == id);

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    ProjectDTO projectDTO = mapper.Map<Project, ProjectDTO>(project);
                    result.Add(projectDTO);
                }
            }
            return result.AsQueryable();
        }
        public IQueryable<ProjectDTO> GetUserDeliveredById(string id)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            ICollection<ProjectDTO> result = new List<ProjectDTO>();
            var projects = db.List().Where(d => d.FromUserId == id);

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    ProjectDTO projectDTO = mapper.Map<Project, ProjectDTO>(project);
                    result.Add(projectDTO);
                }
            }
            return result.AsQueryable();
        }
        public IQueryable<ProjectDTO> GetAll()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            ICollection<ProjectDTO> result = new List<ProjectDTO>();
            var projects = db.List();

            if (projects != null)
            {
                foreach (var project in projects)
                {
                    ProjectDTO projectDTO = mapper.Map<Project, ProjectDTO>(project);
                    result.Add(projectDTO);
                }
            }
            return result.AsQueryable();
        }

        public async Task<OperationDetails> UpdateByIdAsync(ProjectDTO projectDto)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);

            Project project = mapper.Map<ProjectDTO, Project>(projectDto);

            if (project != null)
            {
                try
                {
                    db.Update(project);
                    await db.SaveChangesAsync();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                return new OperationDetails(true, "Задача успешно обновлена", "");
            }
            else
            {
                return new OperationDetails(true, "Произошла ошибка при обновлении", "ProjectId");
            }
        }
    }
}
