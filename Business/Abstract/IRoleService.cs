using Core.Utilities.Results.Abstract;
using Entities.DTOs.RoleDTOs;

namespace Business.Abstract
{
    public interface IRoleService
    {
        Task<IResult> CreateRole(CreateRoleDTO model);
        Task<IResult> DeleteRole(string roleId);
        Task<IResult> UpdateRole(UpdateRoleDTO model);
        Task<IResult> AssignRoleToUserAsync(string userId, string role);
        Task<IResult> RemoveRoleFromUser(string userId, string roleId);
        Task<IDataResult<List<GetRoleDTO>>> GetAllRoles();
        Task<IDataResult<GetRoleDTO>> GetRoleById(string roleId);
    }
}
