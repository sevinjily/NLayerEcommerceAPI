using Asp.Versioning;
using Business.Abstract;
using Entities.DTOs.RoleDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V1
{
    [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create(CreateRoleDTO createRoleDTO)
        {
            var result=await _roleService.CreateRole(createRoleDTO);
            return Ok(result);
        }

        [HttpDelete("[action]/{roleId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var result = await _roleService.DeleteRole(roleId);
            return Ok(result);
        }
        [HttpPut("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(UpdateRoleDTO updateRoleDTO)
        {
            var result = await _roleService.UpdateRole(updateRoleDTO);
            return Ok(result);
        }
        [HttpPost("[action]/{userId}")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> AssignRoleToUser([FromRoute] string userId, [FromBody] string role)
        {
            var result = await _roleService.AssignRoleToUserAsync(userId, role);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("[action]/{userId})")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RemoveRoleFromUser([FromRoute]string userId,[FromBody] string roleId)
        {
            var result = await _roleService.RemoveRoleFromUser(userId, roleId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("[action]")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();
            return Ok(result);
        }
        [HttpGet("[action]/{roleId}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            var result = await _roleService.GetRoleById(roleId);
            return Ok(result);
        }

    }
}
