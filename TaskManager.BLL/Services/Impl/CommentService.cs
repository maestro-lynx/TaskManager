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
    public class CommentService : ICommentService
    {
        private readonly ICommentRepo db;
        public CommentService(ICommentRepo commentRepo)
        {
            db = commentRepo;
        }
        public async Task<OperationDetails> CreateAsync(CommentDTO item)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            Comment comment = mapper.Map<CommentDTO, Comment>(item);

            if (comment != null)
            {
                if (string.IsNullOrEmpty(comment.Content))
                {
                    return new OperationDetails(false, "Вы ввели пустой комментарий", "");
                }
                db.Create(comment);

                await db.SaveChangesAsync();

                return new OperationDetails(true, "Комментарий добавлено успешно", "");
            }
            else
            {
                return new OperationDetails(false, "Произошла ошибка при добавлении комментарии", "");
            }
        }
        
        public async Task<OperationDetails> DeleteByIdAsync(int id)
        {
            Comment comment = db.Get(id);
            if (comment != null)
            {
                db.Delete(comment);
                await db.SaveChangesAsync();
                return new OperationDetails(true, "Комментарий удален", "");
            }
            else
            {
                return new OperationDetails(false, "Произошла ошибка при удалении комментарии", "");
            }
        }

        public async Task<CommentDTO> FindByIdAsync(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            IMapper mapper = new Mapper(config);
            Comment comment = await db.GetAsync(id);
            if (comment != null)
            {
                CommentDTO commentDto = mapper.Map<Comment, CommentDTO>(comment);
                return commentDto;
            }
            else return null;
        }
        public IQueryable<CommentDTO> GetByProjectId(int id)
        {
            ICollection<CommentDTO> result = new List<CommentDTO>();
            var comments = db.List().Where(x=>x.ProjectId == id).ToList();

            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    CommentDTO commentDto = new CommentDTO()
                    {
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt,
                        Id = comment.Id,
                        ProjectId = comment.ProjectId,
                        UserId = comment.UserId
                    };
                    result.Add(commentDto);
                }
            }

            return result.AsQueryable();
        }

        public IQueryable<CommentDTO> GetAll()
        {
            ICollection<CommentDTO> result = new List<CommentDTO>();
            var comments = db.List();

            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    CommentDTO commentDto = new CommentDTO()
                    {
                        Content = comment.Content,
                        CreatedAt = comment.CreatedAt,
                        Id = comment.Id,
                        ProjectId = comment.ProjectId,
                        UserId = comment.UserId
                    };
                    result.Add(commentDto);
                }
            }

            return result.AsQueryable();
        }
    }
}
