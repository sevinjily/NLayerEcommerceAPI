using Business.Abstract;
using Entities.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(Roles= "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateRoleDTO createRoleDTO)
        {
            var result=await _roleService.CreateRole(createRoleDTO);
            return Ok(result);
        }

        [HttpDelete("[action]/{roleId}")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var result = await _roleService.DeleteRole(roleId);
            return Ok(result);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(UpdateRoleDTO updateRoleDTO)
        {
            var result = await _roleService.UpdateRole(updateRoleDTO);
            return Ok(result);
        }
        [HttpPost("[action]/{userId}")]

        public async Task<IActionResult> AssignRoleToUser([FromRoute] string userId, [FromBody] string role)
        {
            var result = await _roleService.AssignRoleToUserAsync(userId, role);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("[action]/{userId})")]
        public async Task<IActionResult> RemoveRoleFromUser([FromRoute]string userId,[FromBody] string roleId)
        {
            var result = await _roleService.RemoveRoleFromUser(userId, roleId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

    }
}
