using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BLL.DTO;
using TaskManager.BLL.Infrastructure;

namespace TaskManager.BLL.Services
{
    public interface IUserService
    {
        Task<OperationDetails> CreateAsync(UserDTO userDto);
        Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto);
        UserDTO FindById(string id);
        Task<UserDTO> FindByIdAsync(string id);
        IQueryable<UserDTO> GetAll();
        IQueryable<UserDTO> GetAllUsersWithRoles();
        List<string> GetAllRoles();
        string GetUserFullNameById(string id);
        Task<OperationDetails> DeleteByIdAsync(string id);
        Task<OperationDetails> UpdateAsync(UserDTO userDto);
        Task<UserDTO> FindByNameAsync(string userName);
        Task<OperationDetails> UpdateByIdAsync(string id);
    }
}
