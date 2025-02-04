using Core.Utilities.Results.Abstract;
using Entities.Common;
using Entities.DTOs.RoleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IRoleService
    {
        Task<IResult> CreateRole(CreateRoleDTO model);
        Task<IResult> DeleteRole(string roleId);
        Task<IResult> UpdateRole(UpdateRoleDTO model);
        Task<IResult> AssignRoleToUserAsync(string userId, string role);
        Task<IResult> RemoveRoleFromUser(string userId, string roleId);
    }
}
