using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccesResults;
using Entities.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleManager(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IResult> AssignRoleToUserAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new ErrorResult(HttpStatusCode.NotFound);
            await _userManager.AddToRoleAsync(user, role);
            return new SuccessResult(HttpStatusCode.OK);
        }
        public async Task<IResult> CreateRole(CreateRoleDTO model)
        {
            var existingRole = await _roleManager.FindByNameAsync(model.RoleName);
            if (existingRole != null)
            {
                return new ErrorResult("This role is already exist!",HttpStatusCode.BadRequest);
            }
            await _roleManager.CreateAsync(new AppRole
            {
                Name = model.RoleName
            });
            return new SuccessResult(HttpStatusCode.Created);
        }

        public async Task<IResult> DeleteRole(string roleId)
        {
           var findRole=_roleManager.FindByIdAsync(roleId);
            if (findRole is null)
               return new ErrorResult("User not found",HttpStatusCode.NotFound);
            await _roleManager.DeleteAsync(findRole.Result);
            return new SuccessResult(HttpStatusCode.OK);
        }

        public async Task<IDataResult<List<GetRoleDTO>>> GetAllRoles()
        {
            var roles= _roleManager.Roles.Select(x => new GetRoleDTO {RoleId=x.Id, RoleName = x.Name }).ToList();
            return new ErrorDataResult<List<GetRoleDTO>>(roles, HttpStatusCode.OK);
        }

        public async Task<IDataResult<GetRoleDTO>> GetRoleById(string roleId)
        {
            var findRole = await _roleManager.FindByIdAsync(roleId);
            if (findRole is null)
                return new ErrorDataResult<GetRoleDTO>("Role not found", HttpStatusCode.NotFound);
            return new SuccessDataResult<GetRoleDTO>(new 
                GetRoleDTO { RoleId = findRole.Id, RoleName = findRole.Name }, HttpStatusCode.OK);
        }

        public async Task<IResult> RemoveRoleFromUser(string userId, string roleId)
        {
            var findUser = await _userManager.FindByIdAsync(userId);
            if (findUser is null)
                return new ErrorResult("User not found", HttpStatusCode.NotFound);
            

                var findRole = await _roleManager.FindByIdAsync(roleId);
                if (findRole is null)
                    return new ErrorResult("Role not found", HttpStatusCode.NotFound);
          
           var result=await _userManager.RemoveFromRoleAsync(findUser, findRole.Name);
            if(result.Succeeded)
            {
                return new SuccessResult(HttpStatusCode.OK);
            }
            else
            {
                return new ErrorResult(result.Errors.ToString(), HttpStatusCode.BadRequest);


            }
        }

        public async Task<IResult> UpdateRole(UpdateRoleDTO model)
        {
            var findRole = await _roleManager.FindByIdAsync(model.RoleId);
            if (findRole is null)
                return new ErrorResult("Role not found", HttpStatusCode.NotFound);
            findRole.Name = model.NewRoleName;
            await _roleManager.UpdateAsync(findRole);
            return new SuccessResult(HttpStatusCode.OK);

        }
    }
}
