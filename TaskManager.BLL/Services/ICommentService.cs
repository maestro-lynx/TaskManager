using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;

namespace TaskManager.BLL.Services
{
    public interface ICommentService :IService<CommentDTO>
    {
        IQueryable<CommentDTO> GetByProjectId(int id);
    }
}
